namespace NetCore.Api.Middleware.Middlewares
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
        {
            // 在中间件处理请求之前执行的逻辑
            // ...
            await Console.Out.WriteLineAsync("进入了自定义中间件-1");
            //int i1 = 0;
            //var i2 = 1 / i1;   //触发异常
            //进入下一个中间件
            await _next(context);
            // 在中间件处理请求之后执行的逻辑
            // ...
            await Console.Out.WriteLineAsync("进入了自定义中间件-2");

        }

    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}
