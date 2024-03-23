using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    /// 性能计时Filter
    /// 1.Items对象保存键值对的值，很不错 
    /// </summary>
    public class TimingActionFilter : IActionFilter
    {
        private const string StopwatchKey = "Stopwatch";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 在动作方法执行之前启动一个新的计时器
            var stopwatch = new Stopwatch();
            context.HttpContext.Items[StopwatchKey] = stopwatch; //Items对象保存键值对的值，很不错
            stopwatch.Start();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 在动作方法执行完毕后停止计时器
            if (context.HttpContext.Items[StopwatchKey] is Stopwatch stopwatch)
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                // 在这里你可以记录时间，例如输出到日志或其他存储
                Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} took {elapsedMilliseconds}ms.");

                // 或者将耗时添加到HTTP响应的头部（可选）
                context.HttpContext.Response.Headers["X-Action-Duration"] = $"{elapsedMilliseconds}ms";
            }
        }


    }
}
