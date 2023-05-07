using AutoMapper;
using BrandAndProductDatabase.API.DataAccess;
using BrandAndProductDatabase.API.DataAccess.Repositories;
using BrandAndProductDatabase.API.Models.Entity;
using BrandAndProductDatabase.API.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.Tests.DataAccess;

[TestClass]
public class BrandRespositoryTester
{
    private BrandAndProductDbContextTest _context;
    private IMapper _mapper;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<BrandAndProductDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;

        _context = new BrandAndProductDbContextTest(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfiles>();
        });
        
        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task GetAll_ReturnsAllBrands()
    {
        // Arrange
        var brands = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = "Brand 1",
                Country = "USA",
                EnvironmentRating = 5,
                PeopleRating = 4,
                AnimalRating = 3,
                RatingDescription = "Description 1",
                Categories = new List<string> { "Category 1", "Category 2" },
                Ranges = new List<string> { "Range 1", "Range 2" }
            },
            new BrandEntity
            {
                Id = 2,
                Name = "Brand 2",
                Country = "Canada",
                EnvironmentRating = 4,
                PeopleRating = 3,
                AnimalRating = 2,
                RatingDescription = "Description 2",
                Categories = new List<string> { "Category 3", "Category 4" },
                Ranges = new List<string> { "Range 3", "Range 4" }
            }
        };
        _context.Brands.AddRange(brands);
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);
        
        // Act
        var response = await repository.GetAllAsync();
        var result = response.Object.ToList();
        
        // Assert
        result.Should().BeEquivalentTo(brands);
    }
}