using BrandAndProductDatabase.API.Business.BrandBusiness;
using BrandAndProductDatabase.API.Business.ProductBusiness;
using BrandAndProductDatabase.API.Config;
using BrandAndProductDatabase.API.DataAccess;
using BrandAndProductDatabase.API.DataAccess.BrandData;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.DataAccess.Repositories;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
SwaggerConfiguration.Configure(builder.Services);

// Dependency Injection
builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddDbContext<BrandAndProductDbContext>();

builder.Services.AddTransient<DbContext, BrandAndProductDbContext>();

builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddTransient<IBrandData, BrandData>();

builder.Services.AddTransient<IBrandBusiness, BrandBusiness>();
builder.Services.AddTransient<IProductBusiness, ProductBusiness>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

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