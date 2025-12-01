using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Extension methods for gRPC services
    /// </summary>
    public static class GrpcExtensions
    {
        /// <summary>
        /// Executes a gRPC call with retry policy
        /// </summary>
        /// <typeparam name="TService">The type of gRPC service</typeparam>
        /// <typeparam name="TResult">The return type of the gRPC call</typeparam>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="callFunc">The function to execute the gRPC call</param>
        /// <param name="addressEndpoint">Optional endpoint address (if not provided, the default endpoint will be used)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The result of the gRPC call</returns>
        public static async Task<TResult> ExecuteGrpcCallWithRetryAsync<TService, TResult>(
            this IServiceProvider serviceProvider,
            Func<TService, Task<TResult>> callFunc,
            string addressEndpoint = null,
            CancellationToken cancellationToken = default) where TService : class
        {
            var factory = serviceProvider.GetRequiredService<GrpcChannelFactory>();
            
            if (string.IsNullOrEmpty(addressEndpoint))
            {
                // Use the default endpoint from configuration
                addressEndpoint = CalibrationSaaS.Infraestructure.Blazor.Services.ConnectionStatusService.grpcUrl;
            }
            
            return await factory.ExecuteWithRetryAsync(addressEndpoint, callFunc, cancellationToken);
        }
        
        /// <summary>
        /// Executes a gRPC call with retry policy and CallContext
        /// </summary>
        /// <typeparam name="TService">The type of gRPC service</typeparam>
        /// <typeparam name="TResult">The return type of the gRPC call</typeparam>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="callFunc">The function to execute the gRPC call</param>
        /// <param name="callContext">The CallContext for the gRPC call</param>
        /// <param name="addressEndpoint">Optional endpoint address (if not provided, the default endpoint will be used)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The result of the gRPC call</returns>
        public static async Task<TResult> ExecuteGrpcCallWithRetryAsync<TService, TResult>(
            this IServiceProvider serviceProvider,
            Func<TService, CallContext, Task<TResult>> callFunc,
            CallContext callContext,
            string addressEndpoint = null,
            CancellationToken cancellationToken = default) where TService : class
        {
            return await serviceProvider.ExecuteGrpcCallWithRetryAsync<TService, TResult>(
                service => callFunc(service, callContext),
                addressEndpoint,
                cancellationToken);
        }
    }
}
