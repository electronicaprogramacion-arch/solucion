using Microsoft.Extensions.Logging;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Null implementation of INullStateFacade.
    /// </summary>
    public class NullStateFacade : INullStateFacade
    {
        private readonly ILogger<NullStateFacade> _logger;

        public NullStateFacade(ILogger<NullStateFacade> logger)
        {
            _logger = logger;
        }
    }
}
