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
    /// ���ط���:
    /// 1.Ocelot ����ֱ������API������������ǵ�������Ҳ�����Ƕ�������������������þͿ��ԣ�������Ҫ����Consul������Consulֻ��Ϊ�˶�̬����
    /// 2.Ocelot ���ؿ�����Ϊ���ؾ��������������ܹ�Ҳ���ԣ�Ҳ������Ϊ��Ȩ�Ŀ�ʼ�����ü�Ȩ������WebApi��Ȩ����Ҳ������Ϊ
    /// 3.Ocelot ������������Ļ�����ôǰ��һ����һ̨���ؾ�������Nginx������ת��������
    /// 
    /// ���⣺ΪʲôҪ��Ocelot�����ؾ������أ�����Nginx������
    /// ��Ocelot�����Լ�ʵ���㷨���򵥣����԰�������һ���Ѿ�ʵ���˸��ؾ�����м������ֻ�ǵ�����API�ķ�������
    /// ���⣺�����֤����Ȩ��ʲô��˼��
    /// �������֤��Authentication������֤���ǲ��ǺϷ����û���һ����
    /// 1. Single Key aka Authentication Scheme ����Կ���������֤������ֻ����Կ���ж��ǲ��ǺϷ����û��������Կ��ʧ�˾ͺ�����
    /// 2. Multiple Authentication Schemes ������֤����
    /// 3.JWT Tokens JWT ���� ���������������Ȩ�ķ�ʽ
    /// 4.Identity Server Bearer Tokens ��ݷ�������������
    /// 5.Auth0 by Okta Auth0 �� Okta �ṩ
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            const string AuthenticationProviderKey = "MyKey";  //���ע����Կ


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
               .AddAdministration("/administration", "wr123"); //���õ������֤

               //{

               //    Action<JwtBearerOptions> options = o =>
               //    {
               //        o.Authority = "�ⲿ�������֤��ַ";
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
