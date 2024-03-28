
using ASPNetCore.Api.AOP.FilterAop;
using Microsoft.AspNetCore.Mvc.Filters;
using ASPNetCore.Api.AOP.Middlewares;

namespace ASPNetCore.Api.AOP
{
    /// <summary>
    /// https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0#use-middleware-in-the-filter-pipeline  我只能说这个太牛了 	 中间件也可以
    /// 过滤器执行顺序：https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-2.png?view=aspnetcore-8.0
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AopActionFilterAttribute>(); // ActionFilter注册到全局
                options.Filters.Add<MyGlobalFilter>(); //注册到全局，并且进行排除的ActionFilter
                options.Filters.Add<AopExceptionFilterAttribute>(); //ExceptionFilter注册到全局
                options.Filters.Add<AopResultFilterAttribute>(); // ResultFilterAttribute注册到全局
            });

            // AuthorizationFilter
            // ExceptionFilter
            // ResourceFilter
            // ResultFilter
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();// 注入日志服务
            builder.Services.AddMemoryCache(); // 添加缓存服务
            builder.Services.AddScoped<TimingActionFilter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMyMiddleware();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
