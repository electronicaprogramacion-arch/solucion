using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Mock implementation of IBasicAuthFacade that does nothing.
    /// Used to support components that require authentication without actually implementing it.
    /// </summary>
    public class MockStateFacade : IBasicAuthFacade
    {
        private readonly ILogger<MockStateFacade> _logger;

        public MockStateFacade(ILogger<MockStateFacade> logger)
        {
            _logger = logger;
        }

        public bool IsAuthenticated => true;

        public string UserName => "Guest User";

        public string Email => "guest@example.com";

        public string[] Roles => new[] { "User" };

        public string Token => string.Empty;

        public string RefreshToken => string.Empty;

        public DateTime Expiration => DateTime.Now.AddDays(1);

        public bool IsExpired => false;

        // Implement IBasicAuthFacade methods
        public Task<bool> IsInRole(string role)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsInRoles(string[] roles)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Login(string username, string password)
        {
            return Task.FromResult(true);
        }

        public Task Logout()
        {
            return Task.CompletedTask;
        }

        public Task<bool> RefreshTokenAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Register(string username, string password, string email)
        {
            return Task.FromResult(true);
        }
    }
}
