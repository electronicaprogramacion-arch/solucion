using CalibrationSaaS.CustomerPortal.Data;
using CalibrationSaaS.CustomerPortal.Models.Authentication;
using CalibrationSaaS.CustomerPortal.Services.Email;
using CalibrationSaaS.CustomerPortal.Services.GrpcClients;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using LocalTenantInfo = CalibrationSaaS.CustomerPortal.Models.MultiTenancy.TenantInfo;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Customer authentication service implementation
/// </summary>
public class CustomerAuthenticationService : ICustomerAuthenticationService
{
    private readonly CustomerPortalDbContext _dbContext;
    private readonly ICustomerPortalGrpcService _customerGrpcService;
    private readonly ITwoFactorService _twoFactorService;
    private readonly IEmailService _emailService;
    private readonly ILogger<CustomerAuthenticationService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMultiTenantContextAccessor<LocalTenantInfo> _tenantAccessor;
    private readonly ISessionStorageService _sessionStorage;
    private readonly AuthenticationOptions _authOptions;

    public CustomerAuthenticationService(
        CustomerPortalDbContext dbContext,
        ICustomerPortalGrpcService customerGrpcService,
        ITwoFactorService twoFactorService,
        IEmailService emailService,
        ILogger<CustomerAuthenticationService> logger,
        IHttpContextAccessor httpContextAccessor,
        IMultiTenantContextAccessor<LocalTenantInfo> tenantAccessor,
        ISessionStorageService sessionStorage,
        IOptions<AuthenticationOptions> authOptions)
    {
        _dbContext = dbContext;
        _customerGrpcService = customerGrpcService;
        _twoFactorService = twoFactorService;
        _emailService = emailService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _tenantAccessor = tenantAccessor;
        _sessionStorage = sessionStorage;
        _authOptions = authOptions.Value;
    }

    public async Task<AuthenticationResult> InitiateLoginAsync(LoginRequest request)
    {
        try
        {
            // Debug: Log tenant context information
            var multiTenantContext = _tenantAccessor.MultiTenantContext;
            _logger.LogInformation($"MultiTenantContext is null: {multiTenantContext == null}");

            if (multiTenantContext != null)
            {
                _logger.LogInformation($"TenantInfo is null: {multiTenantContext.TenantInfo == null}");
                if (multiTenantContext.TenantInfo != null)
                {
                    _logger.LogInformation($"Tenant Identifier: {multiTenantContext.TenantInfo.Identifier}");
                    _logger.LogInformation($"Tenant Name: {multiTenantContext.TenantInfo.Name}");
                }
            }

            // Check if tenant info is available
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;

            // TEMPORARY FIX: If tenant context is null, create a hardcoded tenant for testing
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant context is null, using hardcoded tenant for testing");
                tenantInfo = new LocalTenantInfo
                {
                    Id = "thermotemp-tenant-001",
                    Identifier = "thermotemp",
                    Name = "ThermoTemp Development",
                    ConnectionString = "Server=20.1.195.112;Initial Catalog=CalibrationSaaS_ThermoTemp_CP;Persist Security Info=False;User ID=DevUser;Password=Gr@nP@ramo2025!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=240;",
                    CompanyName = "ThermoTemp",
                    IsActive = true,
                    TimeZone = "UTC",
                    Culture = "en-US",
                    ContactEmail = "dev@thermotemp.com",
                    MaxUsers = 100,
                    MaxStorageMB = 2000,
                    EnableAuditLogging = true,
                    EnableEmailNotifications = true
                };
                _logger.LogInformation($"✅ Using hardcoded tenant: {tenantInfo.Identifier} - {tenantInfo.Name}");
            }

            // Validate email format
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                await LogAuthenticationEventAsync(request.Email, "Invalid Email Format", false, "Invalid email format provided");
                return AuthenticationResult.Failure("Invalid email format");
            }

            // Check rate limiting
            if (await IsRateLimitedAsync(request.Email))
            {
                await LogAuthenticationEventAsync(request.Email, "Rate Limited", false, "Too many login attempts");
                return AuthenticationResult.Failure("Too many login attempts. Please try again later.");
            }

