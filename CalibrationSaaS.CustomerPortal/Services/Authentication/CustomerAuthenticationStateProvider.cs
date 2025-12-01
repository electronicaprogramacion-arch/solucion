using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CalibrationSaaS.CustomerPortal.Services.Authentication;

/// <summary>
/// Authentication state provider for the Customer Portal
/// </summary>
public class CustomerAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ICustomerAuthenticationService _authService;
    private readonly ILogger<CustomerAuthenticationStateProvider> _logger;

    public CustomerAuthenticationStateProvider(
        ICustomerAuthenticationService authService,
        ILogger<CustomerAuthenticationStateProvider> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var session = await _authService.GetCurrentSessionAsync();
            
            if (session == null || !session.IsActive || session.ExpiresAt <= DateTime.UtcNow)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, session.Email),
                new(ClaimTypes.Name, session.ContactName),
                new(ClaimTypes.NameIdentifier, session.ContactId.ToString()),
                new(CustomerClaims.CustomerId, session.CustomerId.ToString()),
                new(CustomerClaims.CustomerName, session.CustomerName),
                new(CustomerClaims.TenantId, session.TenantId),
                new(CustomerClaims.SessionId, session.SessionId)
            };

            var identity = new ClaimsIdentity(claims, "CustomerPortal");
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting authentication state");
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    /// <summary>
    /// Notify that the authentication state has changed
    /// </summary>
    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
