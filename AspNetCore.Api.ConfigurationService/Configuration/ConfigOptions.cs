namespace AspNetCore.Api.ConfigurationService.Configuration
{
    /// <summary>
    /// 读取强类型配置
    /// </summary>
    public class ConfigOptions
    {
        public static void ReadOptionConfig(IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection(DbOptions.Data)
                                                        .Get<DbOptions>();
            Console.WriteLine($"数据库链接为：{dbOptions?.DbConnectionString}");

        }
    }

    public class DbOptions
    {
        public static string Data = "Data";  //这个名字要和配置文件中第一个键的值一样，  "Data": { "DbConnectionString": "Server=localhost;Database=MyDatabase;User ID=sa;Password=password;"}
    public string DbConnectionString { get; set; }
    }
}
