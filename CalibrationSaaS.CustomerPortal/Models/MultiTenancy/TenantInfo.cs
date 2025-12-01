using Finbuckle.MultiTenant;

namespace CalibrationSaaS.CustomerPortal.Models.MultiTenancy;

/// <summary>
/// Tenant information for multi-tenancy support
/// </summary>
public class TenantInfo : ITenantInfo
{
    /// <summary>
    /// Unique tenant identifier
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Tenant identifier used in routing/resolution
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// Display name of the tenant
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Database connection string for this tenant
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Company name for the tenant
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Company logo URL
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Primary contact email
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Whether the tenant is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Tenant creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Custom settings for the tenant (JSON)
    /// </summary>
    public string? Settings { get; set; }

    /// <summary>
    /// Subscription plan for the tenant
    /// </summary>
    public string? SubscriptionPlan { get; set; }

    /// <summary>
    /// Maximum number of users allowed
    /// </summary>
    public int MaxUsers { get; set; } = 100;

    /// <summary>
    /// Maximum storage in MB
    /// </summary>
    public long MaxStorageMB { get; set; } = 1000;

    /// <summary>
    /// Time zone for the tenant
    /// </summary>
    public string TimeZone { get; set; } = "UTC";

    /// <summary>
    /// Culture/locale for the tenant
    /// </summary>
    public string Culture { get; set; } = "en-US";

    /// <summary>
    /// Custom domain for the tenant (if applicable)
    /// </summary>
    public string? CustomDomain { get; set; }

    /// <summary>
    /// SSL certificate thumbprint for custom domain
    /// </summary>
    public string? SslCertificateThumbprint { get; set; }

    /// <summary>
    /// Whether to enable audit logging for this tenant
    /// </summary>
    public bool EnableAuditLogging { get; set; } = true;

    /// <summary>
    /// Whether to enable email notifications
    /// </summary>
    public bool EnableEmailNotifications { get; set; } = true;

    /// <summary>
    /// SMTP settings for tenant-specific email configuration
    /// </summary>
    public string? SmtpSettings { get; set; }

    /// <summary>
    /// API rate limit settings
    /// </summary>
    public string? RateLimitSettings { get; set; }

    /// <summary>
    /// Backup configuration
    /// </summary>
    public string? BackupSettings { get; set; }

    /// <summary>
    /// gRPC service URL for this tenant
    /// </summary>
    public string? GrpcServiceUrl { get; set; }

    /// <summary>
    /// Get tenant-specific database connection string
    /// </summary>
    /// <param name="defaultConnectionString">Default connection string template</param>
    /// <returns>Tenant-specific connection string</returns>
    public string GetConnectionString(string defaultConnectionString)
    {
        if (!string.IsNullOrEmpty(ConnectionString))
        {
            return ConnectionString;
        }

        // If no specific connection string, use default with tenant-specific database
        if (!string.IsNullOrEmpty(Identifier))
        {
            return defaultConnectionString.Replace("{TenantId}", Identifier);
        }

        return defaultConnectionString;
    }

    /// <summary>
    /// Check if tenant has feature enabled
    /// </summary>
    /// <param name="featureName">Feature name to check</param>
    /// <returns>True if feature is enabled</returns>
    public bool HasFeature(string featureName)
    {
        if (string.IsNullOrEmpty(Settings))
            return false;

        try
        {
            var settings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(Settings);
            return settings?.ContainsKey($"feature_{featureName}") == true &&
                   settings[$"feature_{featureName}"].ToString()?.ToLower() == "true";
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Get tenant setting value
    /// </summary>
    /// <param name="settingName">Setting name</param>
    /// <param name="defaultValue">Default value if setting not found</param>
    /// <returns>Setting value or default</returns>
    public T GetSetting<T>(string settingName, T defaultValue = default!)
    {
        if (string.IsNullOrEmpty(Settings))
            return defaultValue;

        try
        {
            var settings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(Settings);
            if (settings?.ContainsKey(settingName) == true)
            {
                var value = settings[settingName];
                if (value is T directValue)
                    return directValue;

                // Try to convert
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
        catch
        {
            // Return default on any error
        }

        return defaultValue;
    }

    /// <summary>
    /// Set tenant setting value
    /// </summary>
    /// <param name="settingName">Setting name</param>
    /// <param name="value">Setting value</param>
    public void SetSetting<T>(string settingName, T value)
    {
        Dictionary<string, object> settings;

        if (string.IsNullOrEmpty(Settings))
        {
            settings = new Dictionary<string, object>();
        }
        else
        {
            try
            {
                settings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(Settings) 
                          ?? new Dictionary<string, object>();
            }
            catch
            {
                settings = new Dictionary<string, object>();
            }
        }

        settings[settingName] = value!;
        Settings = System.Text.Json.JsonSerializer.Serialize(settings);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validate tenant configuration
    /// </summary>
    /// <returns>List of validation errors</returns>
    public List<string> Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Id))
            errors.Add("Tenant ID is required");

        if (string.IsNullOrWhiteSpace(Identifier))
            errors.Add("Tenant Identifier is required");

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Tenant Name is required");

        if (string.IsNullOrWhiteSpace(CompanyName))
            errors.Add("Company Name is required");

        if (!string.IsNullOrWhiteSpace(ContactEmail) && !IsValidEmail(ContactEmail))
            errors.Add("Contact Email is not valid");

        if (MaxUsers <= 0)
            errors.Add("Max Users must be greater than 0");

        if (MaxStorageMB <= 0)
            errors.Add("Max Storage must be greater than 0");

        return errors;
    }

    /// <summary>
    /// Simple email validation
    /// </summary>
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
