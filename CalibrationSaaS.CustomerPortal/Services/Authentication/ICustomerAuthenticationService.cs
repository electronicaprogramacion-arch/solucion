using CalibrationSaaS.CustomerPortal.Models.Authentication;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Service interface for customer authentication operations
/// </summary>
public interface ICustomerAuthenticationService
{
    /// <summary>
    /// Initiate login process by validating email and sending 2FA code
    /// </summary>
    /// <param name="request">Login request with email and metadata</param>
    /// <returns>Authentication result indicating next steps</returns>
    Task<AuthenticationResult> InitiateLoginAsync(LoginRequest request);

    /// <summary>
    /// Verify 2FA code and complete authentication
    /// </summary>
    /// <param name="request">2FA verification request</param>
    /// <returns>Authentication result with session information</returns>
    Task<AuthenticationResult> VerifyTwoFactorCodeAsync(TwoFactorRequest request);

    /// <summary>
    /// Check if an email belongs to a valid customer contact
    /// </summary>
    /// <param name="email">Email address to validate</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>True if email is valid customer contact</returns>
    Task<bool> IsValidCustomerContactAsync(string email, string tenantId);

    /// <summary>
    /// Get customer contact information by email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>Customer contact information or null if not found</returns>
    Task<CustomerContactInfo?> GetCustomerContactAsync(string email, string tenantId);

    /// <summary>
    /// Validate an existing session
    /// </summary>
    /// <param name="sessionId">Session identifier</param>
    /// <returns>Customer session if valid, null otherwise</returns>
    Task<CustomerSession?> ValidateSessionAsync(string sessionId);

    /// <summary>
    /// Extend an existing session
    /// </summary>
    /// <param name="sessionId">Session identifier</param>
    /// <param name="extensionMinutes">Minutes to extend session</param>
    /// <returns>True if session was extended successfully</returns>
    Task<bool> ExtendSessionAsync(string sessionId, int extensionMinutes = 30);

    /// <summary>
    /// Logout and invalidate session
    /// </summary>
    /// <param name="sessionId">Session identifier</param>
    /// <returns>True if logout was successful</returns>
    Task<bool> LogoutAsync(string sessionId);

    /// <summary>
    /// Get current session from HTTP context
    /// </summary>
    /// <returns>Current customer session or null</returns>
    Task<CustomerSession?> GetCurrentSessionAsync();

    /// <summary>
    /// Check if user is currently authenticated
    /// </summary>
    /// <returns>True if user has valid session</returns>
    Task<bool> IsAuthenticatedAsync();

    /// <summary>
    /// Get authentication audit logs for a customer
    /// </summary>
    /// <param name="email">Customer email</param>
    /// <param name="fromDate">Start date for logs</param>
    /// <param name="toDate">End date for logs</param>
    /// <returns>List of audit log entries</returns>
    Task<List<AuthenticationAuditLog>> GetAuditLogsAsync(string email, DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Log authentication event for audit purposes
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="action">Action performed</param>
    /// <param name="success">Whether action was successful</param>
    /// <param name="details">Additional details</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="userAgent">Client user agent</param>
    /// <param name="sessionId">Session ID if applicable</param>
    /// <param name="errorMessage">Error message if failed</param>
    Task LogAuthenticationEventAsync(
        string email,
        string action,
        bool success,
        string? details = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? sessionId = null,
        string? errorMessage = null);

    /// <summary>
    /// Clean up expired sessions from the database
    /// </summary>
    /// <returns>Number of sessions cleaned up</returns>
    Task<int> CleanupExpiredSessionsAsync();
}

/// <summary>
/// Customer contact information for authentication
/// </summary>
public class CustomerContactInfo
{
    public int ContactId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public bool IsDelete { get; set; }

    /// <summary>
    /// Full name of the contact
    /// </summary>
    public string FullName => $"{Name} {LastName}".Trim();

    /// <summary>
    /// Check if contact is active and can authenticate
    /// </summary>
    public bool CanAuthenticate => IsEnabled && !IsDelete;
}
