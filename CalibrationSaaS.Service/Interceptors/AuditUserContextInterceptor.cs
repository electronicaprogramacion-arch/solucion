using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using CalibrationSaaS.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Interceptors
{
    /// <summary>
    /// gRPC interceptor that ensures user context is properly captured for audit logging
    /// This interceptor extracts the authenticated user from the gRPC call context
    /// and makes it available to the audit system
    /// </summary>
    public class AuditUserContextInterceptor : Interceptor
    {
        private readonly ILogger<AuditUserContextInterceptor> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AuditUserContextInterceptor(ILogger<AuditUserContextInterceptor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            // Extract user information from the gRPC call context
            SetUserContextForAudit(context);

            // Continue with the original call
            return await continuation(request, context);
        }

        public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            ServerCallContext context,
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            SetUserContextForAudit(context);
            return await continuation(requestStream, context);
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
            TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            SetUserContextForAudit(context);
            await continuation(request, responseStream, context);
        }

        public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            SetUserContextForAudit(context);
            await continuation(requestStream, responseStream, context);
        }

        /// <summary>
        /// Extract user context from gRPC call and set it for audit logging
        /// </summary>
        /// <param name="context">The gRPC server call context</param>
        private void SetUserContextForAudit(ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("AuditUserContextInterceptor: Starting user context extraction...");

                // Get the HTTP context from the gRPC call
                var httpContext = context.GetHttpContext();
                _logger.LogInformation("AuditUserContextInterceptor: HttpContext is {Status}", httpContext == null ? "null" : "available");

                if (httpContext?.User != null)
                {
                    var user = httpContext.User;
                    _logger.LogInformation("AuditUserContextInterceptor: User.Identity.IsAuthenticated: {IsAuth}, User.Identity.Name: '{Name}'",
                        user.Identity?.IsAuthenticated, user.Identity?.Name);

                    if (user.Identity?.IsAuthenticated == true)
                    {
                        // Log all claims for debugging
                        var claims = user.Claims.ToList();
                        _logger.LogInformation("AuditUserContextInterceptor: Available claims: {Claims}",
                            string.Join(", ", claims.Select(c => $"{c.Type}={c.Value}")));

                        // Extract username using the same logic as ServiceBase
                        var nameClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                        if (nameClaim == null)
                        {
                            nameClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                        }

                        // Also try "name" claim (lowercase) which is common in OIDC
                        if (nameClaim == null)
                        {
                            nameClaim = user.Claims.FirstOrDefault(x => x.Type == "name");
                        }

                        var userName = nameClaim?.Value;

                        _logger.LogInformation("AuditUserContextInterceptor: Captured user '{UserName}' from gRPC call", userName);

                        // Store user context in HTTP context items for audit system to access
                        if (!string.IsNullOrEmpty(userName))
                        {
                            httpContext.Items["AuditUserName"] = userName;
                            httpContext.Items["AuditUserId"] = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                            httpContext.Items["AuditUserClaims"] = user;

                            _logger.LogInformation("AuditUserContextInterceptor: Set audit user context for '{UserName}'", userName);
                        }
                        else
                        {
                            _logger.LogWarning("AuditUserContextInterceptor: UserName is null or empty after claim extraction");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("AuditUserContextInterceptor: User is not authenticated");
                    }
                }
                else
                {
                    _logger.LogWarning("AuditUserContextInterceptor: HttpContext or User is null");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuditUserContextInterceptor: Error setting user context for audit");
            }
        }
    }
}
