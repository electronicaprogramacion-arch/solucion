using CalibrifyApp.Server.Services;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Background service to clean up component states and prevent memory leaks
    /// </summary>
    public class ComponentCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ComponentCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5); // Run every 5 minutes
        private readonly TimeSpan _maxComponentAge = TimeSpan.FromHours(1); // Clean up components older than 1 hour

        public ComponentCleanupService(IServiceProvider serviceProvider, ILogger<ComponentCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[CLEANUP SERVICE] Component cleanup service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PerformCleanup();
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[CLEANUP SERVICE] Error during cleanup cycle");
                    // Continue running even if cleanup fails
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Wait 1 minute before retry
                }
            }

            _logger.LogInformation("[CLEANUP SERVICE] Component cleanup service stopped");
        }

        private async Task PerformCleanup()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                
                // Clean up component lifecycle optimizer
                var lifecycleOptimizer = scope.ServiceProvider.GetService<ComponentLifecycleOptimizer>();
                if (lifecycleOptimizer != null)
                {
                    lifecycleOptimizer.CleanupOldStates(_maxComponentAge);
                    _logger.LogDebug("[CLEANUP SERVICE] Component lifecycle states cleaned up");
                }

                // Force garbage collection periodically to help with memory management
                if (DateTime.UtcNow.Minute % 15 == 0) // Every 15 minutes
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    _logger.LogDebug("[CLEANUP SERVICE] Forced garbage collection completed");
                }

                // Log memory usage for monitoring
                var memoryBefore = GC.GetTotalMemory(false);
                var memoryAfter = GC.GetTotalMemory(true); // Force collection
                
                _logger.LogDebug("[CLEANUP SERVICE] Memory usage - Before: {MemoryBefore:N0} bytes, After: {MemoryAfter:N0} bytes, Freed: {MemoryFreed:N0} bytes",
                    memoryBefore, memoryAfter, memoryBefore - memoryAfter);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CLEANUP SERVICE] Error during cleanup operations");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CLEANUP SERVICE] Stopping component cleanup service");
            await base.StopAsync(cancellationToken);
        }
    }
}
