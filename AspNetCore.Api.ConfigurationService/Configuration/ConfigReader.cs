namespace AspNetCore.Api.ConfigurationService.Configuration
{
    /// <summary>
    /// 直接读取配置：数据库链接
    /// </summary>
    public class ConfigReader
    {
        public static void ReadConfig(IConfiguration configuration)
        {
        
            //1.【建议使用】方括号读取：["Data:DbConnectionString"]，也可以是["Data:DbConnectionString:Db3"]
            string db2 = configuration["Data:DbConnectionString"];
            Console.WriteLine($"方括号读取：[\"Data:DbConnectionString\"]; ：{db2}");

            //2.GetValue读取
            string db3 = configuration.GetValue<string>("Data:DbConnectionString");
            Console.WriteLine($"GetValue读取 ：{db3}");

            //3.GetSection
            string db4 = configuration.GetSection("Data:DbConnectionString").Get<string>();
            db4 = configuration.GetSection("Data")["DbConnectionString"]; //上下两种写法结果是一样的
            Console.WriteLine($"GetSection读取 ：{db4}");

            #region 注释
            //.无语了 ，GetConnectionString函数 只是用来读取数据库链接字符串的，ConnectionString写死了的
            // string db1 = configuration.GetConnectionString("Data:DbConnectionString");
            //Console.WriteLine($"GetConnectionString函数读取 ：{db1}");
            #endregion
        }
    }
}
