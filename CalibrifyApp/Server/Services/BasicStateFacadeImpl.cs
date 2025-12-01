using Microsoft.Extensions.Logging;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Basic implementation of IBasicStateFacade that does nothing.
    /// Used to support components that require state management without authentication.
    /// </summary>
    public class BasicStateFacadeImpl : IBasicStateFacade
    {
        private readonly ILogger<BasicStateFacadeImpl> _logger;

        public BasicStateFacadeImpl(ILogger<BasicStateFacadeImpl> logger)
        {
            _logger = logger;
        }
    }
}
