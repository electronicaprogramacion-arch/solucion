using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace CalibrifyApp.Server.Security
{
    /// <summary>
    /// Custom authentication state that extends RemoteAuthenticationState to include additional properties
    /// </summary>
    public class CustomRemoteAuthenticationState : RemoteAuthenticationState
    {
        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        public string? StateId { get; set; }

        /// <summary>
        /// Gets or sets the code verifier for PKCE.
        /// </summary>
        public string? CodeVerifier { get; set; }
    }
}
