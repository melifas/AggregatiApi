using AggregationApi.Clients;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Net;
using System.Threading.Tasks;
using AggregationApi.Enums;
using AggregationApi.Interfaces;
using AggregationApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AggregationApi.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AggregateController : ControllerBase
	{
		/// <summary>
		/// The <see cref="IAggregateDataService"/>.
		/// </summary>
		private readonly IAggregateDataService _aggregateDataService;

		/// <summary>
		/// Initializes a new instance of the <see cref="AggregateController"/> class.
		/// </summary>
		public AggregateController(
			IAggregateDataService aggregateDataService)
		{
			_aggregateDataService = aggregateDataService;
		}


		/// <summary>
		/// Gets the aggregate data.
		/// </summary>
		/// <param name="filterBy"> filter data by Country (optional, defaults to United States). </param>
		/// <param name="sortAsc"> True to sort ascending by Article Title otherwise descending (optional, defaults to true). </param>
		[HttpGet("aggregate", Name = nameof(GetAggregateApis))]
		[SwaggerResponse(StatusCodes.Status200OK, "Aggregate data fetched successfully!", typeof(AggregatedDataResultsInfo))]
		[SwaggerResponse(StatusCodes.Status404NotFound)]
		[SwaggerResponse(StatusCodes.Status400BadRequest)]
		[SwaggerResponse(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAggregateApis(
			[EnumDataType(typeof(EnumFilterDataBy))] EnumFilterDataBy filterBy = EnumFilterDataBy.us,
			bool sortAsc = true)
		{
			try
			{

				var aggregateData = await _aggregateDataService.GetAggregateData(filterBy, sortAsc);

				return Ok(aggregateData);
			}
			catch (Refit.ApiException ae)
			{
				switch (ae.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return NotFound(ae.Message);
					case HttpStatusCode.BadRequest:
						return BadRequest(ae.Message);
					default:
						return StatusCode(StatusCodes.Status500InternalServerError, Constants.GenericErrorMessage);

				}
			}
			catch (Exception e)
			{
				//logging here....
				return StatusCode(StatusCodes.Status500InternalServerError, Constants.GenericErrorMessage);
			}
		}
	}
}
