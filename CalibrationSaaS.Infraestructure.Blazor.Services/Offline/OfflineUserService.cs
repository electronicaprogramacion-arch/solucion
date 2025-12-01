using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Security;
using Helpers.Controls;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using SqliteWasmHelper;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class OfflineUserService<TContext> where TContext : class
    {
        private readonly ISqliteWasmDbContextFactory<TContext> _dbFactory;
        private readonly ILogger<OfflineUserService<TContext>> _logger;

        public OfflineUserService(
            ISqliteWasmDbContextFactory<TContext> dbFactory,
            ILogger<OfflineUserService<TContext>> logger)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ClaimsPrincipal> GetAuthenticatedUserAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving authenticated user from offline storage");

                var currentUser = await GetStoredUserAsync();
                if (currentUser == null)
                {
                    _logger.LogWarning("No authenticated user found in offline storage");
                    return new ClaimsPrincipal(new ClaimsIdentity());
                }

                var identity = CreateClaimsIdentity(currentUser);
                var principal = new ClaimsPrincipal(identity);

                _logger.LogInformation("Successfully retrieved authenticated user: {UserName}", 
                    currentUser.Name ?? "Unknown");

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving authenticated user from offline storage");
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            try
            {
                var user = await GetStoredUserAsync();
                if (user == null)
                {
                    _logger.LogWarning("No current user found in offline storage");
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user from offline storage");
                return null;
            }
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            try
            {
                var user = await GetStoredUserAsync();
                return user != null && user.Claims?.Any() == true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user authentication status");
                return false;
            }
        }

        private async Task<CurrentUser> GetStoredUserAsync()
        {
            if (_dbFactory == null)
            {
                _logger.LogError("Database factory is null");
                return null;
            }

            using var db = await _dbFactory.CreateDbContextAsync();
            var user = await db.CurrentUser
                .AsNoTracking()
                .Include(x => x.Claims)
                .FirstOrDefaultAsync();

            return user;
        }

        private ClaimsIdentity CreateClaimsIdentity(CurrentUser currentUser)
        {
            var identity = new ClaimsIdentity("offline", "name", "role");

            // Add basic user claims
            if (!string.IsNullOrEmpty(currentUser.Name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, currentUser.Name));
                identity.AddClaim(new Claim("name", currentUser.Name));
            }

            if (currentUser.TechID > 0)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, currentUser.TechID.ToString()));
                identity.AddClaim(new Claim("sub", currentUser.TechID.ToString()));
            }

            // Add stored claims
            if (currentUser.Claims?.Any() == true)
            {
                foreach (var claim in currentUser.Claims)
                {
                    if (!string.IsNullOrEmpty(claim.Key) && !string.IsNullOrEmpty(claim.Value))
                    {
                        identity.AddClaim(new Claim(claim.Key, claim.Value));
                    }
                }
            }

            // Add roles if available
            if (!string.IsNullOrEmpty(currentUser.Roles))
            {
                var roles = currentUser.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                }
            }

            return identity;
        }
    }
}