
namespace NetCore.Api.Middleware.Middlewares
{
    /// <summary>
    /// 通过接口创建的中间件
    /// </summary>
    public class MyInterfaceMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await Console.Out.WriteLineAsync("进入了接口创建的中间件");
            await next(context);
        }
    }

    public static class MyInterfaceMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyInterfaceMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyInterfaceMiddleware>();
        }


    }
}
