using JsonSubTypes;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi.Extensions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SomeWebApp
{
    //[AzureResource]
    //[SwaggerSchema(Required = new[] { "TemperatureC" })]
    //[SwaggerSubTypes(typeof(WeatherForecastNetanya), typeof(WeatherForecastJerusalem))]
    //[SwaggerSubType(typeof(WeatherForecastNetanya))]
    //[SwaggerSubType(typeof(WeatherForecastJerusalem))]
    [JsonConverter(typeof(JsonSubtypes), "type")]    
    [JsonSubtypes.KnownSubType(typeof(WeatherForecastNetanya), "type")]
    [JsonSubtypes.KnownSubType(typeof(WeatherForecastJerusalem), "type")]
    //[KnownType(typeof(WeatherForecastJerusalem))]
    abstract public class WeatherForecast
    {
        [Required]
        [JsonProperty(PropertyName = "type")]
        public abstract GeoJsonObjectType Type { get; set; }
        public DateTime Date { get; set; }

        [SwaggerSchema("The WeatherForecast Temperature Celsius", ReadOnly = true)]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }


    [SubTypeOf(typeof(WeatherForecast))]
    /// <summary>
    /// Som description!
    /// </summary>    
    public class WeatherForecastNetanya : WeatherForecast
    {
        public WeatherForecastNetanya()
        {
            Type = GeoJsonObjectType.Netanya;
        }
        [Mutability(Mutability = MutabilityTypes.read)]
        [ReadOnly(true)]
        public int SomeNetanyaProp { get; set; }
        public override GeoJsonObjectType Type { get; set; }
    }

    [SubTypeOf(typeof(WeatherForecast))]
    public class WeatherForecastJerusalem : WeatherForecast
    {
        public WeatherForecastJerusalem()
        {
            Type = GeoJsonObjectType.Jerusalem;
        }
        [ReadOnly(true)]
        public int SomeJerusalemProp { get; set; }
        public override GeoJsonObjectType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
