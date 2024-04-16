using AggregationApi.Models.NewsWeather;
using Refit;

namespace AggregationApi.Clients
{
    /// <summary>
    /// Refit news api http client
    /// </summary>
    public interface IRefitNewsApiClient
    {
        [Get("/top-headlines")]
        Task<NewsResponseInfo> GetNewsHeadLines([AliasAs("country")] string country, [AliasAs("apiKey")] string apiKey);
    }
}
