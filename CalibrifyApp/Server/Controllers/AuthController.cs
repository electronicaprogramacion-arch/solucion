using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CalibrifyApp.Server.Data;

namespace CalibrifyApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("state")]
        public async Task<IActionResult> GetAuthenticationState()
        {
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        
                        var authInfo = new
                        {
                            IsAuthenticated = true,
                            Name = user.UserName ?? user.Email,
                            Email = user.Email,
                            Id = user.Id,
                            Roles = roles.ToArray()
                        };

                        _logger.LogInformation("Returning authenticated state for user: {UserName}", user.UserName);
                        return Ok(authInfo);
                    }
                }

                _logger.LogInformation("Returning unauthenticated state");
                return Ok(new
                {
                    IsAuthenticated = false,
                    Name = (string?)null,
                    Email = (string?)null,
                    Id = (string?)null,
                    Roles = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting authentication state");
                return StatusCode(500, new
                {
                    IsAuthenticated = false,
                    Name = (string?)null,
                    Email = (string?)null,
                    Id = (string?)null,
                    Roles = Array.Empty<string>()
                });
            }
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshAuthenticationState()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    var authInfo = new
                    {
                        IsAuthenticated = true,
                        Name = user.UserName ?? user.Email,
                        Email = user.Email,
                        Id = user.Id,
                        Roles = roles.ToArray()
                    };

                    _logger.LogInformation("Refreshed authentication state for user: {UserName}", user.UserName);
                    return Ok(authInfo);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing authentication state");
                return StatusCode(500);
            }
        }
    }
}
