using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPNetCore.Api.AOP.FilterAop
{

    /// <summary>
    /// 使用ActionFilter记录日志，同步版本
    /// </summary>
    public class AopActionFilterAttribute : ActionFilterAttribute
    {

        private readonly ILogger _logger;
        public AopActionFilterAttribute(ILogger<AopActionFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 方法执行之前
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"请求路径:{context.HttpContext.Request.Path} ,方法：{context.HttpContext.Request.Method}");
        }

        /// <summary>
        /// 方法执行之后
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"获取响应后的状态：{context.HttpContext.Response.StatusCode}");
        }
    }

    /// <summary>
    /// 异步版本：注意异步版本响应前，与响应后的内容都是在一个异步方法里面
    /// 实现了异步版本如果同时实现了同步版本，并且都注册了，那么会优先用异步版本
    /// </summary>
    public class AopActionFilterAttributeAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //在方法执行之前做一些事情
            await next(); //调用下一个过滤器中的代码
            //在方法执行之后做一些事情
        }
    }
}
