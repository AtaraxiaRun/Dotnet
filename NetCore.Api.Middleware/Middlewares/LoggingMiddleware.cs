namespace NetCore.Api.Middleware.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request
            _logger.LogInformation("Handling request: " + context.Request.Path);

            // Call the next delegate/middleware in the pipeline
            await _next(context);

            // Log the response
            _logger.LogInformation("Finished handling request.");
        }
    }

    // 扩展方法用于添加中间件
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }

}
