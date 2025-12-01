using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp1.Server.Services
{
    /// <summary>
    /// Custom circuit handler to handle JSDisconnectedException.
    /// </summary>
    public class CustomCircuitHandler : CircuitHandler
    {
        private readonly ILogger<CustomCircuitHandler> _logger;

        public CustomCircuitHandler(ILogger<CustomCircuitHandler> logger)
        {
            _logger = logger;
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Circuit opened: {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Circuit closed: {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Connection down: {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connection up: {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }
    }
}
