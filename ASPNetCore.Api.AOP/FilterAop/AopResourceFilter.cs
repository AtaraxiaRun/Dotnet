using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    /// 资源过滤器-缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AopResourceFilter : Attribute, IResourceFilter
    {

        private readonly int _duration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">秒</param>
        /// <param name="cache">缓存类</param>
        public AopResourceFilter(int duration)
        {
            _duration = duration;
        }

        /// <summary>
        /// 获取资源请求前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // 在操作执行前，检查是否已经有缓存的结果
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            var cacheKey = GenerateCacheKey(context);
            if (cache.TryGetValue(cacheKey, out object cachedValue))
            {
                //如果有缓存，那么直接返回结果，后面的Action过滤器不会执行,结果过滤器会执行，后面的中间件也会执行
                context.Result = new ContentResult()
                {
                    Content = cachedValue.ToString(),
                    ContentType = "application/json"
                };
            }
        }

        /// <summary>
        /// 获取资源请求后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            // 在操作执行后，将结果存在缓存
            if (context.Result is ObjectResult result)
            {
                var cacheKey = GenerateCacheKey(context);
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    // 设置缓存的过期时间
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_duration)

                };
                cache.Set(cacheKey, result.Value, cacheEntryOptions);
            }

        }



        /// <summary>
        /// 根据上下文生成缓存键
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GenerateCacheKey(FilterContext context)
        {
            var routeData = context.RouteData;
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();
            var cacheKey = $"{controllerName}_{actionName}";
            return cacheKey;
        }
    }
}
