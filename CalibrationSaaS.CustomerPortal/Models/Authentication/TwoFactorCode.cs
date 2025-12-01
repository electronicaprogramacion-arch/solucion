using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Authentication;

/// <summary>
/// Two-factor authentication code entity
/// </summary>
public class TwoFactorCode
{
    /// <summary>
    /// Unique identifier for the 2FA code
    /// </summary>
    [Key]
    [MaxLength(50)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Email address the code was sent to
    /// </summary>
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The verification code
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// When the code was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the code expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Whether the code has been used
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// When the code was used (if applicable)
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// Number of validation attempts made
    /// </summary>
    public int AttemptCount { get; set; } = 0;

    /// <summary>
    /// Maximum number of attempts allowed
    /// </summary>
    public int MaxAttempts { get; set; } = 3;

    /// <summary>
    /// IP address from which the code was requested
    /// </summary>
    [MaxLength(45)] // IPv6 support
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent from which the code was requested
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy
    /// </summary>
    [MaxLength(50)]
    public string? TenantId { get; set; }

    /// <summary>
    /// Check if the code is valid (not expired, not used, attempts not exceeded)
    /// </summary>
    public bool IsValid => !IsUsed && 
                          ExpiresAt > DateTime.UtcNow && 
                          AttemptCount < MaxAttempts;

    /// <summary>
    /// Check if the code is expired
    /// </summary>
    public bool IsExpired => ExpiresAt <= DateTime.UtcNow;

    /// <summary>
    /// Check if maximum attempts have been exceeded
    /// </summary>
    public bool MaxAttemptsExceeded => AttemptCount >= MaxAttempts;

    /// <summary>
    /// Get remaining attempts
    /// </summary>
    public int RemainingAttempts => Math.Max(0, MaxAttempts - AttemptCount);

    /// <summary>
    /// Mark the code as used
    /// </summary>
    public void MarkAsUsed()
    {
        IsUsed = true;
        UsedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Increment attempt count
    /// </summary>
    public void IncrementAttempts()
    {
        AttemptCount++;
    }

    /// <summary>
    /// Validate the provided code against this entity
    /// </summary>
    /// <param name="providedCode">Code to validate</param>
    /// <returns>True if codes match</returns>
    public bool ValidateCode(string providedCode)
    {
        return string.Equals(Code, providedCode, StringComparison.Ordinal);
    }

    /// <summary>
    /// Get time remaining until expiration
    /// </summary>
    public TimeSpan TimeUntilExpiration => ExpiresAt > DateTime.UtcNow 
        ? ExpiresAt - DateTime.UtcNow 
        : TimeSpan.Zero;

    /// <summary>
    /// Get minutes remaining until expiration
    /// </summary>
    public int MinutesUntilExpiration => (int)Math.Ceiling(TimeUntilExpiration.TotalMinutes);

    /// <summary>
    /// Create a new 2FA code
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="code">Generated code</param>
    /// <param name="expirationMinutes">Expiration time in minutes</param>
    /// <param name="maxAttempts">Maximum attempts allowed</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <returns>New TwoFactorCode instance</returns>
    public static TwoFactorCode Create(
        string email,
        string code,
        int expirationMinutes = 10,
        int maxAttempts = 3,
        string? ipAddress = null,
        string? userAgent = null,
        string? tenantId = null)
    {
        return new TwoFactorCode
        {
            Email = email,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            MaxAttempts = maxAttempts,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TenantId = tenantId
        };
    }

    /// <summary>
    /// Generate a random numeric code
    /// </summary>
    /// <param name="length">Length of the code (default: 6)</param>
    /// <returns>Random numeric code</returns>
    public static string GenerateCode(int length = 6)
    {
        var random = new Random();
        var code = string.Empty;
        
        for (int i = 0; i < length; i++)
        {
            code += random.Next(0, 10).ToString();
        }
        
        return code;
    }

    /// <summary>
    /// Generate a random alphanumeric code
    /// </summary>
    /// <param name="length">Length of the code</param>
    /// <param name="allowedCharacters">Characters to use for generation</param>
    /// <returns>Random alphanumeric code</returns>
    public static string GenerateAlphanumericCode(int length = 6, string allowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
        var random = new Random();
        var code = string.Empty;
        
        for (int i = 0; i < length; i++)
        {
            code += allowedCharacters[random.Next(allowedCharacters.Length)];
        }
        
        return code;
    }
}
