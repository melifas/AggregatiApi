using System.ComponentModel;

namespace AggregationApi.Enums
{
	/// <summary>
	/// Enum for filter aggregate data.
	/// </summary>
	public enum EnumFilterDataBy
	{
		/// <summary>
		/// The Us filter.
		/// </summary>
		[Description("United States")]
		us = 0,

		/// <summary>
		/// The Italy filter.
		/// </summary>
		[Description("Italy")]
		it = 1

	}
}
