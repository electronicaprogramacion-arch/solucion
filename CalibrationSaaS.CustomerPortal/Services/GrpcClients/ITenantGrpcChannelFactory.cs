using Grpc.Net.Client;

namespace CalibrationSaaS.CustomerPortal.Services.GrpcClients;

/// <summary>
/// Factory for creating tenant-specific gRPC channels
/// </summary>
public interface ITenantGrpcChannelFactory
{
    /// <summary>
    /// Get or create a gRPC channel for the current tenant
    /// </summary>
    /// <returns>gRPC channel configured for the current tenant</returns>
    Task<GrpcChannel> GetChannelAsync();

    /// <summary>
    /// Get or create a gRPC channel for a specific tenant
    /// </summary>
    /// <param name="tenantId">Tenant identifier</param>
    /// <returns>gRPC channel configured for the specified tenant</returns>
    Task<GrpcChannel> GetChannelAsync(string tenantId);
}
