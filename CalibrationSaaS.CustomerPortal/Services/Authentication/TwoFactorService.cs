using CalibrationSaaS.CustomerPortal.Data;
using CalibrationSaaS.CustomerPortal.Models.Authentication;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Two-factor authentication service implementation
/// </summary>
public class TwoFactorService : ITwoFactorService
{
    private readonly CustomerPortalDbContext _dbContext;
    private readonly ITenantInfo _tenantInfo;
    private readonly ILogger<TwoFactorService> _logger;
    private readonly TwoFactorOptions _options;

    public TwoFactorService(
        CustomerPortalDbContext dbContext,
        ITenantInfo tenantInfo,
        ILogger<TwoFactorService> logger,
        IOptions<TwoFactorOptions> options)
    {
        _dbContext = dbContext;
        _tenantInfo = tenantInfo;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> GenerateCodeAsync(string email, string tenantId, string? ipAddress = null)
    {
        try
        {
            _logger.LogInformation("🔍 === 2FA CODE GENERATION START ===");
            _logger.LogInformation("📧 Email: {Email}", email);
            _logger.LogInformation("🏢 TenantId: {TenantId}", tenantId);
            _logger.LogInformation("🌐 IP Address: {IpAddress}", ipAddress ?? "Not provided");

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("❌ Email validation failed: Email is required");
                throw new ArgumentException("Email is required", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                _logger.LogWarning("❌ TenantId validation failed: TenantId is required");
                throw new ArgumentException("TenantId is required", nameof(tenantId));
            }

            // Rate limiting
            if (await IsRateLimitExceededAsync(email))
            {
                _logger.LogWarning("Rate limit exceeded for {Email}", email);
                throw new InvalidOperationException("Too many verification codes requested. Please try again later.");
            }

            // Invalidate any previous active codes for this email/tenant
            await InvalidateExistingCodesAsync(email);

            // Generate new code
            _logger.LogInformation("🎲 Generating verification code...");
            var code = _options.UseAlphanumericCodes ?
                TwoFactorCode.GenerateAlphanumericCode(6) :
                TwoFactorCode.GenerateCode(6);

            // Persist the code
            var entity = TwoFactorCode.Create(
                email: email,
                code: code,
                expirationMinutes: 10,
                maxAttempts: _options.MaxVerificationAttempts,
                ipAddress: ipAddress,
                userAgent: null,
                tenantId: tenantId);

            _dbContext.TwoFactorCodes.Add(entity);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("✅ 2FA code generated and stored for {Email}", email);
            return code;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error generating 2FA code for {Email}", email);
            throw new InvalidOperationException("An error occurred while generating the verification code", ex);
        }
    }

    public async Task<TwoFactorResult> VerifyCodeAsync(string email, string code)
    {
        try
        {
            _logger.LogInformation("🔍 === 2FA CODE VERIFICATION START ===");
            _logger.LogInformation("📧 Email: {Email}", email);
            _logger.LogInformation("🔢 Code: {Code}", code);
            _logger.LogInformation("🏢 TenantId: {TenantId}", _tenantInfo?.Identifier ?? "null");

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("❌ Validation failed: Email or code is empty");
                return TwoFactorResult.Failure("Email and code are required");
            }

            // Tenant missing: fall back to strict DB validation without tenant filter
            if (_tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant info not available; performing strict DB validation without tenant filter");
                var twoFactorCodeFallback = await _dbContext.TwoFactorCodes
                    .Where(c => c.Email == email && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow)
                    .OrderByDescending(c => c.CreatedAt)
                    .FirstOrDefaultAsync();

                if (twoFactorCodeFallback == null)
                {
                    _logger.LogWarning("No valid 2FA code found for {Email} (no-tenant fallback)", email);
                    return TwoFactorResult.Failure("Invalid or expired verification code");
                }

                twoFactorCodeFallback.AttemptCount++;
                if (twoFactorCodeFallback.AttemptCount > _options.MaxVerificationAttempts)
                {
                    twoFactorCodeFallback.IsUsed = true;
                    await _dbContext.SaveChangesAsync();
                    return TwoFactorResult.Failure("Too many verification attempts. Please request a new code.");
                }

                var isValidFallback = string.Equals(twoFactorCodeFallback.Code, code, StringComparison.OrdinalIgnoreCase);
                if (!isValidFallback)
                {
                    await _dbContext.SaveChangesAsync();
                    var remainingAttempts = _options.MaxVerificationAttempts - twoFactorCodeFallback.AttemptCount;
                    return TwoFactorResult.Failure($"Invalid verification code. {remainingAttempts} attempts remaining.");
                }

                twoFactorCodeFallback.IsUsed = true;
                twoFactorCodeFallback.UsedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return TwoFactorResult.CreateSuccess();
            }

            // PRODUCTION MODE: Use database lookup
            _logger.LogInformation("🔍 Production mode: Looking up code in database");

            // Find the most recent active code for this email
            var twoFactorCode = await _dbContext.TwoFactorCodes
                .Where(c => c.Email == email &&
                           c.TenantId == _tenantInfo.Identifier &&
                           !c.IsUsed &&
                           c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (twoFactorCode == null)
            {
                _logger.LogWarning("No valid 2FA code found for {Email} in tenant {TenantId}", email, _tenantInfo.Identifier);
                return TwoFactorResult.Failure("Invalid or expired verification code");
            }

            // Increment attempt count
            twoFactorCode.AttemptCount++;

            // Check if too many attempts
            if (twoFactorCode.AttemptCount > _options.MaxVerificationAttempts)
            {
                twoFactorCode.IsUsed = true; // Invalidate the code
                await _dbContext.SaveChangesAsync();
                
                _logger.LogWarning("Too many verification attempts for 2FA code for {Email} in tenant {TenantId}", email, _tenantInfo.Identifier);
                return TwoFactorResult.Failure("Too many verification attempts. Please request a new code.");
            }

            // Verify the code
            var isValidCode = string.Equals(twoFactorCode.Code, code, StringComparison.OrdinalIgnoreCase);

            if (isValidCode)
            {
                // Mark code as used
                twoFactorCode.IsUsed = true;
                twoFactorCode.UsedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("2FA code verified successfully for {Email} in tenant {TenantId}", email, _tenantInfo.Identifier);
                return TwoFactorResult.CreateSuccess();
            }
            else
            {
                // Save the failed attempt
                await _dbContext.SaveChangesAsync();
                
                _logger.LogWarning("Invalid 2FA code provided for {Email} in tenant {TenantId}. Attempt {AttemptCount}/{MaxAttempts}", 
                    email, _tenantInfo.Identifier, twoFactorCode.AttemptCount, _options.MaxVerificationAttempts);
                
                var remainingAttempts = _options.MaxVerificationAttempts - twoFactorCode.AttemptCount;
                return TwoFactorResult.Failure($"Invalid verification code. {remainingAttempts} attempts remaining.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying 2FA code for {Email}", email);
            return TwoFactorResult.Failure("An error occurred while verifying the code");
        }
    }

    public async Task<bool> InvalidateCodeAsync(string email, string code)
    {
        try
        {
            var twoFactorCode = await _dbContext.TwoFactorCodes
                .FirstOrDefaultAsync(c => c.Email == email && 
                                         c.Code == code && 
                                         c.TenantId == _tenantInfo.Identifier &&
                                         !c.IsUsed);

            if (twoFactorCode != null)
            {
                twoFactorCode.IsUsed = true;
                twoFactorCode.UsedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation("2FA code invalidated for {Email} in tenant {TenantId}", email, _tenantInfo.Identifier);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating 2FA code for {Email}", email);
            return false;
        }
    }

    public async Task<bool> IsCodeValidAsync(string email, string code)
    {
        try
        {
            var twoFactorCode = await _dbContext.TwoFactorCodes
                .AnyAsync(c => c.Email == email && 
                              c.Code == code && 
                              c.TenantId == _tenantInfo.Identifier &&
                              !c.IsUsed && 
                              c.ExpiresAt > DateTime.UtcNow);

            return twoFactorCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if 2FA code is valid for {Email}", email);
            return false;
        }
    }



    public async Task<Dictionary<string, int>> GetCodeStatsAsync(DateTime fromDate)
    {
        try
        {
            var stats = await _dbContext.TwoFactorCodes
                .Where(c => c.CreatedAt >= fromDate && c.TenantId == _tenantInfo.Identifier)
                .GroupBy(c => 1)
                .Select(g => new
                {
                    TotalGenerated = g.Count(),
                    TotalUsed = g.Count(c => c.IsUsed),
                    TotalExpired = g.Count(c => c.ExpiresAt <= DateTime.UtcNow && !c.IsUsed),
                    AverageAttempts = g.Average(c => c.AttemptCount)
                })
                .FirstOrDefaultAsync();

            if (stats == null)
            {
                return new Dictionary<string, int>
                {
                    ["TotalGenerated"] = 0,
                    ["TotalUsed"] = 0,
                    ["TotalExpired"] = 0,
                    ["AverageAttempts"] = 0
                };
            }

            return new Dictionary<string, int>
            {
                ["TotalGenerated"] = stats.TotalGenerated,
                ["TotalUsed"] = stats.TotalUsed,
                ["TotalExpired"] = stats.TotalExpired,
                ["AverageAttempts"] = (int)Math.Round(stats.AverageAttempts)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting 2FA code statistics");
            return new Dictionary<string, int>();
        }
    }

    private async Task<bool> IsRateLimitedAsync(string email)
    {
        try
        {
            var now = DateTime.UtcNow;
            var timeWindow = now.AddMinutes(-_options.RateLimitWindowMinutes);

            var recentCodes = await _dbContext.TwoFactorCodes
                .CountAsync(c => c.Email == email && 
                               c.TenantId == _tenantInfo.Identifier &&
                               c.CreatedAt >= timeWindow);

            return recentCodes >= _options.MaxCodesPerWindow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for {Email}", email);
            return false; // Allow on error to avoid blocking legitimate users
        }
    }

    private async Task InvalidateExistingCodesAsync(string email)
    {
        try
        {
            var existingCodes = await _dbContext.TwoFactorCodes
                .Where(c => c.Email == email && 
                           c.TenantId == _tenantInfo.Identifier &&
                           !c.IsUsed && 
                           c.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var code in existingCodes)
            {
                code.IsUsed = true;
                code.UsedAt = DateTime.UtcNow;
            }

            if (existingCodes.Any())
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Invalidated {Count} existing 2FA codes for {Email}", existingCodes.Count, email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating existing codes for {Email}", email);
        }
    }

    // Missing interface methods
    public async Task<string> GenerateCodeAsync(string email, string? ipAddress = null)
    {
        // Use hardcoded tenant for testing
        return await GenerateCodeAsync(email, "thermotemp", ipAddress);
    }

    public async Task<bool> ValidateCodeAsync(string email, string code, bool markAsUsed = true)
    {
        var result = await VerifyCodeAsync(email, code, markAsUsed);
        return result.IsValid;
    }

    public async Task<TwoFactorValidationResult> VerifyCodeAsync(string email, string code, bool markAsUsed = true)
    {
        try
        {
            _logger.LogInformation("🔍 === 2FA CODE VERIFICATION START ===");
            _logger.LogInformation("📧 Email: {Email}", email);
            _logger.LogInformation("🔢 Code: {Code}", code);
            _logger.LogInformation("🏢 TenantId: {TenantId}", _tenantInfo?.Identifier ?? "null");

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("❌ Validation failed: Email or code is empty");
                return TwoFactorValidationResult.Failed("Email and code are required");
            }

            // Tenant missing: strict DB validation without tenant filter
            if (_tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant info not available; performing strict DB validation without tenant filter");
                var codeEntityFallback = await _dbContext.TwoFactorCodes
                    .FirstOrDefaultAsync(c => c.Email == email && c.Code == code);

                if (codeEntityFallback == null)
                {
                    _logger.LogWarning("No matching code found in database (no-tenant fallback)");
                    return TwoFactorValidationResult.Failed("Invalid verification code");
                }

                if (codeEntityFallback.IsUsed)
                {
                    _logger.LogWarning("Code has already been used");
                    return TwoFactorValidationResult.Used();
                }

                if (codeEntityFallback.ExpiresAt <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Code has expired");
                    return TwoFactorValidationResult.Expired();
                }

                if (codeEntityFallback.RemainingAttempts <= 0)
                {
                    _logger.LogWarning("Maximum attempts exceeded");
                    return TwoFactorValidationResult.AttemptsExceeded();
                }

                codeEntityFallback.IncrementAttempts();
                if (markAsUsed)
                {
                    codeEntityFallback.IsUsed = true;
                }

                await _dbContext.SaveChangesAsync();
                return TwoFactorValidationResult.Success();
            }

            // PRODUCTION MODE: Use database lookup
            _logger.LogInformation("🔍 Production mode: Looking up code in database");

            var codeEntity = await _dbContext.TwoFactorCodes
                .FirstOrDefaultAsync(c => c.Email == email && c.Code == code);

            if (codeEntity == null)
            {
                _logger.LogWarning("❌ No matching code found in database");
                return TwoFactorValidationResult.Failed("Invalid verification code");
            }

            if (codeEntity.IsUsed)
            {
                _logger.LogWarning("❌ Code has already been used");
                return TwoFactorValidationResult.Used();
            }

            if (codeEntity.ExpiresAt <= DateTime.UtcNow)
            {
                _logger.LogWarning("❌ Code has expired");
                return TwoFactorValidationResult.Expired();
            }

            if (codeEntity.RemainingAttempts <= 0)
            {
                _logger.LogWarning("❌ Maximum attempts exceeded");
                return TwoFactorValidationResult.AttemptsExceeded();
            }

            // Increment attempts
            codeEntity.IncrementAttempts();

            if (markAsUsed)
            {
                codeEntity.IsUsed = true;
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("✅ 2FA code verification successful");
            return TwoFactorValidationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error verifying code for {Email}", email);
            return TwoFactorValidationResult.Failed("Verification failed");
        }
    }

    public async Task<int> InvalidateCodesAsync(string email)
    {
        try
        {
            var codes = await _dbContext.TwoFactorCodes
                .Where(c => c.Email == email && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var code in codes)
            {
                code.IsUsed = true;
            }

            await _dbContext.SaveChangesAsync();
            return codes.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating codes for {Email}", email);
            return 0;
        }
    }

    public async Task<int?> GetRemainingAttemptsAsync(string email, string code)
    {
        try
        {
            var codeEntity = await _dbContext.TwoFactorCodes
                .FirstOrDefaultAsync(c => c.Email == email && c.Code == code && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow);

            return codeEntity?.RemainingAttempts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting remaining attempts for {Email}", email);
            return null;
        }
    }

    public async Task<bool> IsRateLimitExceededAsync(string email, int timeWindowMinutes = 15, int maxCodes = 3)
    {
        try
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-timeWindowMinutes);
            var recentCodes = await _dbContext.TwoFactorCodes
                .CountAsync(c => c.Email == email && c.CreatedAt > cutoffTime);

            return recentCodes >= maxCodes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for {Email}", email);
            return false;
        }
    }

    public async Task<DateTime?> GetCodeExpirationAsync(string email)
    {
        try
        {
            var latestCode = await _dbContext.TwoFactorCodes
                .Where(c => c.Email == email && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            return latestCode?.ExpiresAt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting code expiration for {Email}", email);
            return null;
        }
    }

    public async Task<int> CleanupExpiredCodesAsync()
    {
        try
        {
            var expiredCodes = await _dbContext.TwoFactorCodes
                .Where(c => c.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            _dbContext.TwoFactorCodes.RemoveRange(expiredCodes);
            await _dbContext.SaveChangesAsync();

            return expiredCodes.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired codes");
            return 0;
        }
    }

    public async Task<Dictionary<string, int>> GetTwoFactorStatsAsync(DateTime fromDate)
    {
        return await GetCodeStatsAsync(fromDate);
    }
}

/// <summary>
/// Two-factor authentication configuration options
/// </summary>
public class TwoFactorOptions
{
    public const string SectionName = "TwoFactorSettings";

    /// <summary>
    /// Use alphanumeric codes instead of numeric only
    /// </summary>
    public bool UseAlphanumericCodes { get; set; } = false;

    /// <summary>
    /// Maximum verification attempts per code
    /// </summary>
    public int MaxVerificationAttempts { get; set; } = 3;

    /// <summary>
    /// Rate limit window in minutes
    /// </summary>
    public int RateLimitWindowMinutes { get; set; } = 15;

    /// <summary>
    /// Maximum codes that can be generated per window
    /// </summary>
    public int MaxCodesPerWindow { get; set; } = 5;

    /// <summary>
    /// Default code expiration in minutes
    /// </summary>
    public int DefaultExpirationMinutes { get; set; } = 10;

    /// <summary>
    /// Cleanup interval for expired codes in minutes
    /// </summary>
    public int CleanupIntervalMinutes { get; set; } = 30;

    /// <summary>
    /// Enable detailed logging for 2FA operations
    /// </summary>
    public bool EnableDetailedLogging { get; set; } = true;
}

/// <summary>
/// Result of two-factor authentication operations
/// </summary>
public class TwoFactorResult
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? Code { get; private set; }

    private TwoFactorResult(bool success, string? errorMessage = null, string? code = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Code = code;
    }

    public static TwoFactorResult CreateSuccess(string? code = null)
    {
        return new TwoFactorResult(true, code: code);
    }

    public static TwoFactorResult Failure(string errorMessage)
    {
        return new TwoFactorResult(false, errorMessage);
    }
}


