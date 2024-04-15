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
		/// Initializes a new instance of the <see cref="AggregateController"/> class.
		/// </summary>
		public AggregateController(IRefitOpenWeatherClient refitOpenWeatherClient, IRefitNewsApiClient newsApiClient)
		{
			_refitOpenWeatherClient = refitOpenWeatherClient;
			_newsApiClient = newsApiClient;
		}

		// GET: api/<Aggregate>
		[HttpGet("forecast", Name = nameof(GetOpenWeatherMapForecast))]
		public async Task<IActionResult> GetOpenWeatherMapForecast()
		{
			try
			{
				var t = await _refitOpenWeatherClient.GetForecast("44.34", "10.99", "aa7bdb936df9a0a89004d7a6784c1794")
					.ConfigureAwait(true);
				return Ok(t);
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
				var t = await _newsApiClient.GetNewsHeadLines("us", "e6f98768db344326914f9df67b762de9")
					.ConfigureAwait(true);
				return Ok(t);
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
