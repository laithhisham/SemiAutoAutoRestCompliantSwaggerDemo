using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeWebApp.Controllers
{
    [SwaggerTag("Create, read, update and delete WeatherForecast")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IEnumerable<WeatherForecast> _db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var rng = new Random();
            _db = Enumerable.Range(1, 5).Select(index => new WeatherForecastNetanya
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
            _db.ToList().AddRange(

                Enumerable.Range(1, 5).Select(index => new WeatherForecastJerusalem
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)],
                    SomeJerusalemProp = 33
                }));
        }

        [SwaggerOperation(
            Summary = "Get a weather forecast",
            Description = "fetches from Db",
            OperationId = "Get",
            Tags = new[] { "forecast" })]
        [SwaggerResponse(200, "The WeatherForecast was fetched", typeof(IEnumerable<WeatherForecast>))]
        [SwaggerResponse(400, "invalid request")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromQuery, SwaggerParameter("WeatherForecast Id", Required = true)] string id)
        {
            return _db;
        }
    }
}
