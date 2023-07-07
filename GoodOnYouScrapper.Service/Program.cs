using GoodOnYouScrapper.Service.Business.BrandBusiness;
using GoodOnYouScrapper.Service.DataAccess.BrandData;
using GoodOnYouScrapper.Service.Services;
using GoodOnYouScrapper.Service.Utils.HttpClientWrapper;
using HtmlAgilityPack;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<HtmlDocument>();
builder.Services.AddTransient<IBrandData, BrandData>();
builder.Services.AddTransient<IBrandBusiness, BrandBusiness>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BrandScrapperService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();