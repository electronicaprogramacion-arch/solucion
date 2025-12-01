using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Security
{
    /// <summary>
    /// Adapter class that allows using a CustomRemoteAuthenticationState service with the default RemoteAuthenticationState
    /// </summary>
    public class RemoteAuthenticationStateAdapter : IRemoteAuthenticationService<RemoteAuthenticationState>
    {
        private readonly IRemoteAuthenticationService<CustomRemoteAuthenticationState> _service;

        public RemoteAuthenticationStateAdapter(IRemoteAuthenticationService<CustomRemoteAuthenticationState> service)
        {
            _service = service;
        }

        public async Task<RemoteAuthenticationResult<RemoteAuthenticationState>> CompleteSignInAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            // Convert the context to use CustomRemoteAuthenticationState
            var customContext = new RemoteAuthenticationContext<CustomRemoteAuthenticationState>
            {
                State = context.State != null ? new CustomRemoteAuthenticationState() : null,
                Url = context.Url,
                InteractiveRequest = context.InteractiveRequest
            };

            // Call the service with the converted context
            var result = await _service.CompleteSignInAsync(customContext);

            // Convert the result back to use RemoteAuthenticationState
            return new RemoteAuthenticationResult<RemoteAuthenticationState>
            {
                Status = result.Status,
                ErrorMessage = result.ErrorMessage,
                State = context.State
            };
        }

        public async Task<RemoteAuthenticationResult<RemoteAuthenticationState>> CompleteSignOutAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            // Convert the context to use CustomRemoteAuthenticationState
            var customContext = new RemoteAuthenticationContext<CustomRemoteAuthenticationState>
            {
                State = context.State != null ? new CustomRemoteAuthenticationState() : null,
                Url = context.Url,
                InteractiveRequest = context.InteractiveRequest
            };

            // Call the service with the converted context
            var result = await _service.CompleteSignOutAsync(customContext);

            // Convert the result back to use RemoteAuthenticationState
            return new RemoteAuthenticationResult<RemoteAuthenticationState>
            {
                Status = result.Status,
                ErrorMessage = result.ErrorMessage,
                State = context.State
            };
        }

        public async Task<RemoteAuthenticationResult<RemoteAuthenticationState>> SignInAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            // Convert the context to use CustomRemoteAuthenticationState
            var customContext = new RemoteAuthenticationContext<CustomRemoteAuthenticationState>
            {
                State = context.State != null ? new CustomRemoteAuthenticationState() : null,
                Url = context.Url,
                InteractiveRequest = context.InteractiveRequest
            };

            // Call the service with the converted context
            var result = await _service.SignInAsync(customContext);

            // Convert the result back to use RemoteAuthenticationState
            return new RemoteAuthenticationResult<RemoteAuthenticationState>
            {
                Status = result.Status,
                ErrorMessage = result.ErrorMessage,
                State = context.State
            };
        }

        public async Task<RemoteAuthenticationResult<RemoteAuthenticationState>> SignOutAsync(RemoteAuthenticationContext<RemoteAuthenticationState> context)
        {
            // Convert the context to use CustomRemoteAuthenticationState
            var customContext = new RemoteAuthenticationContext<CustomRemoteAuthenticationState>
            {
                State = context.State != null ? new CustomRemoteAuthenticationState() : null,
                Url = context.Url,
                InteractiveRequest = context.InteractiveRequest
            };

            // Call the service with the converted context
            var result = await _service.SignOutAsync(customContext);

            // Convert the result back to use RemoteAuthenticationState
            return new RemoteAuthenticationResult<RemoteAuthenticationState>
            {
                Status = result.Status,
                ErrorMessage = result.ErrorMessage,
                State = context.State
            };
        }
    }
}
