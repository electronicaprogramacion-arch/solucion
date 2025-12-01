using System;
using System.Threading;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Example of an optimized gRPC service that uses the GrpcChannelFactory and retry policy
    /// </summary>
    public class OptimizedGrpcService
    {
        private readonly ILogger<OptimizedGrpcService> _logger;
        private readonly GrpcChannelFactory _channelFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _addressEndpoint;

        public OptimizedGrpcService(
            ILogger<OptimizedGrpcService> logger,
            GrpcChannelFactory channelFactory,
            IServiceProvider serviceProvider,
            string addressEndpoint)
        {
            _logger = logger;
            _channelFactory = channelFactory;
            _serviceProvider = serviceProvider;
            _addressEndpoint = addressEndpoint;
        }

        /// <summary>
        /// Example method that calls a gRPC service with retry policy using the factory directly
        /// </summary>
        public async Task<T> GetDataWithRetryUsingFactory<T>(Func<IBasicsServices<CallContext>, Task<T>> callFunc, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Calling gRPC service with retry policy using factory");
                return await _channelFactory.ExecuteWithRetryAsync<IBasicsServices<CallContext>, T>(
                    _addressEndpoint,
                    callFunc,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC service");
                throw;
            }
        }

        /// <summary>
        /// Example method that calls a gRPC service with retry policy using the extension method
        /// </summary>
        public async Task<T> GetDataWithRetryUsingExtension<T>(Func<IBasicsServices<CallContext>, Task<T>> callFunc, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Calling gRPC service with retry policy using extension method");
                return await _serviceProvider.ExecuteGrpcCallWithRetryAsync<IBasicsServices<CallContext>, T>(
                    callFunc,
                    _addressEndpoint,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC service");
                throw;
            }
        }

        /// <summary>
        /// Example method that calls a gRPC service with retry policy and CallContext
        /// </summary>
        public async Task<T> GetDataWithRetryAndContext<T>(Func<IBasicsServices<CallContext>, CallContext, Task<T>> callFunc, CallContext callContext, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Calling gRPC service with retry policy and CallContext");
                return await _serviceProvider.ExecuteGrpcCallWithRetryAsync<IBasicsServices<CallContext>, T>(
                    callFunc,
                    callContext,
                    _addressEndpoint,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling gRPC service");
                throw;
            }
        }
    }
}
