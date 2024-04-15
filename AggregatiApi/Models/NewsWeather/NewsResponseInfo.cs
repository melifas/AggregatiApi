using System.Text.Json.Serialization;

namespace AggregationApi.Models.NewsWeather
{
	/// <summary>
	/// The news response info model.
	/// </summary>
	public class NewsResponseInfo
	{
		[JsonPropertyName("articles")]
		public List<Article> Articles { get; set; }
	}
}