            // Validate customer contact
            var customerContact = await _customerGrpcService.GetCustomerContactByEmailAsync(request.Email, tenantInfo.Identifier!);
            if (customerContact == null || !customerContact.CanAuthenticate)
            {
                await LogAuthenticationEventAsync(request.Email, "Invalid Customer Contact", false, "Customer contact not found or disabled");
                return AuthenticationResult.Failure("Invalid email address or account disabled");
            }

            // Check if customer is active
            var isCustomerActive = await _customerGrpcService.IsCustomerActiveAsync(customerContact.CustomerId, tenantInfo.Identifier!);
            if (!isCustomerActive)
            {
                await LogAuthenticationEventAsync(request.Email, "Inactive Customer", false, "Customer account is inactive");
                return AuthenticationResult.Failure("Customer account is inactive");
            }

            // Generate 2FA code (display on screen instead of sending email)
            _logger.LogInformation("🎯 Generating 2FA code for {Email}", request.Email);

            try
            {
                var code = await _twoFactorService.GenerateCodeAsync(
                    request.Email,
                    tenantInfo.Identifier!,
                    GetClientIpAddress());

                _logger.LogInformation("✅ 2FA code generated successfully: {Code}", code);

                // For demo: Log code to console instead of sending email or displaying on screen
                _logger.LogInformation("🎬 DEMO MODE: 2FA Code for {Email}: {Code}", request.Email, code);
                _logger.LogInformation("📋 Copy this code for demo: {Code}", code);

                await LogAuthenticationEventAsync(request.Email, "2FA Code Generated", true, $"2FA code generated: {code}");

                // Create a success result that requires two-factor authentication (without code in message)
                return new AuthenticationResult
                {
                    Success = true,
                    Message = "Verification code has been generated. Check the console for the code.",
                    RequiresTwoFactor = true,
                    CodeExpiresAt = DateTime.UtcNow.AddMinutes(10)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 2FA code generation failed for {Email}", request.Email);
                await LogAuthenticationEventAsync(request.Email, "2FA Code Generation Failed", false, ex.Message);
                return AuthenticationResult.Failure("Unable to generate verification code. Please try again.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating login for {Email}", request.Email);
            await LogAuthenticationEventAsync(request.Email, "System Error", false, ex.Message);
            return AuthenticationResult.Failure("An error occurred. Please try again.");
        }
    }

    public async Task<AuthenticationResult> VerifyCodeAndLoginAsync(string email, string code)
    {
        try
        {
            // Check if tenant info is available
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;

            // TEMPORARY FIX: If tenant context is null, create a hardcoded tenant for testing
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant context is null during 2FA verification, using hardcoded tenant for testing");
                tenantInfo = new LocalTenantInfo
                {
                    Id = "thermotemp-tenant-001",
                    Identifier = "thermotemp",
                    Name = "ThermoTemp Development",
                    ConnectionString = "Server=20.1.195.112;Initial Catalog=CalibrationSaaS_ThermoTemp_CP;Persist Security Info=False;User ID=DevUser;Password=Gr@nP@ramo2025!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=240;",
                    CompanyName = "ThermoTemp",
                    IsActive = true,
                    TimeZone = "UTC",
                    Culture = "en-US",
                    ContactEmail = "dev@thermotemp.com",
                    MaxUsers = 100,
                    MaxStorageMB = 2000,
                    EnableAuditLogging = true,
                    EnableEmailNotifications = true
                };
                _logger.LogInformation($"✅ Using hardcoded tenant for 2FA verification: {tenantInfo.Identifier} - {tenantInfo.Name}");
            }

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                await LogAuthenticationEventAsync(email, "Invalid Input", false, "Email or code is empty");
                return AuthenticationResult.Failure("Email and verification code are required");
            }

            // Check rate limiting
            if (await IsRateLimitedAsync(email))
            {
                await LogAuthenticationEventAsync(email, "Rate Limited", false, "Too many verification attempts");
                return AuthenticationResult.Failure("Too many attempts. Please try again later.");
            }

            // Verify 2FA code
            _logger.LogInformation("🔍 === CALLING 2FA VERIFICATION ===");
            _logger.LogInformation("📧 Email: {Email}", email);
            _logger.LogInformation("🔢 Code: {Code}", code);

