using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.IOC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<ILog, Log>();
            builder.Services.AddScoped<ILog2, Log2>();
            builder.Services.AddSingleton<ILog3, Log3>();
            builder.Services.AddTransient<Log4>();  //直接注入实体对象

            var app = builder.Build();
            Console.WriteLine("**************************log***************");

            {
                var log = app.Services.GetService<ILog>();
                var log2 = app.Services.GetService<ILog>();
                Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                //瞬时输出:False
            }
            Console.WriteLine("**************************log2***************");

            {
                var log = app.Services.GetService<ILog2>();
                var log2 = app.Services.GetService<ILog2>();
                Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                //作用域：输出TRUE 
            }
            {
                var log = app.Services.GetService<ILog2>();
                using (var scope1 = app.Services.CreateScope())
                {
                    var log2 = scope1.ServiceProvider.GetService<ILog2>();
                    Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                    // 在不同作用域中解析，输出FALSE
                }
            }

            Console.WriteLine("**************************log3***************");
            {
                var log = app.Services.GetService<ILog3>();
                var log2 = app.Services.GetService<ILog3>();
                Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                //单例：输出TRUE
            }
            Console.WriteLine("**************************log4***************");
            {
                var log = app.Services.GetService<Log4>();
                var log2 = app.Services.GetService<Log4>();
                Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                //瞬时输出:False
            }



        }
    }

    public interface ILog
    {
        void WriteLog(string Message);
    }

    public class Log : ILog
    {
        private readonly ILogger<Log> _logger;
        public Log(ILogger<Log> logger)
        {
            _logger = logger;
        }
        public void WriteLog(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
