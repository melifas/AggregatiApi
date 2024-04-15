using System.Text.Json.Serialization;

namespace AggregationApi.Models.OpenWeather
{
	public class MainData
	{
		[JsonPropertyName("temp")]
		public decimal Temp { get; set; }
	}
}
