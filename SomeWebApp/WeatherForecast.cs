using Swashbuckle.AspNetCore.Annotations;
using System;

namespace SomeWebApp
{
    [SwaggerSchema(Required = new[] { "TemperatureC" })]
    [SwaggerSubType(typeof(WeatherForecastNetanya), DiscriminatorValue = "WeatherForecastNetanya")]
    [SwaggerSubType(typeof(WeatherForecastJerusalem), DiscriminatorValue = "WeatherForecastJerusalem")]
    abstract public class WeatherForecast
    {
        public DateTime Date { get; set; }

        [SwaggerSchema("The WeatherForecast Temperature Celsius", ReadOnly = true)]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }


    public class WeatherForecastNetanya : WeatherForecast
    {
        public int SomeNetanyaProp { get; set; }
    }

    public class WeatherForecastJerusalem : WeatherForecast
    {
        public int SomeJerusalemProp { get; set; }
    }
}
