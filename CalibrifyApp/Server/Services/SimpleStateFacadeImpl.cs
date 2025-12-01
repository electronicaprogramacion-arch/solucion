using Microsoft.Extensions.Logging;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Simple implementation of ISimpleStateFacade.
    /// </summary>
    public class SimpleStateFacadeImpl : ISimpleStateFacade
    {
        private readonly ILogger<SimpleStateFacadeImpl> _logger;

        public SimpleStateFacadeImpl(ILogger<SimpleStateFacadeImpl> logger)
        {
            _logger = logger;
        }
    }
}
