using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Proxy for KavokuComponentBase that provides a default AuthenticationState.
    /// </summary>
    public class KavokuComponentBaseProxy : IComponent
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly Task<AuthenticationState> _defaultAuthStateTask;

        public KavokuComponentBaseProxy(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
            
            // Create a default AuthenticationState with an unauthenticated user
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);
            _defaultAuthStateTask = Task.FromResult(authState);
        }

        public Task<ClaimsPrincipal> GetUser()
        {
            return _defaultAuthStateTask.ContinueWith(task => task.Result.User);
        }

        public void Attach(RenderHandle renderHandle)
        {
            // Not used
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            // Not used
            return Task.CompletedTask;
        }
    }
}
