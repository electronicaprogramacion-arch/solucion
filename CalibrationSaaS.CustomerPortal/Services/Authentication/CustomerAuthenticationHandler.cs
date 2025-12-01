using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Authentication scheme options for customer portal
/// </summary>
public class CustomerAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Session timeout in minutes
    /// </summary>
    public int SessionTimeoutMinutes { get; set; } = 30;

    /// <summary>
    /// Whether to require HTTPS
    /// </summary>
    public bool RequireHttps { get; set; } = true;

    /// <summary>
    /// Cookie name for session
    /// </summary>
    public string CookieName { get; set; } = "CustomerPortalSession";

    /// <summary>
    /// Login path
    /// </summary>
    public string LoginPath { get; set; } = "/login";

    /// <summary>
    /// Logout path
    /// </summary>
    public string LogoutPath { get; set; } = "/logout";
}

/// <summary>
/// Authentication handler for customer portal sessions
/// </summary>
public class CustomerAuthenticationHandler : AuthenticationHandler<CustomerAuthenticationSchemeOptions>
{
    private readonly ICustomerAuthenticationService _authService;
    private readonly ILogger<CustomerAuthenticationHandler> _logger;

    public CustomerAuthenticationHandler(
        IOptionsMonitor<CustomerAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ICustomerAuthenticationService authService)
        : base(options, logger, encoder)
    {
        _authService = authService;
        _logger = logger.CreateLogger<CustomerAuthenticationHandler>();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // Get session from authentication service
            var session = await _authService.GetCurrentSessionAsync();
            
            if (session == null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            {
                return AuthenticateResult.NoResult();
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, session.Email),
                new(ClaimTypes.Name, session.ContactName),
                new(ClaimTypes.NameIdentifier, session.ContactId.ToString()),
                new("CustomerId", session.CustomerId.ToString()),
                new("CustomerName", session.CustomerName),
                new("TenantId", session.TenantId),
                new("SessionId", session.SessionId)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication");
            return AuthenticateResult.Fail("Authentication error occurred");
        }
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var redirectUri = properties.RedirectUri ?? Options.LoginPath;
        
        if (!string.IsNullOrEmpty(Request.Path))
        {
            redirectUri += $"?returnUrl={Uri.EscapeDataString(Request.Path + Request.QueryString)}";
        }

        Response.Redirect(redirectUri);
        await Task.CompletedTask;
    }

    protected async Task HandleSignOutAsync(AuthenticationProperties? properties = null)
    {
        try
        {
            // Get current session and invalidate it
            var session = await _authService.GetCurrentSessionAsync();
            if (session != null)
            {
                await _authService.LogoutAsync(session.SessionId);
            }

            // Clear session cookie
            Response.Cookies.Delete(Options.CookieName);

            // Redirect to logout page or login page
            var redirectUri = properties?.RedirectUri ?? Options.LoginPath;
            Response.Redirect(redirectUri);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign out");
            Response.Redirect(Options.LoginPath);
        }
    }
}

/// <summary>
/// Extension methods for authentication configuration
/// </summary>
public static class CustomerAuthenticationExtensions
{
    /// <summary>
    /// Add customer authentication to the service collection
    /// </summary>
    public static AuthenticationBuilder AddCustomerAuthentication(
        this AuthenticationBuilder builder,
        Action<CustomerAuthenticationSchemeOptions>? configureOptions = null)
    {
        return builder.AddScheme<CustomerAuthenticationSchemeOptions, CustomerAuthenticationHandler>(
            "CustomerPortal", 
            "Customer Portal Authentication", 
            configureOptions);
    }

    /// <summary>
    /// Add customer authentication to the service collection
    /// </summary>
    public static IServiceCollection AddCustomerAuthentication(
        this IServiceCollection services,
        Action<CustomerAuthenticationSchemeOptions>? configureOptions = null)
    {
        services.AddAuthentication("CustomerPortal")
            .AddCustomerAuthentication(configureOptions);

        return services;
    }
}

/// <summary>
/// Claims constants for customer authentication
/// </summary>
public static class CustomerClaims
{
    public const string CustomerId = "CustomerId";
    public const string CustomerName = "CustomerName";
    public const string TenantId = "TenantId";
    public const string SessionId = "SessionId";
    public const string ContactId = "ContactId";
}

/// <summary>
/// Helper methods for working with customer claims
/// </summary>
public static class CustomerClaimsHelper
{
    /// <summary>
    /// Get customer ID from claims principal
    /// </summary>
    public static int? GetCustomerId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(CustomerClaims.CustomerId);
        return claim != null && int.TryParse(claim.Value, out var customerId) ? customerId : null;
    }

    /// <summary>
    /// Get customer name from claims principal
    /// </summary>
    public static string? GetCustomerName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(CustomerClaims.CustomerName)?.Value;
    }

    /// <summary>
    /// Get tenant ID from claims principal
    /// </summary>
    public static string? GetTenantId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(CustomerClaims.TenantId)?.Value;
    }

    /// <summary>
    /// Get session ID from claims principal
    /// </summary>
    public static string? GetSessionId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(CustomerClaims.SessionId)?.Value;
    }

    /// <summary>
    /// Get contact ID from claims principal
    /// </summary>
    public static int? GetContactId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(CustomerClaims.ContactId);
        return claim != null && int.TryParse(claim.Value, out var contactId) ? contactId : null;
    }

    /// <summary>
    /// Get email from claims principal
    /// </summary>
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value;
    }

    /// <summary>
    /// Get full name from claims principal
    /// </summary>
    public static string? GetFullName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    /// <summary>
    /// Check if user is authenticated
    /// </summary>
    public static bool IsCustomerAuthenticated(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated == true &&
               principal.Claims.Any(c => c.Type == CustomerClaims.CustomerId) &&
               !string.IsNullOrEmpty(principal.FindFirst(CustomerClaims.CustomerId)?.Value);
    }

    /// <summary>
    /// Check if user belongs to specific tenant
    /// </summary>
    public static bool BelongsToTenant(this ClaimsPrincipal principal, string tenantId)
    {
        var userTenantId = principal.GetTenantId();
        return !string.IsNullOrEmpty(userTenantId) && 
               string.Equals(userTenantId, tenantId, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Check if user belongs to specific customer
    /// </summary>
    public static bool BelongsToCustomer(this ClaimsPrincipal principal, int customerId)
    {
        var userCustomerId = principal.GetCustomerId();
        return userCustomerId.HasValue && userCustomerId.Value == customerId;
    }
}
