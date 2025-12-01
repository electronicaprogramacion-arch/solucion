using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Authentication;

/// <summary>
/// Authentication audit log entity for tracking authentication events
/// </summary>
public class AuthenticationAuditLog
{
    /// <summary>
    /// Unique identifier for the audit log entry
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Email address of the user
    /// </summary>
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Action that was performed
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Type of event for categorization
    /// </summary>
    [MaxLength(50)]
    public string? EventType { get; set; }

    /// <summary>
    /// Whether the action was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Timestamp when the action occurred
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP address from which the action was performed
    /// </summary>
    [MaxLength(45)] // IPv6 support
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Tenant ID for multi-tenancy
    /// </summary>
    [MaxLength(50)]
    public string? TenantId { get; set; }

    /// <summary>
    /// Additional details about the action
    /// </summary>
    [MaxLength(1000)]
    public string? Details { get; set; }

    /// <summary>
    /// Error message if the action failed
    /// </summary>
    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Session ID if applicable
    /// </summary>
    [MaxLength(50)]
    public string? SessionId { get; set; }

    /// <summary>
    /// Customer ID if known
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Contact ID if known
    /// </summary>
    public int? ContactId { get; set; }

    /// <summary>
    /// Duration of the action in milliseconds
    /// </summary>
    public long? DurationMs { get; set; }

    /// <summary>
    /// Risk score for the action (0-100, higher = more risky)
    /// </summary>
    public int RiskScore { get; set; } = 0;

    /// <summary>
    /// Additional metadata as JSON
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Create a new audit log entry
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="action">Action performed</param>
    /// <param name="success">Whether action was successful</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="details">Additional details</param>
    /// <param name="errorMessage">Error message if failed</param>
    /// <param name="sessionId">Session ID</param>
    /// <param name="customerId">Customer ID</param>
    /// <param name="contactId">Contact ID</param>
    /// <param name="durationMs">Action duration</param>
    /// <param name="riskScore">Risk score</param>
    /// <returns>New AuthenticationAuditLog instance</returns>
    public static AuthenticationAuditLog Create(
        string email,
        string action,
        bool success,
        string? ipAddress = null,
        string? userAgent = null,
        string? tenantId = null,
        string? details = null,
        string? errorMessage = null,
        string? sessionId = null,
        int? customerId = null,
        int? contactId = null,
        long? durationMs = null,
        int riskScore = 0)
    {
        return new AuthenticationAuditLog
        {
            Email = email,
            Action = action,
            Success = success,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            TenantId = tenantId,
            Details = details,
            ErrorMessage = errorMessage,
            SessionId = sessionId,
            CustomerId = customerId,
            ContactId = contactId,
            DurationMs = durationMs,
            RiskScore = riskScore
        };
    }

    /// <summary>
    /// Set metadata as JSON
    /// </summary>
    /// <param name="metadata">Metadata object to serialize</param>
    public void SetMetadata(object metadata)
    {
        if (metadata != null)
        {
            Metadata = System.Text.Json.JsonSerializer.Serialize(metadata);
        }
    }

    /// <summary>
    /// Get metadata as typed object
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <returns>Deserialized metadata or default</returns>
    public T? GetMetadata<T>() where T : class
    {
        if (string.IsNullOrEmpty(Metadata))
            return null;

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(Metadata);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Calculate risk score based on various factors
    /// </summary>
    /// <param name="isNewIp">Whether this is a new IP for the user</param>
    /// <param name="isNewUserAgent">Whether this is a new user agent</param>
    /// <param name="failedAttempts">Number of recent failed attempts</param>
    /// <param name="isOffHours">Whether login is during off hours</param>
    /// <param name="isUnusualLocation">Whether location is unusual for user</param>
    /// <returns>Risk score (0-100)</returns>
    public static int CalculateRiskScore(
        bool isNewIp = false,
        bool isNewUserAgent = false,
        int failedAttempts = 0,
        bool isOffHours = false,
        bool isUnusualLocation = false)
    {
        int score = 0;

        if (isNewIp) score += 20;
        if (isNewUserAgent) score += 15;
        if (failedAttempts > 0) score += Math.Min(failedAttempts * 10, 30);
        if (isOffHours) score += 10;
        if (isUnusualLocation) score += 25;

        return Math.Min(score, 100);
    }

    /// <summary>
    /// Get risk level description
    /// </summary>
    public string RiskLevel => RiskScore switch
    {
        <= 20 => "Low",
        <= 50 => "Medium",
        <= 80 => "High",
        _ => "Critical"
    };

    /// <summary>
    /// Get action display name
    /// </summary>
    public string ActionDisplay => Action switch
    {
        "LOGIN_INITIATED" => "Login Initiated",
        "LOGIN_SUCCESS" => "Login Successful",
        "LOGIN_FAILED" => "Login Failed",
        "2FA_CODE_SENT" => "2FA Code Sent",
        "2FA_CODE_VERIFIED" => "2FA Code Verified",
        "2FA_CODE_FAILED" => "2FA Code Failed",
        "SESSION_CREATED" => "Session Created",
        "SESSION_EXTENDED" => "Session Extended",
        "SESSION_EXPIRED" => "Session Expired",
        "LOGOUT" => "Logout",
        "ACCOUNT_LOCKED" => "Account Locked",
        "PASSWORD_RESET_REQUESTED" => "Password Reset Requested",
        "PASSWORD_RESET_COMPLETED" => "Password Reset Completed",
        _ => Action
    };

    /// <summary>
    /// Get CSS class for status display
    /// </summary>
    public string StatusCssClass => Success switch
    {
        true => "badge bg-success",
        false => "badge bg-danger"
    };

    /// <summary>
    /// Get CSS class for risk level display
    /// </summary>
    public string RiskCssClass => RiskLevel switch
    {
        "Low" => "badge bg-success",
        "Medium" => "badge bg-warning",
        "High" => "badge bg-danger",
        "Critical" => "badge bg-dark",
        _ => "badge bg-secondary"
    };

    /// <summary>
    /// Get formatted timestamp for display
    /// </summary>
    public string TimestampDisplay => Timestamp.ToString("MMM dd, yyyy HH:mm:ss UTC");

    /// <summary>
    /// Get short IP address for display
    /// </summary>
    public string IpAddressDisplay => string.IsNullOrEmpty(IpAddress) ? "Unknown" : 
        IpAddress.Length > 15 ? IpAddress[..12] + "..." : IpAddress;
}
