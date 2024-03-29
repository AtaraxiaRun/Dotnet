﻿namespace NetCore.Api.Middleware.Middlewares
{
    /// <summary>
    /// 有ILog注入的中间件
    /// </summary>
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 带依赖注入的参数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="log">Log参数会自动进行注入</param>
        /// <returns></returns>
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
