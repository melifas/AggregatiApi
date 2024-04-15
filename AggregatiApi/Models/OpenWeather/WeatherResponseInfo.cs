using System.Text.Json.Serialization;

namespace AggregationApi.Models.OpenWeather
{
	/// <summary>
	/// The weather response info model.
	/// </summary>
	public class WeatherResponseInfo
    {

        [JsonPropertyName("base")]
        public string BaseStations { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("main")]
        public MainData Main { get; set; }

    }

}
