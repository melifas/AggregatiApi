using AggregationApi.Clients;
using AggregationApi.Enums;
using AggregationApi.Models;
using AggregationApi.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace TestProject
{
	public class UnitTest1
	{
		[Fact]
		public async Task GetAggregateData_ReturnsCachedData_WhenAvailable()
		{
			// Arrange
			var weatherClientMock = new Mock<IRefitOpenWeatherClient>();
			var newsClientMock = new Mock<IRefitNewsApiClient>();
			var usersClientMock = new Mock<IRefitRandomUsersClient>();
			var cacheMock = new Mock<IDistributedCache>();
			var configMock = new Mock<IConfiguration>();

			var expectedData = new AggregatedDataResultsInfo();
			var cacheKey = $"AggregateData-{EnumFilterDataBy.us}-{true}";
			var cachedDataJson = JsonSerializer.Serialize(expectedData);

			cacheMock.Setup(c => c.GetStringAsync(cacheKey, default))
				.ReturnsAsync(cachedDataJson);

			var service = new AggregateDataService(
				weatherClientMock.Object,
				newsClientMock.Object,
				usersClientMock.Object,
				cacheMock.Object,
				configMock.Object);

			// Act
			var result = await service.GetAggregateData(EnumFilterDataBy.us, true);

			// Assert
			Assert.Equal(expectedData, result);
		}
	}
}