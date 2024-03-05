
using AspNetCore.Api.ConfigurationService.Configuration;

namespace AspNetCore.Api.ConfigurationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #region 注册静态配置类
            builder.Services.AddSingleton(new ConfigAppsettings(builder.Configuration));
            //在控制器中调用，使用
            #endregion
            #region 直接读取配置类
            ConfigReader.ReadConfig(builder.Configuration);
            //一般简单粗暴的话，就直接用这个方法读取
            #endregion
            #region 强类型读取
            //方法1：直接使用
            ConfigOptions.ReadOptionConfig(builder.Configuration);
            //方法2：注入使用
            builder.Services.Configure<DbOptions>(
      builder.Configuration.GetSection(DbOptions.Data) //这一行已经把DbOptions实体注入到了容器中
      );



            //看控制器方法GetOptiongsDb，_dbOptions直接像接口一样注入使用
            //一般对经常使用的属性，并且单个对象有很多属性，可以用实体声明强类型进行使用，注入也方便，不过我感觉还是直接用静态配置类读取最爽
            #endregion
            #region 读取多个配置文件
            /**
            INI 配置提供程序
            JSON 配置提供程序 只演示这个，其他的自己看官网案例
            XML 配置提供程序
            **/
            //默认会加载 appsettings.json,appsettings.Environment.json 配置，如果要添加其他的配置到Configuration对象里面去就要通过这个扩展方法添加，读取方式和之前一样
            builder.Configuration.AddJsonFile("MyConfig.json",
        optional: true,
        reloadOnChange: true);
            // optional: true：文件是可选的。
            //reloadOnChange: true：保存更改后会重载文件。
            /*
             输出一下所有的键值（调试的时候用）：
            foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}
             */
            Console.WriteLine($"简单输出一下姓名：{builder.Configuration["MyNames:Name1"]}");
            #endregion
            #region AddCommandLine(args)添加用户输入的参数到配置中进行使用
            var switchMappings = new Dictionary<string, string>()
         {
             { "-k1", "key1" },
             { "-k2", "key2" }
         };
            //添加用户输入的参数进行使用,还可以自己添加一个数组进去继续用
            builder.Configuration.AddCommandLine(args, switchMappings);
            ConfigCommandLineRead.ReadConfigCommand(builder.Configuration);
            #endregion

            #region AddEnvironmentVariables
            builder.Configuration.AddEnvironmentVariables();  //操作环境变量，详细看官网

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
