using AggregationApi.Models.OpenWeather;
using Refit;

namespace AggregationApi.Clients
{
    /// <summary>
    /// Refit open weather api http client
    /// </summary>
    public interface IRefitOpenWeatherClient
    {
        [Get("/data/2.5/weather")]
        Task<WeatherResponseInfo> GetForecast([AliasAs("lat")] string latitude, [AliasAs("lon")] string longitude, [AliasAs("appid")] string apiKey);
    }
}
