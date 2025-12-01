using CalibrationSaaS.CustomerPortal.Models.Authentication;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Service for managing session storage in the Customer Portal
/// </summary>
public interface ISessionStorageService
{
    /// <summary>
    /// Store a session for the current request context
    /// </summary>
    Task StoreSessionAsync(CustomerSession session);
    
    /// <summary>
    /// Retrieve the current session
    /// </summary>
    Task<CustomerSession?> GetCurrentSessionAsync();
    
    /// <summary>
    /// Clear the current session
    /// </summary>
    Task ClearSessionAsync();
    
    /// <summary>
    /// Check if there's an active session
    /// </summary>
    Task<bool> HasActiveSessionAsync();
}

/// <summary>
/// Implementation of session storage service using in-memory storage with IP/UserAgent fallback
/// </summary>
public class SessionStorageService : ISessionStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SessionStorageService> _logger;
    private static readonly Dictionary<string, CustomerSession> _sessionCache = new();
    private static readonly object _lock = new();

    public SessionStorageService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<SessionStorageService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public Task StoreSessionAsync(CustomerSession session)
    {
        try
        {
            var key = GetSessionKey();
            if (string.IsNullOrEmpty(key))
            {
                _logger.LogWarning("Unable to generate session key for storage");
                return Task.CompletedTask;
            }

            lock (_lock)
            {
                _sessionCache[key] = session;
                _logger.LogInformation("✅ Session stored for key: {Key} (SessionId: {SessionId})", 
                    key, session.SessionId);
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing session");
            return Task.CompletedTask;
        }
    }

    public Task<CustomerSession?> GetCurrentSessionAsync()
    {
        try
        {
            var key = GetSessionKey();
            if (string.IsNullOrEmpty(key))
            {
                return Task.FromResult<CustomerSession?>(null);
            }

            lock (_lock)
            {
                if (_sessionCache.TryGetValue(key, out var session))
                {
                    // Check if session is still valid
                    if (session.IsValid)
                    {
                        _logger.LogDebug("Session found for key: {Key} (SessionId: {SessionId})", 
                            key, session.SessionId);
                        return Task.FromResult<CustomerSession?>(session);
                    }
                    else
                    {
                        // Remove expired session
                        _sessionCache.Remove(key);
                        _logger.LogDebug("Removed expired session for key: {Key}", key);
                    }
                }
            }

            return Task.FromResult<CustomerSession?>(null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current session");
            return Task.FromResult<CustomerSession?>(null);
        }
    }

    public Task ClearSessionAsync()
    {
        try
        {
            var key = GetSessionKey();
            if (string.IsNullOrEmpty(key))
            {
                return Task.CompletedTask;
            }

            lock (_lock)
            {
                if (_sessionCache.Remove(key))
                {
                    _logger.LogInformation("Session cleared for key: {Key}", key);
                }
            }

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing session");
            return Task.CompletedTask;
        }
    }

    public async Task<bool> HasActiveSessionAsync()
    {
        var session = await GetCurrentSessionAsync();
        return session != null;
    }

    private string? GetSessionKey()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        // Create a unique key based on IP address and User-Agent
        // This is a simplified approach for the customer portal
        var ipAddress = GetClientIpAddress(httpContext);
        var userAgent = GetUserAgent(httpContext);
        
        if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(userAgent))
            return null;

        // Create a hash of IP + UserAgent for the key
        var keyData = $"{ipAddress}:{userAgent}";
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(keyData));
        return Convert.ToBase64String(hash)[..16]; // Use first 16 characters
    }

    private static string GetClientIpAddress(HttpContext httpContext)
    {
        var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        }
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        }
        return ipAddress ?? "Unknown";
    }

    private static string GetUserAgent(HttpContext httpContext)
    {
        return httpContext.Request.Headers.UserAgent.FirstOrDefault() ?? "Unknown";
    }
}
