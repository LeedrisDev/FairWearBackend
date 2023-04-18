using System.Reflection;
using GoodOnYouScrapperAPI.Business.BrandBusiness;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Utils.AppConstants;
using GoodOnYouScrapperAPI.Utils.HttpClientWrapper;
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opt.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();