using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    /// 授权方法
    /// AuthorizationFilterAttribute 特性授权在ASP.NET Core中，AuthorizationFilterAttribute 特性已经不再使用，这是一个ASP.NET MVC（ASP.NET 4.x）中的类。在ASP.NET Core中，授权是通过中间件和策略来进行的。
    /// </summary>
    public class AopAuthorizationFilter : Attribute, IAuthorizationFilter
    {

        private readonly string _requiredRole;

        public AopAuthorizationFilter(string requiredRole)
        {
            _requiredRole = requiredRole; ;
        }

        /// <summary>
        /// 授权逻辑
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("开始授权");
            //检查用户是否登录
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // 如果用户未登录，则返回401 状态码
                context.Result = new UnauthorizedResult();
                return;
            }


            // 检查用户是否具有所需角色
            if (!context.HttpContext.User.IsInRole(_requiredRole))
            {
                context.Result = new StatusCodeResult(403);
                return;
            }
        }
    }
}
