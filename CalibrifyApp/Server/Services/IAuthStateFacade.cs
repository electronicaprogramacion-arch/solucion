using Blazed.Controls;
using System;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Extended interface that combines IStateFacade with authentication properties
    /// </summary>
    public interface IAuthStateFacade : IStateFacade
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
        string Email { get; }
        string[] Roles { get; }
        string Token { get; }
        string RefreshToken { get; }
        DateTime Expiration { get; }
        bool IsExpired { get; }
        
        Task<bool> IsInRole(string role);
        Task<bool> IsInRoles(string[] roles);
        Task<bool> Login(string username, string password);
        Task Logout();
        Task<bool> RefreshTokenAsync();
        Task<bool> Register(string username, string password, string email);
    }
}
