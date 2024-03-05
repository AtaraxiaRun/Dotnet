namespace AspNetCore.Api.ConfigurationService.Configuration
{
    public class ConfigCommandLineRead
    {
        /// <summary>
        /// 读取用户输入的参数值
        /// </summary>
        /// <param name="configuration"></param>
        public static void ReadConfigCommand(IConfiguration configuration)
        {
            Console.WriteLine($"key1:{configuration["key1"]} ,key2:{configuration["key2"]},keyUserName:{configuration["userName"]}");
        }
    }
}
