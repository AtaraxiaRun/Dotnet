using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    /// 排除过滤器
    /// </summary>
    public class MyGlobalFilter : IActionFilter
    {
        /// <summary>
        /// 方法执行之前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 检查是否有IgnoreMyGlobalFilterAttribute，如果有则不执行过滤逻辑
            if (context.ActionDescriptor.EndpointMetadata.OfType<IgnoreMyGlobalFilterAttribute>().Any())
            {
                return;
            }
            Console.WriteLine("排除方法进来");
        }
        /// <summary>
        /// 方法执行之后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 检查是否有IgnoreMyGlobalFilterAttribute，如果有则不执行过滤逻辑
            if (context.ActionDescriptor.EndpointMetadata.OfType<IgnoreMyGlobalFilterAttribute>().Any())
            {
                return;
            }
            Console.WriteLine("排除方法出去");
        }


    }
}
