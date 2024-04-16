using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using AggregationApi.Services;
using AggregationApi.Clients;
using AggregationApi.Models;
using AggregationApi.Enums;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

public class AggregateDataServiceTests
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
        var cacheKey = $"AggregateData-{EnumFilterDataBy.News}-{true}";
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
        var result = await service.GetAggregateData(EnumFilterDataBy.News, true);

        // Assert
        Assert.Equal(expectedData, result);
    }
}
