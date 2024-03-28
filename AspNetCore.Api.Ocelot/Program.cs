using Ocelot.Middleware;
using Ocelot;
using Ocelot.DependencyInjection;
using Ocelot.Administration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json.Linq;

namespace AspNetCore.Api.Ocelot
{
    /// <summary>
    /// 网关服务:
    /// 1.Ocelot 可以直接连接API的组件，可以是单个服务，也可以是多个服务，在配置里面配置就可以，并不需要搭配Consul，搭配Consul只是为了动态伸缩
    /// 2.Ocelot 网关可以作为负载均衡服务器（单体架构也可以）也可以作为鉴权的开始（调用鉴权服务器WebApi鉴权），也可以作为
    /// 3.Ocelot 如果放在内网的话，那么前面一般有一台负载均衡器（Nginx）进行转发到内网
    /// 
    /// 问题：为什么要用Ocelot做负载均衡器呢？我用Nginx不行吗
    /// 答：Ocelot不用自己实现算法，简单，可以把它当作一个已经实现了负载均衡的中间件，我只是调用它API的方法而已
    /// 问题：身份验证和授权是什么意思？
    /// 答：身份验证（Authentication）是验证你是不是合法的用户，一般有
    /// 1. Single Key aka Authentication Scheme 单密钥又名身份验证方案：只用密钥就判断是不是合法的用户，如果密钥丢失了就很严重
    /// 2. Multiple Authentication Schemes 多种认证方案
    /// 3.JWT Tokens JWT 令牌 ：经常看到这个鉴权的方式
    /// 4.Identity Server Bearer Tokens 身份服务器承载令牌
    /// 5.Auth0 by Okta Auth0 由 Okta 提供
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            const string AuthenticationProviderKey = "MyKey";  //身份注册密钥


            new WebHostBuilder()
           .UseKestrel()
           .UseContentRoot(Directory.GetCurrentDirectory())
           .ConfigureAppConfiguration((hostingContext, config) =>
           {
               config
                   .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                   .AddJsonFile("appsettings.json", true, true)
                   .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                   .AddJsonFile("ocelot.json")
                   .AddEnvironmentVariables();
           })
           .ConfigureServices(services =>
           {
               services.AddOcelot()
               .AddAdministration("/administration", "wr123"); //内置的身份验证

               //{

               //    Action<JwtBearerOptions> options = o =>
               //    {
               //        o.Authority = "外部的身份验证地址";
               //        o.RequireHttpsMetadata = false;
               //        o.TokenValidationParameters = new TokenValidationParameters
               //        {
               //            ValidateAudience = false,
               //        };

               //    };

               //    services
               //        .AddOcelot()
               //        .AddAdministration("/administration", options);
               //}

           })
           .ConfigureLogging((hostingContext, logging) =>
           {
               //add your logging
           })
           .UseIISIntegration()
           .Configure(app =>
           {
               app.UseOcelot().Wait();
           })
           .Build()
           .Run();
        }
    }
}
