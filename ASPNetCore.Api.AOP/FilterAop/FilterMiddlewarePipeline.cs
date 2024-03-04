namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    /// 不过可以通过细粒度控制的中间件：通过[MiddlewareFilter(FilterMiddlewarePipeline)]标记将指定的中间件作用到指定的控制器，方法，中间件筛选器与资源筛选器在筛选器管道的相同阶段运行，在授权过滤器后面执行
    /// </summary>
    public class FilterMiddlewarePipeline
    {
        /// <summary>
        /// 自定义中间件扩展类：
        /// 规范1：Configure开头
        /// 规范2：IApplicationBuilder app入参
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await Console.Out.WriteLineAsync("FilterMiddlewarePipeline自定义中间件Filter被触发");
                context.Response.Headers.Add("Name", "WangRui");
                await next();
            });
        }
    }
}
