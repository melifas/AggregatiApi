using System.Net.Http.Headers;
using AggregationApi;
using AggregationApi.Interfaces;
using Refit;



var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
	.AddRefitClient<IRefitOpenWeatherClient>(new RefitSettings())
	.ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration[Constants.WeatherForecastBaseUrl]));

builder.Services
	.AddRefitClient<IRefitNewsApiClient>(new RefitSettings())
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri(configuration[Constants.NewsBaseUrl]);
		c.DefaultRequestHeaders.Add("User-Agent", "Refit");
	});

builder.Services
	.AddRefitClient<IRefitRandomUsersClient>(new RefitSettings())
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri(configuration[Constants.RandomUserBaseUrl]);
		c.DefaultRequestHeaders.Add("User-Agent", "Refit");
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
