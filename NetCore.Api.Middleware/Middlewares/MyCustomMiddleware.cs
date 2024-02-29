namespace NetCore.Api.Middleware.Middlewares
{
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext content, ILog log)
        {
            await Console.Out.WriteLineAsync("进入了有参的构造函数中间件");
            log.ShowLog("王锐");
            await _next(content);
        }
    }

    public static class MyCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }

    #region 日志类
    public interface ILog
    {
        void ShowLog(string message);
    }

    public class Log : ILog
    {
        public void ShowLog(string message)
        {
            Console.WriteLine($"{message}日志:{DateTime.Now.Millisecond}");
        }
    }
    #endregion
}
