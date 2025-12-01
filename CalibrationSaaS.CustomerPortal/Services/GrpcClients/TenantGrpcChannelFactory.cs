using CalibrationSaaS.CustomerPortal.Services.MultiTenancy;
using Finbuckle.MultiTenant;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Collections.Concurrent;

namespace CalibrationSaaS.CustomerPortal.Services.GrpcClients;

/// <summary>
/// Factory for creating tenant-specific gRPC channels
/// </summary>
public class TenantGrpcChannelFactory : ITenantGrpcChannelFactory, IDisposable
{
    private readonly ILogger<TenantGrpcChannelFactory> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITenantService _tenantService;
    private readonly IMultiTenantContextAccessor _multiTenantContextAccessor;
    private readonly ConcurrentDictionary<string, GrpcChannel> _channels = new();
    private readonly object _lock = new();
    private bool _disposed = false;

    public TenantGrpcChannelFactory(
        ILogger<TenantGrpcChannelFactory> logger,
        IConfiguration configuration,
        ITenantService tenantService,
        IMultiTenantContextAccessor multiTenantContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _tenantService = tenantService;
        _multiTenantContextAccessor = multiTenantContextAccessor;
    }

    public async Task<GrpcChannel> GetChannelAsync()
    {
        var tenantContext = _multiTenantContextAccessor.MultiTenantContext;
        if (tenantContext?.TenantInfo?.Identifier != null)
        {
            return await GetChannelAsync(tenantContext.TenantInfo.Identifier);
        }

        // Fallback to default configuration
        var defaultUrl = _configuration["GrpcSettings:CalibrationSaaSServiceUrl"] ?? "https://localhost:5333";
        return await GetChannelForUrlAsync("default", defaultUrl);
    }

    public async Task<GrpcChannel> GetChannelAsync(string tenantId)
    {
        if (string.IsNullOrEmpty(tenantId))
        {
            throw new ArgumentException("Tenant ID cannot be null or empty", nameof(tenantId));
        }

        // Check if we already have a channel for this tenant
        if (_channels.TryGetValue(tenantId, out var existingChannel))
        {
            return existingChannel;
        }

        // Get tenant information
        var tenantInfo = await _tenantService.GetTenantAsync(tenantId);
        if (tenantInfo == null)
        {
            _logger.LogWarning("Tenant {TenantId} not found, using default gRPC configuration", tenantId);
            var defaultUrl = _configuration["GrpcSettings:CalibrationSaaSServiceUrl"] ?? "https://localhost:5333";
            return await GetChannelForUrlAsync(tenantId, defaultUrl);
        }

        // Use tenant-specific gRPC URL or fallback to default
        var grpcUrl = tenantInfo.GrpcServiceUrl ?? _configuration["GrpcSettings:CalibrationSaaSServiceUrl"] ?? "https://localhost:5333";
        return await GetChannelForUrlAsync(tenantId, grpcUrl);
    }

    private Task<GrpcChannel> GetChannelForUrlAsync(string key, string serviceUrl)
    {
        // Double-check locking pattern
        if (_channels.TryGetValue(key, out var existingChannel))
        {
            return Task.FromResult(existingChannel);
        }

        lock (_lock)
        {
            if (_channels.TryGetValue(key, out existingChannel))
            {
                return Task.FromResult(existingChannel);
            }

            try
            {
                _logger.LogInformation("Creating gRPC channel for key {Key} with URL {ServiceUrl}", key, serviceUrl);

                var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
                var httpClient = new HttpClient(httpHandler)
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var channel = GrpcChannel.ForAddress(serviceUrl, new GrpcChannelOptions
                {
                    HttpClient = httpClient,
                    MaxReceiveMessageSize = 10 * 1024 * 1024, // 10 MB
                    MaxSendMessageSize = 10 * 1024 * 1024 // 10 MB
                });

                _channels.TryAdd(key, channel);
                return Task.FromResult(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gRPC channel for key {Key} with URL {ServiceUrl}", key, serviceUrl);
                throw;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (var channel in _channels.Values)
        {
            try
            {
                channel?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing gRPC channel");
            }
        }

        _channels.Clear();
        _disposed = true;
    }
}
