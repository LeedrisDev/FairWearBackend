using AutoMapper;
using BrandAndProductDatabase.API.DataAccess;
using BrandAndProductDatabase.API.DataAccess.Repositories;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddDbContext<BrandAndProductDbContext>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

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

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});

var mapper = config.CreateMapper();
var repo = new BrandRepository(new BrandAndProductDbContext(), mapper);

await repo.AddAsync(new BrandDto()
{
    Name = "Levis",
    Country = "United States",
    EnvironmentRating = 1,
    PeopleRating = 1,
    AnimalRating = 1,
    RatingDescription = "",
    Categories = new List<String>()
    {
        "Jeans", "T-Shirts"
    },
    Ranges = new List<String>()
});


var res = await repo.GetAllAsync();

foreach (var brand in res.Object)
{
    Console.WriteLine("Brand: " + brand.Name + " Country" + brand.Country + "EnvironmentRating " + brand.EnvironmentRating + " PeopleRating" +
                      brand.PeopleRating + " AnimalRating" + brand.AnimalRating);
}


app.Run();