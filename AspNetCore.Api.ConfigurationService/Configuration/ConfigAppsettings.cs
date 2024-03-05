namespace AspNetCore.Api.ConfigurationService.Configuration
{
    /// <summary>
    /// appsettings.json操作类,统一静态管理
    /// </summary>
    public class ConfigAppsettings
    {
        public static IConfiguration Configuration { get; set; }
        static string contentPath { get; set; }


        public ConfigAppsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }

        /// <summary>
        /// 根据路径  configuration["App:Name"];
        /// </summary>
        /// <param name="sectionsPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionsPath)
        {
            try
            {
                return Configuration[sectionsPath];
            }
            catch (Exception) { }

            return "";

        }
    }
}
