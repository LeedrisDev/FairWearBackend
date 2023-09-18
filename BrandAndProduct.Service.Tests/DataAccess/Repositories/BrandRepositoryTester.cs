using System.Net;
using AutoMapper;
using BrandAndProduct.Service.DataAccess;
using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.Repositories;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProduct.Service.Tests.DataAccess.Repositories;

[TestClass]
public class BrandRepositoryTester
{
    private BrandAndProductDbContextInMemoryDatabase _context;
    private IMapper _mapper;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<BrandAndProductDbContext>()
            .UseInMemoryDatabase(databaseName: "test_database")
            .Options;

        _context = new BrandAndProductDbContextInMemoryDatabase(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });

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
        var filterDict = new Dictionary<string, string>();
        var genericFilter = new GenericFilterFactory<IFilter>();
        var filter = genericFilter.CreateFilter(filterDict);
        var result = await repository.GetAllAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.ToList().Should().BeEquivalentTo(brands);
    }

    [TestMethod]
    public async Task GetFilter_ReturnsFilteredBrands()
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
            },
            new BrandEntity
            {
                Id = 3,
                Name = "Brand 3",
                Country = "USA",
                EnvironmentRating = 5,
                PeopleRating = 4,
                AnimalRating = 3,
                RatingDescription = "Description 1",
                Categories = new List<string> { "Category 1", "Category 2" },
                Ranges = new List<string> { "Range 1", "Range 2" }
            },
        };

        var expected = new List<BrandEntity>
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
                Id = 3,
                Name = "Brand 3",
                Country = "USA",
                EnvironmentRating = 5,
                PeopleRating = 4,
                AnimalRating = 3,
                RatingDescription = "Description 1",
                Categories = new List<string> { "Category 1", "Category 2" },
                Ranges = new List<string> { "Range 1", "Range 2" }
            },
        };
        _context.Brands.AddRange(brands);
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);

        // Act
        var filterDict = new Dictionary<string, string>()
        {
            { "Country", "USA" }
        };

        var genericFilter = new GenericFilterFactory<IFilter>();
        var filter = genericFilter.CreateFilter(filterDict);
        var result = await repository.GetAllAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.ToList().Should().BeEquivalentTo(expected);
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
        await _context.SaveChangesAsync();

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
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);
        // Act
        var result = await repository.GetByIdAsync(3);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }

    [TestMethod]
    public async Task AddAsync_AddsBrandToDatabase()
    {
        // Arrange
        var brandToAdd = new BrandDto
        {
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };
        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.AddAsync(brandToAdd);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Id.Should().NotBe(0);

        var brandInDb = _context.Brands.FirstOrDefault(b => b.Id == result.Object.Id);
        brandInDb.Should().NotBeNull();
        brandInDb.Should()
            .BeEquivalentTo(brandToAdd, options => options.Excluding(x => x.Id));
    }

    [TestMethod]
    public async Task UpdateAsync_UpdatesBrandInDatabase()
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

        var brandToUpdate = new BrandDto
        {
            Id = 2,
            Name = "Updated Brand",
            Country = "Updated Country",
            EnvironmentRating = 3,
            PeopleRating = 3,
            AnimalRating = 3,
            RatingDescription = "Updated Rating",
            Categories = new List<string> { "Updated Category" },
            Ranges = new List<string> { "Updated Range" }
        };

        _context.Brands.AddRange(brands);
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.UpdateAsync(brandToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should().BeEquivalentTo(brandToUpdate);

        var brandInDb = _context.Brands
            .FirstOrDefault(b => b.Id == 2);
        brandInDb.Should().NotBeNull();
        brandInDb.Should().BeEquivalentTo(brandToUpdate);
    }

    [TestMethod]
    public async Task UpdateAsync_ReturnsNotFoundForNonExistentId()
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
        };

        var brandToUpdate = new BrandDto
        {
            Id = 2,
            Name = "Updated Brand",
            Country = "Updated Country",
            EnvironmentRating = 3,
            PeopleRating = 3,
            AnimalRating = 3,
            RatingDescription = "Updated Rating",
            Categories = new List<string> { "Updated Category" },
            Ranges = new List<string> { "Updated Range" }
        };

        _context.Brands.AddRange(brands);
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.UpdateAsync(brandToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesBrandFromDatabase()
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
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should().BeEquivalentTo(brands.First());

        var brandInDb = _context.Brands.FirstOrDefault(b => b.Id == 1);
        brandInDb.Should().BeNull();
    }

    [TestMethod]
    public async Task DeleteAsync_ReturnsNotFoundForNonExistentId()
    {
        // Arrange
        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandDto()
    {
        // Arrange
        var brandName = "Nike";
        var brands = new List<BrandEntity>
        {
            new BrandEntity
            {
                Id = 1,
                Name = brandName,
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
        await _context.SaveChangesAsync();

        var repository = new BrandRepository(_context, _mapper);

        var expectedObject = _mapper.Map<BrandDto>(brands.First());
        // Act
        var result = await repository.GetBrandByNameAsync(brandName);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should().BeEquivalentTo(expectedObject);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsNotFound()
    {
        // Arrange
        var brandName = "Nike";
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
        await _context.SaveChangesAsync();
        var repository = new BrandRepository(_context, _mapper);

        // Act
        var result = await repository.GetBrandByNameAsync(brandName);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}