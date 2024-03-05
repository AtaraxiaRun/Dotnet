using AspNetCore.Api.ConfigurationService.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCore.Api.ConfigurationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DbOptions _dbOptions;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IOptions<DbOptions> dbOptions)
        {
            _logger = logger;
            _dbOptions = dbOptions.Value;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// ͨ����̬���ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDbConnectionString")]
        public IActionResult GetDbConnectionString()
        {
            string dbConnectionString = ConfigAppsettings.App("Data", "DbConnectionString");
            string dbConnectionString2 = ConfigAppsettings.GetValue("Data:DbConnectionString");

            return Ok($"dbConnectionString :{dbConnectionString},dbConnectionString2:{dbConnectionString2}");
        }
        /// <summary>
        /// ͨ�����캯��ע���ȡǿ����������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOptiongsDb")]
        public IActionResult GetOptiongsDb()
        {
            //_dbOptionsֱ����ӿ�һ��ע��ʹ��
            return Ok($"GetOptiongsDb.dbConnectionString :{_dbOptions.DbConnectionString}");
        }

    }
}
