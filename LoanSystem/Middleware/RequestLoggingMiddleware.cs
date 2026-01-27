using System.Security.Claims;

namespace LoanSystem.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var requestTime = DateTime.UtcNow;
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            _logger.LogInformation(
                "Request: {Path} | Time: {Time} | UserId: {UserId}",
                requestPath,
                requestTime.ToString("yyyy-MM-dd HH:mm:ss"),
                userId);

            context.Response.Headers.Append("X-Request-Id", Guid.NewGuid().ToString());
            context.Response.Headers.Append("X-Request-Time", requestTime.ToString("o"));

            await _next(context);
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
