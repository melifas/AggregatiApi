using AggregationApi.Enums;
using AggregationApi.Models;

namespace AggregationApi.Interfaces
{
	public interface IAggregateDataService
	{
		Task<AggregatedDataResultsInfo> GetAggregateData(EnumFilterDataBy sortBy, bool sortAsc);
	}
}
