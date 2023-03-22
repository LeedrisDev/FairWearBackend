using GoodOnYouScrapperAPI.Business.BrandBusiness;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Utils.AppConstants;
using GoodOnYouScrapperAPI.Utils.HttpClientWrapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(AppConstants.APIMajorVersion, AppConstants.APIMinorVersion);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<HtmlDocument>();
builder.Services.AddTransient<IBrandData, BrandData>();
builder.Services.AddTransient<IBrandBusiness, BrandBusiness>();

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