            var verificationResult = await _twoFactorService.VerifyCodeAsync(email, code, markAsUsed: true);

            _logger.LogInformation("🔍 === 2FA VERIFICATION RESULT ===");
            _logger.LogInformation("✅ IsValid: {IsValid}", verificationResult.IsValid);
            _logger.LogInformation("📝 ErrorMessage: {ErrorMessage}", verificationResult.ErrorMessage ?? "None");

            if (!verificationResult.IsValid)
            {
                _logger.LogWarning("❌ 2FA verification failed: {ErrorMessage}", verificationResult.ErrorMessage);
                await LogAuthenticationEventAsync(email, "Invalid 2FA Code", false, verificationResult.ErrorMessage ?? "Invalid verification code");
                return AuthenticationResult.Failure(verificationResult.ErrorMessage ?? "Invalid verification code");
            }

            _logger.LogInformation("✅ 2FA verification successful!");

            // Get customer contact information
            var customerContact = await _customerGrpcService.GetCustomerContactByEmailAsync(email, tenantInfo.Identifier!);
            if (customerContact == null || !customerContact.CanAuthenticate)
            {
                await LogAuthenticationEventAsync(email, "Invalid Customer Contact", false, "Customer contact not found during verification");
                return AuthenticationResult.Failure("Authentication failed");
            }

            // Create session
            var session = await CreateSessionAsync(customerContact);
            if (session == null)
            {
                await LogAuthenticationEventAsync(email, "Session Creation Failed", false, "Failed to create user session");
                return AuthenticationResult.Failure("Unable to create session. Please try again.");
            }

            // Update last login
            await _customerGrpcService.UpdateLastLoginAsync(
                customerContact.CustomerId,
                customerContact.ContactId,
                tenantInfo.Identifier!,
                DateTime.UtcNow);

