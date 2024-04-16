using AggregationApi.Clients;
using AggregationApi.Enums;
using AggregationApi.Interfaces;
using AggregationApi.Models;
using AggregationApi.Models.Users;
using System.Linq;
using AggregationApi.Models.NewsWeather;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AggregationApi.Services
{
	/// <summary>
	/// Implements the <see cref="IAggregateDataService"/> <see langword="interface"/>
	/// </summary>
	public class AggregateDataService : IAggregateDataService
	{
		/// <summary>
		/// The <see cref="IRefitOpenWeatherClient"/>.
		/// </summary>
		private readonly IRefitOpenWeatherClient _refitOpenWeatherClient;

		/// <summary>
		/// The <see cref="IRefitNewsApiClient"/>.
		/// </summary>
		private readonly IRefitNewsApiClient _newsApiClient;

		/// <summary>
		/// The <see cref="IRefitRandomUsersClient"/>.
		/// </summary>
		private readonly IRefitRandomUsersClient _usersClient;

		/// <summary>
		/// The <see cref="IDistributedCache"/>.
		/// </summary>
		private readonly IDistributedCache _cache;

		/// <summary>
		/// The <see cref="IConfiguration"/>.
		/// </summary>
		private readonly IConfiguration _config;

		/// <summary>
		/// Initializes a new instance of the <see cref="AggregateDataService"/> class.
		/// </summary>
		/// <param name="refitOpenWeatherClient"> The <see cref="IRefitOpenWeatherClient"/>. </param>
		/// <param name="newsApiClient"> The <see cref="IRefitNewsApiClient"/>. </param>
		/// <param name="usersClient"> The <see cref="IRefitRandomUsersClient"/>. </param>
		/// <param name="cache"> The <see cref="IDistributedCache"/>. </param>
		/// <param name="config"></param>
		public AggregateDataService(
			IRefitOpenWeatherClient refitOpenWeatherClient,
			IRefitNewsApiClient newsApiClient,
			IRefitRandomUsersClient usersClient,
			IDistributedCache cache,
			IConfiguration config)
		{
			_refitOpenWeatherClient = refitOpenWeatherClient;
			_newsApiClient = newsApiClient;
			_usersClient = usersClient;
			_cache = cache;
			_config = config;
		}

		/// <inheritdoc/>
		public async Task<AggregatedDataResultsInfo> GetAggregateData(EnumFilterDataBy filterBy, bool sortAsc)
		{

			var cacheKey = $"AggregateData-{filterBy}-{sortAsc}";

			var cachedResultsJson = await _cache.GetStringAsync(cacheKey);

			if (cachedResultsJson != null)
			{
				// If the data is in the cache, deserialize it and return it
				var cachedResults = JsonSerializer.Deserialize<AggregatedDataResultsInfo>(cachedResultsJson);

				return cachedResults;
			}


			var weatherResponse = _refitOpenWeatherClient.GetForecast("44.34", "10.99",
				_config[Constants.WeatherForecastApiKey] ?? "");

			var newsResponse = _newsApiClient.GetNewsHeadLines(filterBy.ToString(), _config[Constants.NewsApiKey] ?? "");

			var randomResponse = _usersClient.GetRandomUsers();

			await Task.WhenAll(weatherResponse, newsResponse, randomResponse);

			List<Article> ordered = !sortAsc ? newsResponse.Result.Articles.OrderByDescending(c=>c.Title).ToList() : newsResponse.Result.Articles.OrderBy(c => c.Title).ToList();

			var results = new AggregatedDataResultsInfo
			{
				Temp = weatherResponse.Result.Main.Temp,
				Articles = ordered,
				UsersResponseInfos = randomResponse.Result.Select(c=>new UsersResponseInfo{Body = c.Body, Title = c.Title}).ToList()
			};

			var resultsJson = JsonSerializer.Serialize(results);
			await _cache.SetStringAsync(cacheKey, resultsJson);

			return results;

		}
	}
}
