
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
            #region ע�ᾲ̬������
            builder.Services.AddSingleton(new ConfigAppsettings(builder.Configuration));
            //�ڿ������е��ã�ʹ��
            #endregion
            #region ֱ�Ӷ�ȡ������
            ConfigReader.ReadConfig(builder.Configuration);
            //һ��򵥴ֱ��Ļ�����ֱ�������������ȡ
            #endregion
            #region ǿ���Ͷ�ȡ
            //����1��ֱ��ʹ��
            ConfigOptions.ReadOptionConfig(builder.Configuration);
            //����2��ע��ʹ��
            builder.Services.Configure<DbOptions>(
      builder.Configuration.GetSection(DbOptions.Data) //��һ���Ѿ���DbOptionsʵ��ע�뵽��������
      );



            //������������GetOptiongsDb��_dbOptionsֱ����ӿ�һ��ע��ʹ��
            //һ��Ծ���ʹ�õ����ԣ����ҵ��������кܶ����ԣ�������ʵ������ǿ���ͽ���ʹ�ã�ע��Ҳ���㣬�����Ҹо�����ֱ���þ�̬�������ȡ��ˬ
            #endregion
            #region ��ȡ��������ļ�
            /**
            INI �����ṩ����
            JSON �����ṩ���� ֻ��ʾ������������Լ�����������
            XML �����ṩ����
            **/
            //Ĭ�ϻ���� appsettings.json,appsettings.Environment.json ���ã����Ҫ������������õ�Configuration��������ȥ��Ҫͨ�������չ������ӣ���ȡ��ʽ��֮ǰһ��
            builder.Configuration.AddJsonFile("MyConfig.json",
        optional: true,
        reloadOnChange: true);
            // optional: true���ļ��ǿ�ѡ�ġ�
            //reloadOnChange: true��������ĺ�������ļ���
            /*
             ���һ�����еļ�ֵ�����Ե�ʱ���ã���
            foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}
             */
            Console.WriteLine($"�����һ��������{builder.Configuration["MyNames:Name1"]}");
            #endregion
            #region AddCommandLine(args)����û�����Ĳ����������н���ʹ��
            var switchMappings = new Dictionary<string, string>()
         {
             { "-k1", "key1" },
             { "-k2", "key2" }
         };
            //����û�����Ĳ�������ʹ��,�������Լ����һ�������ȥ������
            builder.Configuration.AddCommandLine(args, switchMappings);
            ConfigCommandLineRead.ReadConfigCommand(builder.Configuration);
            #endregion

            #region AddEnvironmentVariables
            builder.Configuration.AddEnvironmentVariables();  //����������������ϸ������

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
