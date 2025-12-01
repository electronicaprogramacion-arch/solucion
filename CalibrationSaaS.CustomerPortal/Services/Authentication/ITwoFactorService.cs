namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Service interface for two-factor authentication operations
/// </summary>
public interface ITwoFactorService
{
    /// <summary>
    /// Generate a new 2FA code for the specified email
    /// </summary>
    /// <param name="email">Email address to generate code for</param>
    /// <param name="ipAddress">Client IP address for audit</param>
    /// <returns>Generated 2FA code</returns>
    Task<string> GenerateCodeAsync(string email, string? ipAddress = null);

    /// <summary>
    /// Generate a new 2FA code for the specified email with tenant context
    /// </summary>
    /// <param name="email">Email address to generate code for</param>
    /// <param name="tenantId">Tenant ID for multi-tenancy</param>
    /// <param name="ipAddress">Client IP address for audit</param>
    /// <returns>Generated 2FA code</returns>
    Task<string> GenerateCodeAsync(string email, string tenantId, string? ipAddress = null);

    /// <summary>
    /// Validate a 2FA code for the specified email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="code">Code to validate</param>
    /// <param name="markAsUsed">Whether to mark the code as used if valid</param>
    /// <returns>True if code is valid</returns>
    Task<bool> ValidateCodeAsync(string email, string code, bool markAsUsed = true);

    /// <summary>
    /// Verify a 2FA code for the specified email with detailed result
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="code">Code to verify</param>
    /// <param name="markAsUsed">Whether to mark the code as used if valid</param>
    /// <returns>Detailed verification result</returns>
    Task<TwoFactorValidationResult> VerifyCodeAsync(string email, string code, bool markAsUsed = true);

    /// <summary>
    /// Invalidate all codes for the specified email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>Number of codes invalidated</returns>
    Task<int> InvalidateCodesAsync(string email);

    /// <summary>
    /// Get remaining attempts for a code
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="code">Code to check</param>
    /// <returns>Number of remaining attempts, or null if code not found</returns>
    Task<int?> GetRemainingAttemptsAsync(string email, string code);

    /// <summary>
    /// Check if email has reached rate limit for code generation
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="timeWindowMinutes">Time window to check (default: 15 minutes)</param>
    /// <param name="maxCodes">Maximum codes allowed in time window (default: 3)</param>
    /// <returns>True if rate limit exceeded</returns>
    Task<bool> IsRateLimitExceededAsync(string email, int timeWindowMinutes = 15, int maxCodes = 3);

    /// <summary>
    /// Get the expiration time for the latest valid code for an email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>Expiration time or null if no valid code exists</returns>
    Task<DateTime?> GetCodeExpirationAsync(string email);

    /// <summary>
    /// Clean up expired codes from the database
    /// </summary>
    /// <returns>Number of codes cleaned up</returns>
    Task<int> CleanupExpiredCodesAsync();

    /// <summary>
    /// Get 2FA statistics for monitoring
    /// </summary>
    /// <param name="fromDate">Start date for statistics</param>
    /// <returns>Dictionary with statistics</returns>
    Task<Dictionary<string, int>> GetTwoFactorStatsAsync(DateTime fromDate);
}



/// <summary>
/// Result of 2FA code validation
/// </summary>
public class TwoFactorValidationResult
{
    public bool IsValid { get; set; }
    public bool IsExpired { get; set; }
    public bool IsUsed { get; set; }
    public bool MaxAttemptsExceeded { get; set; }
    public int RemainingAttempts { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Create a successful validation result
    /// </summary>
    public static TwoFactorValidationResult Success()
    {
        return new TwoFactorValidationResult { IsValid = true };
    }

    /// <summary>
    /// Create a failed validation result
    /// </summary>
    public static TwoFactorValidationResult Failed(string errorMessage, int remainingAttempts = 0)
    {
        return new TwoFactorValidationResult
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            RemainingAttempts = remainingAttempts
        };
    }

    /// <summary>
    /// Create an expired validation result
    /// </summary>
    public static TwoFactorValidationResult Expired()
    {
        return new TwoFactorValidationResult
        {
            IsValid = false,
            IsExpired = true,
            ErrorMessage = "Verification code has expired"
        };
    }

    /// <summary>
    /// Create a used validation result
    /// </summary>
    public static TwoFactorValidationResult Used()
    {
        return new TwoFactorValidationResult
        {
            IsValid = false,
            IsUsed = true,
            ErrorMessage = "Verification code has already been used"
        };
    }

    /// <summary>
    /// Create a max attempts exceeded result
    /// </summary>
    public static TwoFactorValidationResult AttemptsExceeded()
    {
        return new TwoFactorValidationResult
        {
            IsValid = false,
            MaxAttemptsExceeded = true,
            ErrorMessage = "Maximum verification attempts exceeded"
        };
    }
}
