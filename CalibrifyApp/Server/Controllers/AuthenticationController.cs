using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CalibrifyApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            try
            {
                _logger.LogInformation("Login endpoint called with returnUrl: {ReturnUrl}", returnUrl);

                // Get the identity server URL from configuration
                var authority = "https://localhost/";

                // Get the client ID
                var clientId = "CalibrationSaaS";

                // Get the redirect URI
                var redirectUri = $"{Request.Scheme}://{Request.Host}/authentication/login-callback";

                // Get the response type
                var responseType = "code";

                // Get the scopes
                var scopes = "openid profile";

                // Generate a random state value
                var state = Guid.NewGuid().ToString("N");

                // Generate a code verifier and challenge
                var codeVerifier = GenerateCodeVerifier();
                var codeChallenge = GenerateCodeChallenge(codeVerifier);

                // Store the code verifier and state in cookies
                Response.Cookies.Append("codeVerifier", codeVerifier, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });

                Response.Cookies.Append("state", state, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });

                Response.Cookies.Append("returnUrl", returnUrl, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });

                // Construct the authorization URL
                var authorizationUrl = $"{authority}connect/authorize?client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&response_type={responseType}&scope={Uri.EscapeDataString(scopes)}&state={state}&code_challenge={codeChallenge}&code_challenge_method=S256&response_mode=query";

                _logger.LogInformation("Redirecting to: {AuthorizationUrl}", authorizationUrl);

                // Redirect to the identity server
                return Redirect(authorizationUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Login endpoint");
                return StatusCode(500, "An error occurred during login");
            }
        }

        private string GenerateCodeVerifier()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private string GenerateCodeChallenge(string codeVerifier)
        {
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                return Convert.ToBase64String(challengeBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }
    }
}
