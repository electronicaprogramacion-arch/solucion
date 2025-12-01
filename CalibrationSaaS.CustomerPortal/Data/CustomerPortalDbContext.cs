using CalibrationSaaS.CustomerPortal.Models.Authentication;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalibrationSaaS.CustomerPortal.Data;

/// <summary>
/// Database context for Customer Portal with multi-tenancy support
/// </summary>
public class CustomerPortalDbContext : DbContext, IDataProtectionKeyContext
{
    private readonly ITenantInfo? _tenantInfo;

    public CustomerPortalDbContext(DbContextOptions<CustomerPortalDbContext> options, ITenantInfo? tenantInfo = null)
        : base(options)
    {
        _tenantInfo = tenantInfo;
    }

    // Authentication entities
    public DbSet<CustomerSession> CustomerSessions { get; set; }
    public DbSet<TwoFactorCode> TwoFactorCodes { get; set; }
    public DbSet<AuthenticationAuditLog> AuthenticationAuditLogs { get; set; }

    // Data Protection keys
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure multi-tenancy
        if (_tenantInfo != null)
        {
            // Add tenant isolation if needed
            // For now, we'll rely on separate databases per tenant
        }

        // Configure CustomerSession
        modelBuilder.Entity<CustomerSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);
            entity.Property(e => e.SessionId).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.CustomerName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.ContactName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.TenantId).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45); // IPv6 support
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            // Indexes
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.ContactId);
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.Email, e.IsActive });
        });

        // Configure TwoFactorCode
        modelBuilder.Entity<TwoFactorCode>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Code).HasMaxLength(10).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45);

            // Indexes
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.Email, e.IsUsed, e.ExpiresAt });
        });

        // Configure AuthenticationAuditLog
        modelBuilder.Entity<AuthenticationAuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Action).HasMaxLength(50).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.TenantId).HasMaxLength(50);
            entity.Property(e => e.Details).HasMaxLength(1000);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            entity.Property(e => e.SessionId).HasMaxLength(50);

            // Indexes
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Action);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => new { e.Email, e.Action, e.Timestamp });
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_tenantInfo?.ConnectionString != null)
        {
            optionsBuilder.UseSqlServer(_tenantInfo.ConnectionString);
        }

        // Enable detailed errors in development
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }

        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// Clean up expired sessions and codes
    /// </summary>
    public async Task CleanupExpiredDataAsync()
    {
        var cutoffTime = DateTime.UtcNow;

        // Remove expired sessions
        var expiredSessions = await CustomerSessions
            .Where(s => s.ExpiresAt < cutoffTime || !s.IsActive)
            .ToListAsync();

        if (expiredSessions.Any())
        {
            CustomerSessions.RemoveRange(expiredSessions);
        }

        // Remove expired 2FA codes
        var expiredCodes = await TwoFactorCodes
            .Where(c => c.ExpiresAt < cutoffTime || c.IsUsed)
            .ToListAsync();

        if (expiredCodes.Any())
        {
            TwoFactorCodes.RemoveRange(expiredCodes);
        }

        // Clean up old audit logs (keep last 90 days)
        var auditCutoff = DateTime.UtcNow.AddDays(-90);
        var oldAuditLogs = await AuthenticationAuditLogs
            .Where(a => a.Timestamp < auditCutoff)
            .ToListAsync();

        if (oldAuditLogs.Any())
        {
            AuthenticationAuditLogs.RemoveRange(oldAuditLogs);
        }

        await SaveChangesAsync();
    }

    /// <summary>
    /// Get active session count for monitoring
    /// </summary>
    public async Task<int> GetActiveSessionCountAsync()
    {
        return await CustomerSessions
            .Where(s => s.IsActive && s.ExpiresAt > DateTime.UtcNow)
            .CountAsync();
    }

    /// <summary>
    /// Get authentication statistics
    /// </summary>
    public async Task<Dictionary<string, int>> GetAuthenticationStatsAsync(DateTime fromDate)
    {
        var stats = await AuthenticationAuditLogs
            .Where(a => a.Timestamp >= fromDate)
            .GroupBy(a => a.Action)
            .Select(g => new { Action = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Action, x => x.Count);

        return stats;
    }
}
