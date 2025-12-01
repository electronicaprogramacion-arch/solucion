using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    /// <summary>
    /// Interface for providing current user context information for audit logging
    /// </summary>
    public interface IUserContextProvider
    {
        /// <summary>
        /// Get the current authenticated user's username
        /// </summary>
        /// <returns>Username or null if not authenticated</returns>
        string GetCurrentUserName();

        /// <summary>
        /// Get the current authenticated user's ID
        /// </summary>
        /// <returns>User ID or null if not authenticated</returns>
        string GetUserId();

        /// <summary>
        /// Get the current user's tenant ID
        /// </summary>
        /// <returns>Tenant ID or null if not available</returns>
        int? GetTenantId();

        /// <summary>
        /// Get the current user's tenant name
        /// </summary>
        /// <returns>Tenant name or null if not available</returns>
        string GetTenantName();

        /// <summary>
        /// Get the current authenticated user's claims principal
        /// </summary>
        /// <returns>ClaimsPrincipal or null if not authenticated</returns>
        ClaimsPrincipal GetCurrentUser();

        /// <summary>
        /// Check if the current user is authenticated
        /// </summary>
        /// <returns>True if authenticated, false otherwise</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Get the current user's tech ID (CalibrationSaaS specific)
        /// </summary>
        /// <returns>Tech ID or null if not available</returns>
        int? GetTechId();
    }
}