            await LogAuthenticationEventAsync(email, "Login Successful", true, "User logged in successfully");
            return AuthenticationResult.CreateSuccess("Login successful", session);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying code and logging in for {Email}", email);
            await LogAuthenticationEventAsync(email, "System Error", false, ex.Message);
            return AuthenticationResult.Failure("An error occurred. Please try again.");
        }
    }

    public async Task<bool> LogoutAsync(string sessionId)
    {
        try
        {
            var session = await _dbContext.CustomerSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive);

            if (session != null)
            {
                session.IsActive = false;
                session.EndedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                await LogAuthenticationEventAsync(session.Email, "Logout", true, "User logged out");
                _logger.LogInformation("Session {SessionId} ended for {Email}", sessionId, session.Email);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<CustomerSession?> GetCurrentSessionAsync()
    {
        try
        {
            // First try to get session from our session storage service
            var session = await _sessionStorage.GetCurrentSessionAsync();

            if (session != null)
            {
                // Verify session is still valid in database
                var dbSession = await _dbContext.CustomerSessions
                    .FirstOrDefaultAsync(s => s.SessionId == session.SessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

                if (dbSession != null)
                {
                    // Extend session if needed
                    if (dbSession.ExpiresAt <= DateTime.UtcNow.AddMinutes(5))
                    {
                        await ExtendSessionAsync(dbSession.SessionId);
                    }

                    // Update last activity
                    dbSession.LastActivityAt = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();

                    // Update session storage with latest data
                    await _sessionStorage.StoreSessionAsync(dbSession);

                    return dbSession;
                }
                else
                {
                    // Session not found in database, clear from storage
                    await _sessionStorage.ClearSessionAsync();
                }
            }

            // Fallback: try to find session by IP/User-Agent combination
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var clientIp = GetClientIpAddress();
                var userAgent = GetUserAgent();

                var fallbackSession = await _dbContext.CustomerSessions
                    .Where(s => s.IsActive && s.ExpiresAt > DateTime.UtcNow &&
                               s.IpAddress == clientIp && s.UserAgent == userAgent)
                    .OrderByDescending(s => s.LastActivityAt)
                    .FirstOrDefaultAsync();

                if (fallbackSession != null)
                {
                    // Store in session storage for future requests
                    await _sessionStorage.StoreSessionAsync(fallbackSession);
                    return fallbackSession;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current session");
            return null;
        }
    }

    public async Task<bool> ExtendSessionAsync(string sessionId)
    {
        try
        {
            var session = await _dbContext.CustomerSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive);

            if (session != null)
            {
                session.ExpiresAt = DateTime.UtcNow.AddMinutes(_authOptions.SessionTimeoutMinutes);
                session.LastActivityAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extending session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<bool> IsValidSessionAsync(string sessionId)
    {
        try
        {
            var session = await _dbContext.CustomerSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

            return session != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating session {SessionId}", sessionId);
            return false;
        }
    }

    public async Task<List<AuthenticationAuditLog>> GetAuthenticationLogsAsync(string email, int pageNumber = 1, int pageSize = 50)
    {
        try
        {
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant information not available for authentication logs");
                return new List<AuthenticationAuditLog>();
            }

            return await _dbContext.AuthenticationAuditLogs
                .Where(log => log.Email == email && log.TenantId == tenantInfo.Identifier)
                .OrderByDescending(log => log.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting authentication logs for {Email}", email);
            return new List<AuthenticationAuditLog>();
        }
    }

    public async Task<int> CleanupExpiredSessionsAsync()
    {
        try
        {
            var expiredSessions = await _dbContext.CustomerSessions
                .Where(s => s.IsActive && s.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var session in expiredSessions)
            {
                session.IsActive = false;
                session.EndedAt = DateTime.UtcNow;
            }

            if (expiredSessions.Any())
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
            }

            return expiredSessions.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired sessions");
            return 0;
        }
    }

    private async Task<CustomerSession?> CreateSessionAsync(CustomerContactDto customerContact)
    {
        try
        {
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant context is null during session creation, using hardcoded tenant for testing");
                tenantInfo = new LocalTenantInfo
                {
                    Identifier = "thermotemp",
                    Name = "ThermoTemp Development"
                };
                _logger.LogInformation("✅ Using hardcoded tenant for session creation: {TenantId} - {TenantName}", tenantInfo.Identifier, tenantInfo.Name);
            }

            var sessionId = GenerateSessionId();
            var session = new CustomerSession
            {
                SessionId = sessionId,
                Email = customerContact.Email,
                ContactId = customerContact.ContactId,
                ContactName = customerContact.FullName,
                CustomerId = customerContact.CustomerId,
                CustomerName = customerContact.CustomerName,
                TenantId = tenantInfo.Identifier,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_authOptions.SessionTimeoutMinutes),
                LastActivityAt = DateTime.UtcNow,
                IsActive = true,
                IpAddress = GetClientIpAddress(),
                UserAgent = GetUserAgent()
            };

            _dbContext.CustomerSessions.Add(session);
            await _dbContext.SaveChangesAsync();

            // Store session in our session storage service instead of HTTP sessions
            await _sessionStorage.StoreSessionAsync(session);

            _logger.LogInformation("✅ Session created successfully for customer contact {ContactId} with session ID {SessionId}",
                customerContact.ContactId, sessionId);

            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session for customer contact {ContactId}", customerContact.ContactId);
            return null;
        }
    }

    private async Task<bool> IsRateLimitedAsync(string email)
    {
        try
        {
            // If tenant info is not available, allow the request (fail open for rate limiting)
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant information not available for rate limiting check");
                return false;
            }

            var now = DateTime.UtcNow;
            var oneHourAgo = now.AddHours(-1);

            var recentAttempts = await _dbContext.AuthenticationAuditLogs
                .CountAsync(log => log.Email == email &&
                           log.TenantId == tenantInfo.Identifier &&
                           log.Timestamp >= oneHourAgo);

            return recentAttempts >= _authOptions.MaxAttemptsPerHour;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for {Email}", email);
            return false; // Allow on error to avoid blocking legitimate users
        }
    }

    private async Task LogAuthenticationEventAsync(string email, string eventType, bool success, string details)
    {
        try
        {
            // If tenant info is not available, skip logging
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant information not available for audit logging");
                return;
            }

            var auditLog = new AuthenticationAuditLog
            {
                Email = email,
                TenantId = tenantInfo.Identifier,
                EventType = eventType,
                Success = success,
                Details = details,
                IpAddress = GetClientIpAddress(),
                UserAgent = GetUserAgent(),
                Timestamp = DateTime.UtcNow
            };

            _dbContext.AuthenticationAuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging authentication event for {Email}", email);
        }
    }

    private string GenerateSessionId()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    private string GetClientIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return "Unknown";

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = ipAddress.Split(',')[0].Trim();
            }
        }

        return ipAddress ?? "Unknown";
    }

    private string GetUserAgent()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Missing interface methods
    public async Task<AuthenticationResult> VerifyTwoFactorCodeAsync(TwoFactorRequest request)
    {
        return await VerifyCodeAndLoginAsync(request.Email, request.Code);
    }

    public async Task<bool> IsValidCustomerContactAsync(string email, string tenantId)
    {
        var contact = await GetCustomerContactAsync(email, tenantId);
        return contact?.CanAuthenticate == true;
    }

    public async Task<CustomerContactInfo?> GetCustomerContactAsync(string email, string tenantId)
    {
        try
        {
            var contact = await _customerGrpcService.GetCustomerContactByEmailAsync(email, tenantId);
            if (contact == null) return null;

            return new CustomerContactInfo
            {
                ContactId = contact.ContactId,
                Email = contact.Email,
                Name = contact.Name,
                LastName = contact.LastName,
                CustomerId = contact.CustomerId,
                CustomerName = contact.CustomerName,
                TenantId = contact.TenantId,
                IsEnabled = contact.IsEnabled,
                IsDelete = contact.IsDelete
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer contact for {Email}", email);
            return null;
        }
    }

    public async Task<CustomerSession?> ValidateSessionAsync(string sessionId)
    {
        if (await IsValidSessionAsync(sessionId))
        {
            return await GetCurrentSessionAsync();
        }
        return null;
    }

    public async Task<bool> ExtendSessionAsync(string sessionId, int extensionMinutes = 30)
    {
        return await ExtendSessionAsync(sessionId);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var session = await GetCurrentSessionAsync();
        return session != null;
    }

    public async Task<List<AuthenticationAuditLog>> GetAuditLogsAsync(string email, DateTime fromDate, DateTime toDate)
    {
        return await GetAuthenticationLogsAsync(email);
    }

    public async Task LogAuthenticationEventAsync(
        string email,
        string action,
        bool success,
        string? details = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? errorMessage = null)
    {
        if (!_authOptions.EnableAuditLogging) return;

        try
        {
            var tenantInfo = _tenantAccessor.MultiTenantContext?.TenantInfo;
            if (tenantInfo?.Identifier == null)
            {
                _logger.LogWarning("Tenant information not available for error logging");
                return;
            }

            var auditLog = new AuthenticationAuditLog
            {
                Email = email,
                Action = action,
                Success = success,
                Details = details,
                IpAddress = ipAddress ?? GetClientIpAddress(),
                UserAgent = userAgent ?? GetUserAgent(),
                SessionId = sessionId,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.UtcNow,
                TenantId = tenantInfo.Identifier
            };

            _dbContext.AuthenticationAuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging authentication event for {Email}", email);
        }
    }
}

/// <summary>
/// Authentication configuration options
/// </summary>
public class AuthenticationOptions
{
    public const string SectionName = "AuthenticationSettings";

    /// <summary>
    /// Session timeout in minutes
    /// </summary>
    public int SessionTimeoutMinutes { get; set; } = 30;

    /// <summary>
    /// Two-factor code length
    /// </summary>
    public int TwoFactorCodeLength { get; set; } = 6;

    /// <summary>
    /// Two-factor code expiration in minutes
    /// </summary>
    public int TwoFactorExpirationMinutes { get; set; } = 10;

    /// <summary>
    /// Maximum login attempts per hour
    /// </summary>
    public int MaxAttemptsPerHour { get; set; } = 10;

    /// <summary>
    /// Maximum login attempts per day
    /// </summary>
    public int MaxAttemptsPerDay { get; set; } = 50;

    /// <summary>
    /// Enable audit logging
    /// </summary>
    public bool EnableAuditLogging { get; set; } = true;

    /// <summary>
    /// Cleanup expired sessions interval in minutes
    /// </summary>
    public int CleanupIntervalMinutes { get; set; } = 60;
}
