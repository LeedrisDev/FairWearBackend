using HtmlAgilityPack;
using FairWearProductDataRetriever.API.Business.ProductBusiness;
using FairWearProductDataRetriever.API.Config;
using FairWearProductDataRetriever.API.DataAccess.ProductData;
using FairWearProductDataRetriever.API.Utils.HttpClientWrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

SwaggerConfiguration.Configure(builder.Services);

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<HtmlDocument>();
builder.Services.AddTransient<IProductData, ProductData>();
builder.Services.AddTransient<IProductBusiness, ProductBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();