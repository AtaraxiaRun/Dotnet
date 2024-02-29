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
            var app = builder.Build();

            var log = app.Services.GetService<ILog>();
            log.WriteLog("张三");

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
