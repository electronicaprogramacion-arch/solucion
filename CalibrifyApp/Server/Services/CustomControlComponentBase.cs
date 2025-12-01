using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Enum to represent the policies used in the application.
    /// </summary>
    public enum Policies
    {
        None,
        Read,
        Write,
        Delete,
        Admin
    }

    /// <summary>
    /// Class to represent a component in the application.
    /// </summary>
    public class Component
    {
        public string Route { get; set; }
        public bool IsModal { get; set; }
        public Policies Permission { get; set; }
    }

    /// <summary>
    /// Custom implementation of ControlComponentBase that provides a default AuthenticationState.
    /// </summary>
    public class CustomControlComponentBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> stateTask { get; set; }

        public Task<ClaimsPrincipal> GetUser()
        {
            // Create a guest user identity with minimal claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Guest User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("role", "User")
            }, "NoAuth");

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(user);
        }

        public Task<bool> IsInPolicy(Policies policy)
        {
            // Always return true since we're not using authentication
            return Task.FromResult(true);
        }

        public Task<bool> IsInPolicy(Policies policy, Component component)
        {
            // Always return true since we're not using authentication
            return Task.FromResult(true);
        }
    }
}
