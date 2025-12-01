namespace CalibrationSaaS.CustomerPortal.Services.Email;

/// <summary>
/// Service interface for email operations
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send a two-factor authentication code via email
    /// </summary>
    /// <param name="email">Recipient email address</param>
    /// <param name="code">2FA code to send</param>
    /// <param name="customerName">Customer name for personalization</param>
    /// <param name="expirationMinutes">Code expiration time in minutes</param>
    /// <returns>True if email was sent successfully</returns>
    Task<bool> SendTwoFactorCodeAsync(string email, string code, string customerName, int expirationMinutes);

    /// <summary>
    /// Send a welcome email to new customer portal users
    /// </summary>
    /// <param name="email">Recipient email address</param>
    /// <param name="customerName">Customer name</param>
    /// <param name="contactName">Contact name</param>
    /// <returns>True if email was sent successfully</returns>
    Task<bool> SendWelcomeEmailAsync(string email, string customerName, string contactName);

    /// <summary>
    /// Send account lockout notification
    /// </summary>
    /// <param name="email">Recipient email address</param>
    /// <param name="customerName">Customer name</param>
    /// <param name="lockoutDurationMinutes">Lockout duration in minutes</param>
    /// <returns>True if email was sent successfully</returns>
    Task<bool> SendAccountLockoutNotificationAsync(string email, string customerName, int lockoutDurationMinutes);

    /// <summary>
    /// Send security alert for suspicious activity
    /// </summary>
    /// <param name="email">Recipient email address</param>
    /// <param name="customerName">Customer name</param>
    /// <param name="activityDescription">Description of suspicious activity</param>
    /// <param name="ipAddress">IP address of suspicious activity</param>
    /// <param name="timestamp">Timestamp of activity</param>
    /// <returns>True if email was sent successfully</returns>
    Task<bool> SendSecurityAlertAsync(string email, string customerName, string activityDescription, string ipAddress, DateTime timestamp);

    /// <summary>
    /// Send equipment due date reminder
    /// </summary>
    /// <param name="email">Recipient email address</param>
    /// <param name="customerName">Customer name</param>
    /// <param name="equipmentList">List of equipment due for calibration</param>
    /// <returns>True if email was sent successfully</returns>
    Task<bool> SendEquipmentDueReminderAsync(string email, string customerName, List<EquipmentDueReminder> equipmentList);

    /// <summary>
    /// Test email configuration
    /// </summary>
    /// <param name="testEmail">Email address to send test to</param>
    /// <returns>True if test email was sent successfully</returns>
    Task<bool> SendTestEmailAsync(string testEmail);

    /// <summary>
    /// Get email sending statistics
    /// </summary>
    /// <param name="fromDate">Start date for statistics</param>
    /// <returns>Dictionary with email statistics</returns>
    Task<Dictionary<string, int>> GetEmailStatsAsync(DateTime fromDate);
}

/// <summary>
/// Email configuration options
/// </summary>
public class EmailOptions
{
    public const string SectionName = "EmailSettings";

    /// <summary>
    /// SMTP server hostname
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// SMTP server port
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// Enable SSL/TLS
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// SMTP username
    /// </summary>
    public string SmtpUsername { get; set; } = string.Empty;

    /// <summary>
    /// SMTP password
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// From email address
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// From display name
    /// </summary>
    public string FromName { get; set; } = "CalibrationSaaS Customer Portal";

    /// <summary>
    /// Email timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Enable email logging
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// Maximum emails per hour per recipient
    /// </summary>
    public int MaxEmailsPerHour { get; set; } = 10;

    /// <summary>
    /// Maximum emails per day per recipient
    /// </summary>
    public int MaxEmailsPerDay { get; set; } = 50;

    /// <summary>
    /// Validate email configuration
    /// </summary>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(SmtpServer) &&
               SmtpPort > 0 &&
               !string.IsNullOrEmpty(FromEmail);
    }

    /// <summary>
    /// Get validation errors
    /// </summary>
    public List<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(SmtpServer))
            errors.Add("SMTP server is required");

        if (SmtpPort <= 0)
            errors.Add("SMTP port must be greater than 0");

        if (string.IsNullOrEmpty(FromEmail))
            errors.Add("From email address is required");

        if (!string.IsNullOrEmpty(FromEmail) && !IsValidEmail(FromEmail))
            errors.Add("From email address is not valid");

        return errors;
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
}

/// <summary>
/// Two-factor email template data
/// </summary>
public class TwoFactorEmailData
{
    public string CustomerName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; }
    public string SupportEmail { get; set; } = "support@calibrationsaas.com";
    public string CompanyName { get; set; } = "CalibrationSaaS";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Equipment due reminder item
/// </summary>
public class EquipmentDueReminder
{
    public string SerialNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysUntilDue { get; set; }
    public string Status { get; set; } = string.Empty; // "Overdue", "Due Soon", etc.
}



/// <summary>
/// Email sending result
/// </summary>
public class EmailResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? MessageId { get; set; }
    public DateTime SentAt { get; set; }
    public TimeSpan SendDuration { get; set; }

    /// <summary>
    /// Create a successful email result
    /// </summary>
    public static EmailResult CreateSuccess(string? messageId = null, TimeSpan? duration = null)
    {
        return new EmailResult
        {
            Success = true,
            MessageId = messageId,
            SentAt = DateTime.UtcNow,
            SendDuration = duration ?? TimeSpan.Zero
        };
    }

    /// <summary>
    /// Create a failed email result
    /// </summary>
    public static EmailResult Failed(string errorMessage, TimeSpan? duration = null)
    {
        return new EmailResult
        {
            Success = false,
            ErrorMessage = errorMessage,
            SentAt = DateTime.UtcNow,
            SendDuration = duration ?? TimeSpan.Zero
        };
    }
}

/// <summary>
/// Email template types
/// </summary>
public enum EmailTemplateType
{
    TwoFactorCode,
    Welcome,
    AccountLockout,
    SecurityAlert,
    EquipmentDueReminder,
    Test
}
