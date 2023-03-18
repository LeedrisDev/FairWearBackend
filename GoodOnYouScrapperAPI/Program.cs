using GoodOnYouScrapperAPI.Business.BrandBusiness;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using HtmlAgilityPack;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddHttpClient();
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