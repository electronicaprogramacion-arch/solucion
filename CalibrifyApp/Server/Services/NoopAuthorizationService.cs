using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CalibrifyApp.Server.Services
{
    public class NoopAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            // Always succeed
            return Task.FromResult(AuthorizationResult.Success());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            // Always succeed
            return Task.FromResult(AuthorizationResult.Success());
        }
    }
}
