using AggregationApi.Models.OpenWeather;
using AggregationApi.Models.Users;
using Refit;

namespace AggregationApi.Interfaces
{
	/// <summary>
	/// Refit random users api http client
	/// </summary>
	public interface IRefitRandomUsersClient
	{
		[Get("/posts")]
		Task<List<UsersResponseInfo>> GetRandomUsers();
	}
}
