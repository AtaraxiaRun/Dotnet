using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.RateLimiting;
using System.Threading;
using System.Threading.RateLimiting;

namespace AspNetCore.Api.Pollys.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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

        [HttpGet("Pollys")]
        public void Pollys()
        {
            //每秒钟执行不能超过1次
            Console.WriteLine("2222");

            Policy.RateLimit(1, TimeSpan.FromSeconds(1));
            Console.WriteLine(  "111");

        }
    }
}
