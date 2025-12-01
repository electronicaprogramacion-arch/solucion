using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace CalibrifyApp.Server.Security
{
    /// <summary>
    /// Server-side implementation of authentication paths provider
    /// Provides the paths for authentication operations
    /// </summary>
    public class ServerRemoteAuthenticationPathsProvider
    {
        private readonly RemoteAuthenticationApplicationPathsOptions _options;

        public ServerRemoteAuthenticationPathsProvider(IOptions<RemoteAuthenticationApplicationPathsOptions> options)
        {
            _options = options.Value;
        }

        // This property will be used by the RemoteAuthenticatorView component through reflection
        public RemoteAuthenticationApplicationPathsOptions ApplicationPaths => _options;
    }

    /// <summary>
    /// Options for configuring the paths used in the authentication process
    /// </summary>
    public class RemoteAuthenticationApplicationPathsOptions
    {
        public string LogInPath { get; set; } = "authentication/login";
        public string LogInCallbackPath { get; set; } = "authentication/login-callback";
        public string LogInFailedPath { get; set; } = "authentication/login-failed";
        public string RegisterPath { get; set; } = "authentication/register";
        public string ProfilePath { get; set; } = "authentication/profile";
        public string RemoteRegisterPath { get; set; } = "Identity/Account/Register";
        public string RemoteProfilePath { get; set; } = "Identity/Account/Manage";
        public string LogOutPath { get; set; } = "authentication/logout";
        public string LogOutCallbackPath { get; set; } = "authentication/logout-callback";
        public string LogOutFailedPath { get; set; } = "authentication/logout-failed";
        public string LogOutSucceededPath { get; set; } = "authentication/logout-succeeded";
    }
}
