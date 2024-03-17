
using ASP.NET.CORE.API.JwtHandler.Handlers;
using ASP.NET.CORE.API.JwtHandler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASP.NET.CORE.API.JwtHandler
{
    /// <summary>
    /// 这是授权服务器的代码：生成Token,验证Token
    /// 引用Jar包：
    /// System.IdentityModel.Tokens.Jwt  
    /// Microsoft.IdentityModel.Tokens
    /// Microsoft.AspNetCore.Authentication.JwtBearer
    /// </summary>
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
            // 配置JWTTokenOption，通过appsettings.json中的"JWTTokenOption"部分来设置JWT选项。
            builder.Services.Configure<JWTTokenOption>(builder.Configuration.GetSection("JWTTokenOption"));

            // 注册IJwtService为单一实例（单例服务），并实例化JwtService，传递一个静态密钥和发行者标识。
            builder.Services.AddSingleton<IJwtService>(new JwtService("QUhlI9YTiXAZOxyFUH7a+gOMmUjwWb/Y/cS+1DXSgMg=", "your-issuer"));

            // 添加身份认证服务，并指定使用JWT Bearer作为默认的认证方案。
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 设定Token验证参数。
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 验证发行者签名密钥。
                        ValidateIssuerSigningKey = true,
                        // 设置发行者签名密钥（对称密钥），这里需要使用和发行JWT时相同的密钥。
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")),

                        // 验证Token的发行者。
                        ValidateIssuer = true,
                        // 指定有效的Token发行者。
                        ValidIssuer = "your-issuer",

                        // 验证Token的有效期。
                        ValidateLifetime = true,
                        // 设置时钟偏差为零，这意味着Token一旦过期将立即无效。
                        ClockSkew = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // 先鉴权
            app.UseAuthorization();  //再授权

            app.MapControllers();

            app.Run();
        }
    }
}
