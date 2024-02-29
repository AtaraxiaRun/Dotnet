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
            Console.WriteLine("**************************log3***************");
            {
                var log = app.Services.GetService<ILog3>();
                var log2 = app.Services.GetService<ILog3>();
                Console.WriteLine($"log : {log.GetHashCode()} ,log2 ： {log2.GetHashCode()},{log.GetHashCode() == log2.GetHashCode()}");
                //单例：输出TRUE
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
