using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;
using AggregationApi;
using AggregationApi.Clients;
using AggregationApi.Interfaces;
using AggregationApi.Services;
using Polly;
using Refit;



var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	// Set the comments path for the Swagger JSON and UI.
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

	c.IncludeXmlComments(xmlPath);
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


builder.Services.AddScoped<IAggregateDataService, AggregateDataService>();


IAsyncPolicy<HttpResponseMessage> httpWaitAndRetryPolicy =
	Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
		.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt),
			(result, span, retryCount, ctx) => Console.WriteLine($"Retrying({retryCount})...")
		);

IAsyncPolicy<HttpResponseMessage> fallbackPolicy =
	Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
		.FallbackAsync(FallbackAction, OnFallbackAsync);


Task OnFallbackAsync(DelegateResult<HttpResponseMessage> response, Context context)
{

	//This is a good place to do some logging
	return Task.CompletedTask;
}

Task<HttpResponseMessage> FallbackAction(DelegateResult<HttpResponseMessage> responseToFailedRequest, Context context, CancellationToken cancellationToken)
{

	HttpResponseMessage httpResponseMessage = new HttpResponseMessage(responseToFailedRequest.Result.StatusCode)
	{
		Content = new StringContent($"The fallback executed, the original error was {responseToFailedRequest.Result.ReasonPhrase}")
	};
	return Task.FromResult(httpResponseMessage);
}

IAsyncPolicy<HttpResponseMessage> wrapOfRetryAndFallback = Policy.WrapAsync(fallbackPolicy, httpWaitAndRetryPolicy);


builder.Services
	.AddRefitClient<IRefitOpenWeatherClient>(new RefitSettings())
	.ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration[Constants.WeatherForecastBaseUrl])).AddPolicyHandler(wrapOfRetryAndFallback);

builder.Services
	.AddRefitClient<IRefitNewsApiClient>(new RefitSettings())
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri(configuration[Constants.NewsBaseUrl]);
		c.DefaultRequestHeaders.Add("User-Agent", "Refit");
	}).AddPolicyHandler(wrapOfRetryAndFallback);

builder.Services
	.AddRefitClient<IRefitRandomUsersClient>(new RefitSettings())
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri(configuration[Constants.RandomUserBaseUrl]);
		c.DefaultRequestHeaders.Add("User-Agent", "Refit");
	}).AddPolicyHandler(wrapOfRetryAndFallback);





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
