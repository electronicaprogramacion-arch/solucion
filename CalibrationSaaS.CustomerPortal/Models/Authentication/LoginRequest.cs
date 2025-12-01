using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Authentication;

/// <summary>
/// Request model for customer login
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(255, ErrorMessage = "Email address cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Tenant identifier (can be resolved from subdomain or route)
    /// </summary>
    public string? TenantId { get; set; }
    
    /// <summary>
    /// Client IP address for audit logging
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent for audit logging
    /// </summary>
    public string? UserAgent { get; set; }
}

/// <summary>
/// Response model for login request
/// </summary>
public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool RequiresTwoFactor { get; set; }
    public string? SessionId { get; set; }
    public DateTime? CodeExpiresAt { get; set; }
    public int? RemainingAttempts { get; set; }
}

/// <summary>
/// Request model for two-factor authentication verification
/// </summary>
public class TwoFactorRequest
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Verification code is required")]
    [StringLength(10, MinimumLength = 6, ErrorMessage = "Verification code must be 6 digits")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must be 6 digits")]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// Session ID from login response
    /// </summary>
    public string? SessionId { get; set; }
    
    /// <summary>
    /// Client IP address for audit logging
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent for audit logging
    /// </summary>
    public string? UserAgent { get; set; }
}

/// <summary>
/// Response model for two-factor authentication verification
/// </summary>
public class TwoFactorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? AuthenticationToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public CustomerSession? CustomerSession { get; set; }
    public int? RemainingAttempts { get; set; }
}

/// <summary>
/// Authentication result for internal use
/// </summary>
public class AuthenticationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public CustomerSession? CustomerSession { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public DateTime? CodeExpiresAt { get; set; }
    public int? RemainingAttempts { get; set; }

    /// <summary>
    /// Create a successful authentication result
    /// </summary>
    public static AuthenticationResult CreateSuccess(string message = "Authentication successful", CustomerSession? session = null)
    {
        return new AuthenticationResult
        {
            Success = true,
            Message = message,
            CustomerSession = session
        };
    }

    /// <summary>
    /// Create a failed authentication result
    /// </summary>
    public static AuthenticationResult Failure(string message, string? errorCode = null)
    {
        return new AuthenticationResult
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
}
