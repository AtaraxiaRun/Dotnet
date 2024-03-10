
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace AspNetCore.Api.Pollys
{
    /// <summary>
    /// 这个案例挺牛的：https://cloud.tencent.com/developer/article/2304073
    /// 这两个结合着看：https://www.bilibili.com/video/BV1AM411C7Ny/?p=8&spm_id_from=pageDriver
//https://github.com/sevenwmz/StevenPollyForBilibili
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
           
            // Build the service provider
            var app = builder.Build();
            
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
