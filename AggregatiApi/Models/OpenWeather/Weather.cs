using System.Text.Json.Serialization;

namespace AggregationApi.Models.OpenWeather
{
	public class Weather
	{
		[JsonPropertyName("description")]
		public string Description { get; set; }
	}
}
