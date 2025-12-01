using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Server-side implementation of SignOutSessionStateManager that provides a no-op implementation
    /// for server-side rendering. This allows components that inject SignOutSessionStateManager to work
    /// in both WebAssembly and Server environments.
    /// </summary>
    public class ServerSignOutSessionStateManager : SignOutSessionStateManager
    {
        private readonly ILogger<ServerSignOutSessionStateManager> _logger;

        public ServerSignOutSessionStateManager(IJSRuntime jsRuntime, ILogger<ServerSignOutSessionStateManager> logger)
            : base(jsRuntime)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sets the sign-out state in the session. In server-side rendering, this is a no-op.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that completes when the sign-out state has been set.</returns>
        public override ValueTask SetSignOutState()
        {
            _logger.LogInformation("SetSignOutState called in server mode - this is a no-op implementation");
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// Validates the sign-out state in the session. In server-side rendering, this always returns true.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> that completes when the sign-out state has been validated,
        /// returning true if the sign-out state is valid.</returns>
        public override Task<bool> ValidateSignOutState()
        {
            _logger.LogInformation("ValidateSignOutState called in server mode - always returns true");
            return Task.FromResult(true);
        }
    }
}
