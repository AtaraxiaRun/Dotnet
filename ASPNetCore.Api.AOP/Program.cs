
using ASPNetCore.Api.AOP.FilterAop;
using Microsoft.AspNetCore.Mvc.Filters;
using ASPNetCore.Api.AOP.Middlewares;

namespace ASPNetCore.Api.AOP
{
    /// <summary>
    /// https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0#use-middleware-in-the-filter-pipeline  ��ֻ��˵���̫ţ�� 	 �м��Ҳ����
    /// ������ִ��˳��https://learn.microsoft.com/zh-cn/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-2.png?view=aspnetcore-8.0
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AopActionFilterAttribute>(); // ActionFilterע�ᵽȫ��
                options.Filters.Add<MyGlobalFilter>(); //ע�ᵽȫ�֣����ҽ����ų���ActionFilter
                options.Filters.Add<AopExceptionFilterAttribute>(); //ExceptionFilterע�ᵽȫ��
                options.Filters.Add<AopResultFilterAttribute>(); // ResultFilterAttributeע�ᵽȫ��
            });

            // AuthorizationFilter
            // ExceptionFilter
            // ResourceFilter
            // ResultFilter
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();// ע����־����
            builder.Services.AddMemoryCache(); // ��ӻ������
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
