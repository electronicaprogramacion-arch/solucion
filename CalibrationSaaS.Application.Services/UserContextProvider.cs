using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace CalibrationSaaS.Application.Services
{
    /// <summary>
    /// Implementation of IUserContextProvider that extracts user context from HTTP context
    /// Integrates with the existing CalibrationSaaS identity system
    /// </summary>
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserContextProvider> _logger;

        public UserContextProvider(IHttpContextAccessor httpContextAccessor, ILogger<UserContextProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get the current authenticated user's username
        /// </summary>
        public string GetCurrentUserName()
        {
            try
            {
                _logger.LogInformation("UserContextProvider: GetCurrentUserName called");
                var httpContext = _httpContextAccessor.HttpContext;
                _logger.LogInformation("UserContextProvider: HttpContext is {Status}", httpContext == null ? "null" : "available");

                // First, check if the audit interceptor has stored the user context
                if (httpContext?.Items?.ContainsKey("AuditUserName") == true)
                {
                    var auditUserName = httpContext.Items["AuditUserName"]?.ToString();
                    if (!string.IsNullOrEmpty(auditUserName))
                    {
                        _logger.LogInformation("UserContextProvider: Using audit user context: '{UserName}'", auditUserName);
                        return auditUserName;
                    }
                    else
                    {
                        _logger.LogInformation("UserContextProvider: AuditUserName exists but is null/empty");
                    }
                }
                else
                {
                    _logger.LogInformation("UserContextProvider: No AuditUserName found in HttpContext.Items");
                }

                // Fallback to regular HTTP context user
                var user = GetCurrentUser();
                _logger.LogInformation("UserContextProvider: HttpContext.User is {UserStatus}",
                    user == null ? "null" : (user.Identity?.IsAuthenticated == true ? "authenticated" : "not authenticated"));

                if (user?.Identity?.IsAuthenticated != true)
                {
                    _logger.LogInformation("UserContextProvider: User is not authenticated, returning null");
                    return null;
                }

                // Log all available claims for debugging
                var claims = user.Claims.ToList();
                _logger.LogInformation("UserContextProvider: Available claims: {Claims}",
                    string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));

                // Try to get name claim (following the same pattern as ServiceBase.cs)
                var nameClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (nameClaim == null)
                {
                    // Fallback to NameIdentifier if Name claim is not available
                    nameClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                }

                // Also try "name" claim (lowercase) which is common in OIDC
                if (nameClaim == null)
                {
                    nameClaim = user.Claims.FirstOrDefault(x => x.Type == "name");
                }

                var userName = nameClaim?.Value;
                _logger.LogInformation("UserContextProvider: Resolved username: '{UserName}'", userName);
                return userName;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current user name");
                return null;
            }
        }

        /// <summary>
        /// Get the current authenticated user's ID
        /// </summary>
        public string GetUserId()
        {
            try
            {
                var user = GetCurrentUser();
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Try NameIdentifier first (standard user ID claim)
                var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                    return userIdClaim.Value;

                // Fallback to custom user ID claim if available
                var customUserIdClaim = user.Claims.FirstOrDefault(x => x.Type == "sub" || x.Type == "user_id");
                return customUserIdClaim?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current user ID");
                return null;
            }
        }

        /// <summary>
        /// Get the current user's tenant ID
        /// </summary>
        public int? GetTenantId()
        {
            try
            {
                var user = GetCurrentUser();
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Look for tenant ID in custom claims
                var tenantClaim = user.Claims.FirstOrDefault(x => 
                    x.Type == "tenant_id" || 
                    x.Type == "TenantId" || 
                    x.Type == "tech_id" ||
                    x.Type == "TechID");

                if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
                {
                    return tenantId;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current tenant ID");
                return null;
            }
        }

        /// <summary>
        /// Get the current user's tenant name
        /// </summary>
        public string GetTenantName()
        {
            try
            {
                var user = GetCurrentUser();
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Look for tenant name in custom claims
                var tenantNameClaim = user.Claims.FirstOrDefault(x => 
                    x.Type == "tenant_name" || 
                    x.Type == "TenantName" ||
                    x.Type == "company" ||
                    x.Type == "organization");

                return tenantNameClaim?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current tenant name");
                return null;
            }
        }

        /// <summary>
        /// Get the current authenticated user's claims principal
        /// </summary>
        public ClaimsPrincipal GetCurrentUser()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    _logger.LogInformation("UserContextProvider: Found HttpContext, User.Identity.Name: '{Name}', IsAuthenticated: {IsAuth}",
                        httpContext.User?.Identity?.Name, httpContext.User?.Identity?.IsAuthenticated);

                    // For gRPC calls, the user context should be available through HttpContext
                    // The gRPC service sets the HttpContext.User from the CallContext
                    return httpContext.User;
                }
                else
                {
                    _logger.LogWarning("UserContextProvider: HttpContext is null");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current user claims principal");
                return null;
            }
        }

        /// <summary>
        /// Check if the current user is authenticated
        /// </summary>
        public bool IsAuthenticated()
        {
            try
            {
                var user = GetCurrentUser();
                return user?.Identity?.IsAuthenticated == true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error checking authentication status");
                return false;
            }
        }

        /// <summary>
        /// Get the current user's tech ID (CalibrationSaaS specific)
        /// </summary>
        public int? GetTechId()
        {
            try
            {
                var user = GetCurrentUser();
                if (user?.Identity?.IsAuthenticated != true)
                    return null;

                // Look for tech ID in custom claims (CalibrationSaaS specific)
                var techIdClaim = user.Claims.FirstOrDefault(x => 
                    x.Type == "tech_id" || 
                    x.Type == "TechID" ||
                    x.Type == "technician_id");

                if (techIdClaim != null && int.TryParse(techIdClaim.Value, out int techId))
                {
                    return techId;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting current tech ID");
                return null;
            }
        }
    }
}
