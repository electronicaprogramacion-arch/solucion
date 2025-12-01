using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazorApp1.Server.Middleware
{
    /// <summary>
    /// Custom middleware to handle errors.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");

                // Handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception
            _logger.LogError(exception, "An unhandled exception occurred");

            // Check if the exception is related to authentication
            bool isAuthException = exception.Message.Contains("authentication") ||
                                  exception.Message.Contains("Authorization") ||
                                  exception.Message.Contains("identity") ||
                                  exception.Message.Contains("login") ||
                                  exception.Message.Contains("token") ||
                                  exception.Message.Contains("claim") ||
                                  exception.GetType().Name.Contains("Auth");

            // If the exception is related to authentication, redirect to home
            if (isAuthException)
            {
                _logger.LogWarning("Authentication-related exception caught and handled: {Message}", exception.Message);
                context.Response.Redirect("/");
                return Task.CompletedTask;
            }

            // If the request is for a Blazor component, redirect to the appropriate error page
            if (context.Request.Path.StartsWithSegments("/_blazor"))
            {
                context.Response.Redirect("/");
                return Task.CompletedTask;
            }
            else if (context.Request.Path.StartsWithSegments("/EquipmentType"))
            {
                // Check if this is an AmbiguousMatchException
                if (exception.GetType().Name.Contains("AmbiguousMatchException") ||
                    exception.Message.Contains("matched multiple endpoints"))
                {
                    _logger.LogWarning("AmbiguousMatchException for EquipmentType route: {Message}", exception.Message);
                    // Redirect to our error page
                    context.Response.Redirect("/EquipmentTypeError");
                    return Task.CompletedTask;
                }

                // For other EquipmentType errors, try the original route
                _logger.LogWarning("Redirecting EquipmentType error to original route: {Message}", exception.Message);
                context.Response.Redirect("/EquipmentType");
                return Task.CompletedTask;
            }
            else if (context.Request.Path.StartsWithSegments("/Equipment"))
            {
                context.Response.Redirect("/");
                return Task.CompletedTask;
            }

            // For other requests, return a 500 status code
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Extension methods for the ErrorHandlingMiddleware.
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
