using System.Net;
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
        var result = await repository.GetAllAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.ToList().Should().BeEquivalentTo(brands);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsBrandWithMatchingId()
    {
        // Arrange
        var brands = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = "Brand 1",
                Country = "Country 1",
                EnvironmentRating = 1,
                PeopleRating = 1,
                AnimalRating = 1,
                RatingDescription = "Rating 1",
                Categories = new List<string> { "Category 1" },
                Ranges = new List<string> { "Range 1" }
            },
            new BrandEntity
            {
                Id = 2,
                Name = "Brand 2",
                Country = "Country 2",
                EnvironmentRating = 2,
                PeopleRating = 2,
                AnimalRating = 2,
                RatingDescription = "Rating 2",
                Categories = new List<string> { "Category 2" },
                Ranges = new List<string> { "Range 2" }
            }
        };
        _context.Brands.AddRange(brands);
        _context.SaveChanges();

        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should()
            .BeEquivalentTo(brands.First());

    }
    
    [TestMethod]
    public async Task GetByIdAsync_ReturnsNotFoundForNonExistentId()
    {
        // Arrange
        var brands = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = "Brand 1",
                Country = "Country 1",
                EnvironmentRating = 1,
                PeopleRating = 1,
                AnimalRating = 1,
                RatingDescription = "Rating 1",
                Categories = new List<string> { "Category 1" },
                Ranges = new List<string> { "Range 1" }
            },
            new BrandEntity
            {
                Id = 2,
                Name = "Brand 2",
                Country = "Country 2",
                EnvironmentRating = 2,
                PeopleRating = 2,
                AnimalRating = 2,
                RatingDescription = "Rating 2",
                Categories = new List<string> { "Category 2" },
                Ranges = new List<string> { "Range 2" }
            }
        };
        _context.Brands.AddRange(brands);
        _context.SaveChanges();
        
        var repository = new BrandRepository(_context, _mapper);
        // Act
        var result = await repository.GetByIdAsync(3);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }
    
    




}