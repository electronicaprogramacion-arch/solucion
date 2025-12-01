using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.CustomerPortal.Models.Authentication;

/// <summary>
/// Represents an authenticated customer session
/// </summary>
public class CustomerSession
{
    [Key]
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public int CustomerId { get; set; }
    
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required]
    public int ContactId { get; set; }
    
    [Required]
    public string ContactName { get; set; } = string.Empty;
    
    [Required]
    public string TenantId { get; set; } = string.Empty;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    public DateTime? LastAccessedAt { get; set; }
    
    public string? IpAddress { get; set; }
    
    public string? UserAgent { get; set; }
    
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the session ended (for completed sessions)
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// When the session was last active
    /// </summary>
    public DateTime? LastActivityAt { get; set; }
    
    /// <summary>
    /// Check if the session is still valid
    /// </summary>
    public bool IsValid => IsActive && DateTime.UtcNow < ExpiresAt;
    
    /// <summary>
    /// Update last accessed time
    /// </summary>
    public void UpdateLastAccessed()
    {
        LastAccessedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Extend session expiration
    /// </summary>
    public void ExtendSession(TimeSpan extension)
    {
        ExpiresAt = DateTime.UtcNow.Add(extension);
        UpdateLastAccessed();
    }
    
    /// <summary>
    /// Invalidate the session
    /// </summary>
    public void Invalidate()
    {
        IsActive = false;
    }
}




