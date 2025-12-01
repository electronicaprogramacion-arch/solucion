using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

#pragma warning disable CS0618 // Type or member is obsolete
namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Authentication handler that always succeeds and creates a guest user identity.
    /// Used to support routes that require authentication without actually implementing authentication.
    /// </summary>
    public class NoAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public NoAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Create a guest user identity
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Guest User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("role", "User")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return success result with the ticket
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
