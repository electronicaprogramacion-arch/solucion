using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;

namespace BlazorApp1.Server.Middleware
{
    public class PdbBlockerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PdbBlockerMiddleware> _logger;

        public PdbBlockerMiddleware(RequestDelegate next, ILogger<PdbBlockerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            // Check if the request is for a PDB file or contains problematic patterns
            if (path != null && (path.EndsWith(".pdb") || path.Contains("fj7ijvg861") ||
                                (path.StartsWith("/_framework/") && path.Contains(".pdb"))))
            {
                _logger.LogWarning("Blocked request for PDB file: {Path}", path);

                // Return a 404 Not Found for PDB files
                context.Response.StatusCode = 404;
                return;
            }

            // For framework resources that might cause issues, provide a minimal response
            if (path != null && path.StartsWith("/_framework/") &&
                (path.Contains(".pdb") || path.Contains("debug") || path.Contains("symbols")))
            {
                _logger.LogWarning("Intercepted framework resource request: {Path}", path);

                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/octet-stream";
                await context.Response.WriteAsync(string.Empty);
                return;
            }

            // Continue processing the request
            try
            {
                await _next(context);
            }
            catch (Exception ex) when (path != null && (path.Contains(".pdb") || path.Contains("fj7ijvg861")))
            {
                // Suppress exceptions for PDB-related requests
                _logger.LogWarning("Suppressed exception for PDB-related request: {Path}, Error: {Error}",
                    path, ex.Message);

                context.Response.StatusCode = 404;
            }
        }
    }

    // Extension method to make it easier to add the middleware
    public static class PdbBlockerMiddlewareExtensions
    {
        public static IApplicationBuilder UsePdbBlocker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PdbBlockerMiddleware>();
        }
    }
}
