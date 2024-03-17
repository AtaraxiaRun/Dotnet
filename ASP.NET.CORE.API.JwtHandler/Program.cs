
using ASP.NET.CORE.API.JwtHandler.Handlers;
using ASP.NET.CORE.API.JwtHandler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASP.NET.CORE.API.JwtHandler
{
    /// <summary>
    /// ������Ȩ�������Ĵ��룺����Token,��֤Token
    /// ����Jar����
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
            // ����JWTTokenOption��ͨ��appsettings.json�е�"JWTTokenOption"����������JWTѡ�
            builder.Services.Configure<JWTTokenOption>(builder.Configuration.GetSection("JWTTokenOption"));

            // ע��IJwtServiceΪ��һʵ�����������񣩣���ʵ����JwtService������һ����̬��Կ�ͷ����߱�ʶ��
            builder.Services.AddSingleton<IJwtService>(new JwtService("QUhlI9YTiXAZOxyFUH7a+gOMmUjwWb/Y/cS+1DXSgMg=", "your-issuer"));

            // ��������֤���񣬲�ָ��ʹ��JWT Bearer��ΪĬ�ϵ���֤������
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // �趨Token��֤������
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ��֤������ǩ����Կ��
                        ValidateIssuerSigningKey = true,
                        // ���÷�����ǩ����Կ���Գ���Կ����������Ҫʹ�úͷ���JWTʱ��ͬ����Կ��
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")),

                        // ��֤Token�ķ����ߡ�
                        ValidateIssuer = true,
                        // ָ����Ч��Token�����ߡ�
                        ValidIssuer = "your-issuer",

                        // ��֤Token����Ч�ڡ�
                        ValidateLifetime = true,
                        // ����ʱ��ƫ��Ϊ�㣬����ζ��Tokenһ�����ڽ�������Ч��
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

            app.UseAuthentication(); // �ȼ�Ȩ
            app.UseAuthorization();  //����Ȩ

            app.MapControllers();

            app.Run();
        }
    }
}
