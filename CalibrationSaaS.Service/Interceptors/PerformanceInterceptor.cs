using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Interceptors
{
    /// <summary>
    /// gRPC interceptor for performance monitoring and logging
    /// </summary>
    public class PerformanceInterceptor : Interceptor
    {
        private readonly ILogger<PerformanceInterceptor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PerformanceInterceptor(ILogger<PerformanceInterceptor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var stopwatch = Stopwatch.StartNew();
            var methodName = context.Method;
            var startTime = DateTime.UtcNow;
            
            try
            {
                _logger.LogInformation("🚀 Starting gRPC call: {Method}", methodName);
                
                var response = await continuation(request, context);
                
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                // Log performance based on duration
                if (duration > 2000) // More than 2 seconds
                {
                    _logger.LogWarning("🐌 SLOW gRPC call: {Method} took {Duration}ms", methodName, duration);
                }
                else if (duration > 1000) // More than 1 second
                {
                    _logger.LogWarning("⚠️ Moderate gRPC call: {Method} took {Duration}ms", methodName, duration);
                }
                else
                {
                    _logger.LogInformation("✅ Fast gRPC call: {Method} took {Duration}ms", methodName, duration);
                }
                
                // Log detailed performance data
                LogPerformanceMetrics(methodName, duration, true, null, request, response);
                
                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;
                
                _logger.LogError(ex, "❌ gRPC call failed: {Method} took {Duration}ms - Error: {Error}", 
                    methodName, duration, ex.Message);
                
                // Log error performance data
                LogPerformanceMetrics<TRequest, TResponse>(methodName, duration, false, ex, request, default(TResponse));
                
                throw;
            }
        }

        private void LogPerformanceMetrics<TRequest, TResponse>(
            string methodName, 
            long duration, 
            bool success, 
            Exception? exception,
            TRequest request,
            TResponse? response)
        {
            try
            {
                var performanceData = new
                {
                    Timestamp = DateTime.UtcNow,
                    Method = methodName,
                    Duration = duration,
                    Success = success,
                    Error = exception?.Message,
                    RequestType = typeof(TRequest).Name,
                    ResponseType = typeof(TResponse)?.Name,
                    RequestSize = EstimateObjectSize(request),
                    ResponseSize = response != null ? EstimateObjectSize(response) : 0
                };

                // Log as structured data for analysis
                _logger.LogInformation("📊 Performance Data: {@PerformanceData}", performanceData);

                // Send to performance monitoring service if available
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var performanceService = scope.ServiceProvider.GetService<IPerformanceMonitoringService>();
                        if (performanceService != null)
                        {
                            await performanceService.LogGrpcPerformance(performanceData);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to send performance data to monitoring service");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to log performance metrics for {Method}", methodName);
            }
        }

        private int EstimateObjectSize(object obj)
        {
            //TODO
            return 0;
            try
            {
                if (obj == null) return 0;
                
                // Simple estimation based on JSON serialization
                var json = System.Text.Json.JsonSerializer.Serialize(obj);
                return System.Text.Encoding.UTF8.GetByteCount(json);
            }
            catch
            {
                return 0; // Fallback if serialization fails
            }
        }
    }

    /// <summary>
    /// Interface for performance monitoring service
    /// </summary>
    public interface IPerformanceMonitoringService
    {
        Task LogGrpcPerformance(object performanceData);
        Task LogDatabasePerformance(string operation, long duration, bool success, string? error = null);
        Task LogApplicationPerformance(string component, string operation, long duration, object? metadata = null);
    }

    /// <summary>
    /// Implementation of performance monitoring service
    /// </summary>
    public class PerformanceMonitoringService : IPerformanceMonitoringService
    {
        private readonly ILogger<PerformanceMonitoringService> _logger;
        private readonly List<object> _performanceBuffer = new();
        private readonly object _bufferLock = new();
        private readonly Timer _flushTimer;

        public PerformanceMonitoringService(ILogger<PerformanceMonitoringService> logger)
        {
            _logger = logger;
            
            // Flush performance data every 30 seconds
            _flushTimer = new Timer(FlushPerformanceData, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        }

        public async Task LogGrpcPerformance(object performanceData)
        {
            lock (_bufferLock)
            {
                _performanceBuffer.Add(new
                {
                    Type = "GRPC",
                    Data = performanceData,
                    Timestamp = DateTime.UtcNow
                });
            }
            
            await Task.CompletedTask;
        }

        public async Task LogDatabasePerformance(string operation, long duration, bool success, string? error = null)
        {
            var performanceData = new
            {
                Type = "DATABASE",
                Operation = operation,
                Duration = duration,
                Success = success,
                Error = error,
                Timestamp = DateTime.UtcNow
            };

            lock (_bufferLock)
            {
                _performanceBuffer.Add(performanceData);
            }

            // Log immediately for database operations over 1 second
            if (duration > 1000)
            {
                _logger.LogWarning("🐌 Slow database operation: {Operation} took {Duration}ms", operation, duration);
            }
            
            await Task.CompletedTask;
        }

        public async Task LogApplicationPerformance(string component, string operation, long duration, object? metadata = null)
        {
            var performanceData = new
            {
                Type = "APPLICATION",
                Component = component,
                Operation = operation,
                Duration = duration,
                Metadata = metadata,
                Timestamp = DateTime.UtcNow
            };

            lock (_bufferLock)
            {
                _performanceBuffer.Add(performanceData);
            }
            
            await Task.CompletedTask;
        }

        private void FlushPerformanceData(object? state)
        {
            List<object> dataToFlush;
            
            lock (_bufferLock)
            {
                if (_performanceBuffer.Count == 0) return;
                
                dataToFlush = new List<object>(_performanceBuffer);
                _performanceBuffer.Clear();
            }

            try
            {
                // Generate performance summary
                var summary = GeneratePerformanceSummary(dataToFlush);
                _logger.LogInformation("📊 Performance Summary: {@Summary}", summary);

                // Here you could send data to external monitoring systems
                // like Application Insights, Prometheus, etc.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush performance data");
            }
        }

        private object GeneratePerformanceSummary(List<object> performanceData)
        {
            var grpcCalls = performanceData.Where(d => d.ToString()?.Contains("GRPC") == true).Count();
            var dbOperations = performanceData.Where(d => d.ToString()?.Contains("DATABASE") == true).Count();
            var appOperations = performanceData.Where(d => d.ToString()?.Contains("APPLICATION") == true).Count();

            return new
            {
                TotalOperations = performanceData.Count,
                GrpcCalls = grpcCalls,
                DatabaseOperations = dbOperations,
                ApplicationOperations = appOperations,
                TimeWindow = "30 seconds",
                Timestamp = DateTime.UtcNow
            };
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            FlushPerformanceData(null); // Final flush
        }
    }
}
