using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CalibrationSaaS.Infraestructure.GrpcServices.Interceptors;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Extensions
{
    /// <summary>
    /// Extensions for tracking database performance
    /// </summary>
    public static class DatabasePerformanceExtensions
    {
        /// <summary>
        /// Execute a database operation with performance tracking
        /// </summary>
        public static async Task<T> ExecuteWithPerformanceTracking<T>(
            this DbContext context,
            Func<Task<T>> operation,
            string operationName,
            ILogger logger,
            IPerformanceMonitoringService? performanceService = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var startTime = DateTime.UtcNow;
            
            try
            {
                logger.LogDebug("🔍 Starting database operation: {Operation}", operationName);
                
                var result = await operation();
                
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                // Log based on performance
                if (duration > 2000) // More than 2 seconds
                {
                    logger.LogWarning("🐌 SLOW database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                else if (duration > 500) // More than 500ms
                {
                    logger.LogWarning("⚠️ Moderate database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                else
                {
                    logger.LogDebug("✅ Fast database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                
                // Send to performance monitoring service
                if (performanceService != null)
                {
                    await performanceService.LogDatabasePerformance(operationName, duration, true);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                logger.LogError(ex, "❌ Database operation failed: {Operation} took {Duration}ms - Error: {Error}", 
                    operationName, duration, ex.Message);
                
                // Send error to performance monitoring service
                if (performanceService != null)
                {
                    await performanceService.LogDatabasePerformance(operationName, duration, false, ex.Message);
                }
                
                throw;
            }
        }
        
        /// <summary>
        /// Execute a database operation with performance tracking (void return)
        /// </summary>
        public static async Task ExecuteWithPerformanceTracking(
            this DbContext context,
            Func<Task> operation,
            string operationName,
            ILogger logger,
            IPerformanceMonitoringService? performanceService = null)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                logger.LogDebug("🔍 Starting database operation: {Operation}", operationName);
                
                await operation();
                
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                // Log based on performance
                if (duration > 2000) // More than 2 seconds
                {
                    logger.LogWarning("🐌 SLOW database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                else if (duration > 500) // More than 500ms
                {
                    logger.LogWarning("⚠️ Moderate database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                else
                {
                    logger.LogDebug("✅ Fast database operation: {Operation} took {Duration}ms", operationName, duration);
                }
                
                // Send to performance monitoring service
                if (performanceService != null)
                {
                    await performanceService.LogDatabasePerformance(operationName, duration, true);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                logger.LogError(ex, "❌ Database operation failed: {Operation} took {Duration}ms - Error: {Error}", 
                    operationName, duration, ex.Message);
                
                // Send error to performance monitoring service
                if (performanceService != null)
                {
                    await performanceService.LogDatabasePerformance(operationName, duration, false, ex.Message);
                }
                
                throw;
            }
        }
    }
    
    /// <summary>
    /// Performance tracking wrapper for repositories
    /// </summary>
    public class PerformanceTrackingRepository<T> where T : class
    {
        private readonly ILogger _logger;
        private readonly IPerformanceMonitoringService _performanceService;
        private readonly string _entityName;
        
        public PerformanceTrackingRepository(ILogger logger, IPerformanceMonitoringService performanceService)
        {
            _logger = logger;
            _performanceService = performanceService;
            _entityName = typeof(T).Name;
        }
        
        public async Task<TResult> TrackOperation<TResult>(
            Func<Task<TResult>> operation,
            string operationName,
            object? metadata = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var fullOperationName = $"{_entityName}.{operationName}";
            
            try
            {
                _logger.LogDebug("🔍 Starting repository operation: {Operation}", fullOperationName);
                
                var result = await operation();
                
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                // Log based on performance
                if (duration > 1000) // More than 1 second
                {
                    _logger.LogWarning("🐌 SLOW repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                else if (duration > 300) // More than 300ms
                {
                    _logger.LogInformation("⚠️ Moderate repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                else
                {
                    _logger.LogDebug("✅ Fast repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                
                // Send to performance monitoring service
                await _performanceService.LogApplicationPerformance("Repository", fullOperationName, duration, metadata);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                _logger.LogError(ex, "❌ Repository operation failed: {Operation} took {Duration}ms - Error: {Error}", 
                    fullOperationName, duration, ex.Message);
                
                // Send error to performance monitoring service
                await _performanceService.LogApplicationPerformance("Repository", fullOperationName, duration, 
                    new { error = ex.Message, metadata });
                
                throw;
            }
        }
        
        public async Task TrackOperation(
            Func<Task> operation,
            string operationName,
            object? metadata = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var fullOperationName = $"{_entityName}.{operationName}";
            
            try
            {
                _logger.LogDebug("🔍 Starting repository operation: {Operation}", fullOperationName);
                
                await operation();
                
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                // Log based on performance
                if (duration > 1000) // More than 1 second
                {
                    _logger.LogWarning("🐌 SLOW repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                else if (duration > 300) // More than 300ms
                {
                    _logger.LogInformation("⚠️ Moderate repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                else
                {
                    _logger.LogDebug("✅ Fast repository operation: {Operation} took {Duration}ms", fullOperationName, duration);
                }
                
                // Send to performance monitoring service
                await _performanceService.LogApplicationPerformance("Repository", fullOperationName, duration, metadata);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                _logger.LogError(ex, "❌ Repository operation failed: {Operation} took {Duration}ms - Error: {Error}", 
                    fullOperationName, duration, ex.Message);
                
                // Send error to performance monitoring service
                await _performanceService.LogApplicationPerformance("Repository", fullOperationName, duration, 
                    new { error = ex.Message, metadata });
                
                throw;
            }
        }
    }
}
