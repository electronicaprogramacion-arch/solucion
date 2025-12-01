using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    public class MockAccessTokenProvider : IAccessTokenProvider
    {
        public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            // Create a mock AccessTokenResult that indicates success
            var mockToken = new AccessToken { Value = "mock-token", Expires = System.DateTimeOffset.Now.AddHours(1) };
            var result = new AccessTokenResult(AccessTokenResultStatus.Success, mockToken, null);
            return new ValueTask<AccessTokenResult>(result);
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            // Create a mock AccessTokenResult that indicates success
            var mockToken = new AccessToken { Value = "mock-token", Expires = System.DateTimeOffset.Now.AddHours(1) };
            var result = new AccessTokenResult(AccessTokenResultStatus.Success, mockToken, null);
            return new ValueTask<AccessTokenResult>(result);
        }
    }
}
