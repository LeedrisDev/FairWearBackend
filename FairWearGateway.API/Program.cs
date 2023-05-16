using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Business.ProductBusiness;
using FairWearGateway.API.Config;
using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.DataAccess.ProductData;
using FairWearGateway.API.Utils.HttpClientWrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();

builder.Services.AddTransient<IProductData, ProductData>();
builder.Services.AddTransient<IBrandData, BrandData>();

builder.Services.AddTransient<IProductBusiness, ProductBusiness>();
builder.Services.AddTransient<IBrandBusiness, BrandBusiness>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
SwaggerConfiguration.Configure(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
// }

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FairWear Gateway API");
    options.RoutePrefix = "api/swagger";
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();