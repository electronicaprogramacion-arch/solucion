using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Implements retry policies for gRPC calls
    /// </summary>
    public class GrpcRetryPolicy
    {
        private readonly ILogger<GrpcRetryPolicy> _logger;
        private readonly int _maxRetries;
        private readonly TimeSpan _initialBackoff;
        private readonly TimeSpan _maxBackoff;

        public GrpcRetryPolicy(ILogger<GrpcRetryPolicy> logger, int maxRetries = 3, 
            TimeSpan? initialBackoff = null, TimeSpan? maxBackoff = null)
        {
            _logger = logger;
            _maxRetries = maxRetries;
            _initialBackoff = initialBackoff ?? TimeSpan.FromSeconds(1);
            _maxBackoff = maxBackoff ?? TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Executes a gRPC call with retry policy
        /// </summary>
        /// <typeparam name="T">The return type of the gRPC call</typeparam>
        /// <param name="func">The gRPC call function</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The result of the gRPC call</returns>
        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default)
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount <= _maxRetries)
            {
                try
                {
                    if (retryCount > 0)
                    {
                        _logger.LogInformation("Retry attempt {RetryCount} of {MaxRetries}", retryCount, _maxRetries);
                    }

                    return await func();
                }
                catch (RpcException ex) when (IsTransientStatusCode(ex.StatusCode) && retryCount < _maxRetries)
                {
                    lastException = ex;
                    _logger.LogWarning(ex, "Transient gRPC error occurred (Status: {StatusCode}). Retrying...", ex.StatusCode);
                }
                catch (HttpRequestException ex) when (retryCount < _maxRetries)
                {
                    lastException = ex;
                    _logger.LogWarning(ex, "HTTP request error occurred. Retrying...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Non-transient error occurred during gRPC call");
                    throw;
                }

                retryCount++;
                
                // Calculate backoff with exponential increase and jitter
                TimeSpan delay = CalculateBackoffDelay(retryCount);
                _logger.LogInformation("Waiting {Delay}ms before retry", delay.TotalMilliseconds);
                
                try
                {
                    await Task.Delay(delay, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Operation was canceled during retry delay");
                    throw;
                }
            }

            _logger.LogError(lastException, "All retry attempts failed");
            throw lastException ?? new RpcException(new Status(StatusCode.Internal, "All retry attempts failed"));
        }

        /// <summary>
        /// Determines if a status code is transient and should be retried
        /// </summary>
        /// <param name="statusCode">The gRPC status code</param>
        /// <returns>True if the status code is transient, false otherwise</returns>
        private bool IsTransientStatusCode(StatusCode statusCode)
        {
            return statusCode == StatusCode.DeadlineExceeded ||
                   statusCode == StatusCode.ResourceExhausted ||
                   statusCode == StatusCode.Unavailable ||
                   statusCode == StatusCode.Aborted ||
                   statusCode == StatusCode.Internal ||
                   statusCode == StatusCode.Unknown;
        }

        /// <summary>
        /// Calculates the backoff delay with exponential increase and jitter
        /// </summary>
        /// <param name="retryCount">The current retry count</param>
        /// <returns>The backoff delay</returns>
        private TimeSpan CalculateBackoffDelay(int retryCount)
        {
            // Calculate exponential backoff: initialBackoff * 2^(retryCount-1)
            double backoffMilliseconds = _initialBackoff.TotalMilliseconds * Math.Pow(2, retryCount - 1);
            
            // Add jitter (random value between 0 and 1) to avoid thundering herd
            Random random = new Random();
            double jitter = random.NextDouble();
            backoffMilliseconds = backoffMilliseconds * (1 + jitter * 0.2);
            
            // Cap at max backoff
            backoffMilliseconds = Math.Min(backoffMilliseconds, _maxBackoff.TotalMilliseconds);
            
            return TimeSpan.FromMilliseconds(backoffMilliseconds);
        }
    }
}
