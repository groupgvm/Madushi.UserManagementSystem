using Geveo.Users.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Geveo.Users.Middleware
{
    public class LoggingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        public LoggingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInfo($"Incoming request path : {context.Request.Path}");

            // Invoke the next middleware in the pipeline
            await _next(context);

            // Get distinct response headers
            var responseHeaders = context.Response.Headers.Select(x => x.Value).Distinct().ToList();

            // Log distinct headers
            _logger.LogInfo($"Response headers : {string.Join(", ", responseHeaders)}");
        }
    }

    // Add the middleware to the HTTP request pipeline
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}