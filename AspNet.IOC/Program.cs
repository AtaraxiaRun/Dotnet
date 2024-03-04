using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.IOC
{
    internal class Program
    {
        /// <summary>
        /// https://www.kdocs.cn/l/crMl10mZAmuB 群友分享依赖注入
        /// 2024年3月4日阅读收获
        /// 1.每一个请求都会通过IServiceProvider创建一个IServiceProviderScope,我们可以在HttpContext里面找到这一个子容器
        /// 2.阅读源码的时候，设计模式有作用的，因为你可以一看这个命名就知道这段代码的道理在哪里了
        /// 3.IServiceProvider 根容器不可能获取的到，微软不会让你获取的，你平时用到的都是子容器
        /// 4.每一次获取作用域实例，单例实例，其实都是有缓存的，你获取了一次后续获取都是在缓存里面获取的（这种设计模式可以参考），瞬时模式没有加入缓存，并且单例模式是由根容器去控制释放的，只有你应用程序停止了（或者你主动释放），才会去释放
        /// 5.依赖注入的逻辑其实简单，比如ServiceDescription 保存每一个被注入的实例，IServiceCollection 保存实例的集合，IServiceProvider 管理容器的获取，释放   IServiceScope 里面有一个IServiceProvider对象，代表的是子容器，AutoFac就是支持注入的方式比较多，比如程序集注入，配置注入，属性注入，其实推荐用Scrutor，对Net Core依赖注入的方式进行扩展了很多多样化的注册函数
        /// 6.看源码调试可以学习到很多东西
        /// </summary>
        /// <param name="args"></param>
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
