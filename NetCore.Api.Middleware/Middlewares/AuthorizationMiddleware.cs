namespace NetCore.Api.Middleware.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 检查用户是否已认证
            if (!context.User.Identity.IsAuthenticated)
            {
                // 如果未认证，则返回401未授权状态码
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access Denied");
            }
            else
            {
                // 如果已认证，则继续执行下一个中间件
                await _next(context);
            }
        }
    }

    // 扩展方法用于添加中间件
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }

}
