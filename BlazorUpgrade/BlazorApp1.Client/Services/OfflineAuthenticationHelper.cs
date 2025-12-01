using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BlazorApp1.Client.Services
{
    public class OfflineAuthenticationHelper
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILogger<OfflineAuthenticationHelper> _logger;

        public OfflineAuthenticationHelper(
            AuthenticationStateProvider authStateProvider,
            ILogger<OfflineAuthenticationHelper> logger)
        {
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        public async Task<(bool IsAuthenticated, ClaimsPrincipal User)> GetAuthenticationStateAsync()
        {
            try
            {
                var authState = await _authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                
                bool isAuthenticated = user?.Identity?.IsAuthenticated == true;
                
                _logger.LogInformation("Authentication state: {IsAuthenticated}", isAuthenticated);
                
                return (isAuthenticated, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authentication state");
                return (false, new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<string> GetUserNameAsync()
        {
            try
            {
                var (isAuthenticated, user) = await GetAuthenticationStateAsync();
                if (!isAuthenticated)
                    return null;

                return user.FindFirst("name")?.Value ?? 
                       user.FindFirst(ClaimTypes.Name)?.Value ?? 
                       user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting username");
                return null;
            }
        }

        public async Task<string[]> GetUserRolesAsync()
        {
            try
            {
                var (isAuthenticated, user) = await GetAuthenticationStateAsync();
                if (!isAuthenticated)
                    return Array.Empty<string>();

                var roles = new List<string>();
                foreach (var claim in user.FindAll(ClaimTypes.Role))
                {
                    roles.Add(claim.Value);
                }

                return roles.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user roles");
                return Array.Empty<string>();
            }
        }
    }
}