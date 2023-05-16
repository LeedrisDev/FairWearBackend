using System.Reflection;
using GoodOnYouScrapper.API.Business.BrandBusiness;
using GoodOnYouScrapper.API.DataAccess.BrandData;
using GoodOnYouScrapper.API.Utils;
using GoodOnYouScrapper.API.Utils.HttpClientWrapper;
using HtmlAgilityPack;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = AppConstants.ApiName,
        Version = "v1",
        Description = AppConstants.ApiDescription,
        Contact = new OpenApiContact
        {
            Name = "FairWear Group",
            Email = AppConstants.FairWearMail
        }
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<HtmlDocument>();
builder.Services.AddTransient<IBrandData, BrandData>();
builder.Services.AddTransient<IBrandBusiness, BrandBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodOnYou Scrapper API");
    options.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();