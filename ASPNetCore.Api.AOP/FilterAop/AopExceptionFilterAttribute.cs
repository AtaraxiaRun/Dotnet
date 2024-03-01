using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ASPNetCore.Api.AOP.FilterAop
{
    public class AopExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public AopExceptionFilterAttribute(ILogger<AopExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            // 记录异常
            _logger.LogError(context.Exception, "处理请求时发生未处理的异常");

            // 在生产环境中不应该将详细的异常信息暴露给用户
            var errorReponse = new
            {
                error = new { message = "发生意外错误。请稍后再试。" }
            };

            var result = new ObjectResult(errorReponse)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            //根据不同的异常类型，可以返回不同的错误信息或者状态码
            if (context.Exception is UnauthorizedAccessException)
            {
                result = new ObjectResult(new
                {
                    error = new
                    {
                        message = "您无权执行此操作。" // 无权限时对用户显示的错误信息
                    }
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden // 无权限
                };
            }
            //根据需要添加其他的异常判断

            context.Result = result;
            context.ExceptionHandled = true; // 标记异常已经处理，防止继续抛出
        }
    }
}
