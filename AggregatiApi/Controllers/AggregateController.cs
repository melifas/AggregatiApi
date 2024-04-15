using AggregationApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AggregationApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AggregateController : ControllerBase
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
		/// The <see cref="IConfiguration"/>.
		/// </summary>
		private readonly IConfiguration _config;

		/// <summary>
		/// Initializes a new instance of the <see cref="AggregateController"/> class.
		/// </summary>
		public AggregateController(IRefitOpenWeatherClient refitOpenWeatherClient, IRefitNewsApiClient newsApiClient, IConfiguration config)
		{
			_refitOpenWeatherClient = refitOpenWeatherClient;
			_newsApiClient = newsApiClient;
			_config = config;
		}

		// GET: api/<Aggregate>
		[HttpGet("forecast", Name = nameof(GetOpenWeatherMapForecast))]
		public async Task<IActionResult> GetOpenWeatherMapForecast()
		{
			try
			{

				var apiKey = _config[Constants.WeatherForecastApiKey]??"";

				var weatherResponse = await _refitOpenWeatherClient.GetForecast("44.34", "10.99", apiKey).ConfigureAwait(true);

				return Ok(weatherResponse);
			}
			catch (Refit.ApiException ae)
			{
				Console.WriteLine(ae);
				throw;
			}
		}

		[HttpGet("news", Name = nameof(GetNews))]
		public async Task<IActionResult> GetNews()
		{
			try
			{
				var apiKey = _config[Constants.NewsApiKey] ?? "";

				var newsResponse = await _newsApiClient.GetNewsHeadLines("us", apiKey).ConfigureAwait(true);

				return Ok(newsResponse);
			}
			catch (Refit.ApiException ae)
			{
				Console.WriteLine(ae);
				throw;
			}
		}

		// GET api/<Aggregate>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<Aggregate>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<Aggregate>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<Aggregate>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
