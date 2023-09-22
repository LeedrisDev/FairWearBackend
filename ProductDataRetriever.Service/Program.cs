using ProductDataRetriever.Service.Business.ProductBusiness;
using ProductDataRetriever.Service.DataAccess.ProductData;
using ProductDataRetriever.Service.Services;
using ProductDataRetriever.Service.Utils.HttpClientWrapper;
using HtmlAgilityPack;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<HtmlDocument>();
builder.Services.AddTransient<IProductData, ProductData>();
builder.Services.AddTransient<IProductBusiness, ProductBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductScrapperService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();