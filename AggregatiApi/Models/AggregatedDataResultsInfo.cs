using AggregationApi.Models.NewsWeather;
using AggregationApi.Models.Users;

namespace AggregationApi.Models
{
	public class AggregatedDataResultsInfo
	{
		public decimal Temp { get; set; }
		public List<Article> Articles { get; set; }

		public List<UsersResponseInfo> UsersResponseInfos { get; set; }

	}
}
