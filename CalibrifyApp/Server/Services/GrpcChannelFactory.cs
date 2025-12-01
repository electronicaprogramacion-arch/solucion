using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Factory for creating and managing gRPC channels to optimize performance
    /// </summary>
    public class GrpcChannelFactory : IDisposable
    {
        private readonly ILogger<GrpcChannelFactory> _logger;
        private readonly ConcurrentDictionary<string, GrpcChannel> _channels = new();
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(60);
        private readonly GrpcRetryPolicy _retryPolicy;
        private string _token;

        public GrpcChannelFactory(ILogger<GrpcChannelFactory> logger, ILogger<GrpcRetryPolicy> retryPolicyLogger)
        {
            _logger = logger;
            _retryPolicy = new GrpcRetryPolicy(retryPolicyLogger);
        }

        /// <summary>
        /// Creates or retrieves a cached gRPC service client
        /// </summary>
        /// <typeparam name="T">The type of gRPC service to create</typeparam>
        /// <param name="addressEndpoint">The endpoint address</param>
        /// <returns>A gRPC service client</returns>
        public T CreateService<T>(string addressEndpoint) where T : class
        {
            if (string.IsNullOrEmpty(addressEndpoint))
            {
                throw new ArgumentException("Address endpoint cannot be null or empty", nameof(addressEndpoint));
            }

            try
            {
                var channel = GetOrCreateChannel(addressEndpoint);
                return channel.CreateGrpcService<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gRPC service for {ServiceType} at {Endpoint}", typeof(T).Name, addressEndpoint);
                throw;
            }
        }

        /// <summary>
        /// Executes a gRPC call with retry policy
        /// </summary>
        /// <typeparam name="TService">The type of gRPC service</typeparam>
        /// <typeparam name="TResult">The return type of the gRPC call</typeparam>
        /// <param name="addressEndpoint">The endpoint address</param>
        /// <param name="callFunc">The function to execute the gRPC call</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The result of the gRPC call</returns>
        public async Task<TResult> ExecuteWithRetryAsync<TService, TResult>(
            string addressEndpoint,
            Func<TService, Task<TResult>> callFunc,
            CancellationToken cancellationToken = default) where TService : class
        {
            if (string.IsNullOrEmpty(addressEndpoint))
            {
                throw new ArgumentException("Address endpoint cannot be null or empty", nameof(addressEndpoint));
            }

            return await _retryPolicy.ExecuteWithRetryAsync(async () =>
            {
                var service = CreateService<TService>(addressEndpoint);
                return await callFunc(service);
            }, cancellationToken);
        }

        /// <summary>
        /// Gets an existing channel or creates a new one if it doesn't exist
        /// </summary>
        /// <param name="addressEndpoint">The endpoint address</param>
        /// <returns>A gRPC channel</returns>
        private GrpcChannel GetOrCreateChannel(string addressEndpoint)
        {
            return _channels.GetOrAdd(addressEndpoint, CreateChannel);
        }

        /// <summary>
        /// Creates a new gRPC channel with optimized settings
        /// </summary>
        /// <param name="addressEndpoint">The endpoint address</param>
        /// <returns>A new gRPC channel</returns>
        private GrpcChannel CreateChannel(string addressEndpoint)
        {
            _logger.LogInformation("Creating new gRPC channel for endpoint: {Endpoint}", addressEndpoint);

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            // Create handler with optimized settings
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                EnableMultipleHttp2Connections = true,
                MaxConnectionsPerServer = 10
            });

            // Create HTTP client with optimized timeout
            var httpClient = new HttpClient(handler)
            {
                Timeout = _defaultTimeout
            };

            // Create channel with optimized options
            var channel = GrpcChannel.ForAddress(addressEndpoint, new GrpcChannelOptions
            {
                HttpClient = httpClient,
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                MaxReceiveMessageSize = 10 * 1024 * 1024, // 10 MB
                MaxSendMessageSize = 10 * 1024 * 1024,    // 10 MB
            });

            return channel;
        }

        /// <summary>
        /// Sets the authentication token for gRPC calls
        /// </summary>
        /// <param name="token">The authentication token</param>
        public void SetToken(string token)
        {
            _token = token;

            // Clear existing channels to force recreation with new token
            foreach (var channel in _channels.Values)
            {
                try
                {
                    channel.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error disposing channel during token update");
                }
            }

            _channels.Clear();
        }

        /// <summary>
        /// Disposes all channels
        /// </summary>
        public void Dispose()
        {
            // Create a copy of the channels to avoid modification during enumeration
            var channelsToDispose = _channels.Values.ToArray();
            _channels.Clear();

            foreach (var channel in channelsToDispose)
            {
                try
                {
                    channel.Dispose();
                    _logger.LogDebug("Successfully disposed gRPC channel");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error disposing channel during factory disposal");
                }
            }
        }
    }
}
