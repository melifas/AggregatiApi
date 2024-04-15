using System.Text.Json.Serialization;

namespace AggregationApi.Models.Users
{
	public class UsersResponseInfo
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("body")]
		public string Body { get; set; }
	}
}
