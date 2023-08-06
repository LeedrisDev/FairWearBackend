using System.Net;
using AutoMapper;
using BrandAndProduct.Service.DataAccess;
using BrandAndProduct.Service.DataAccess.Repositories;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProduct.Service.Tests.DataAccess.Repositories;

[TestClass]
public class ProductRepositoryTester
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
    public async Task GetAll_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.ToList().Should().BeEquivalentTo(products);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsProductWithMatchingId()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should()
            .BeEquivalentTo(products.First());
    }


    [TestMethod]
    public async Task GetByIdAsync_ReturnsNotFoundForNonExistentId()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.GetByIdAsync(3);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }

    [TestMethod]
    public async Task AddAsync_AddsProductToDatabase()
    {
        // Arrange
        var productToAdd = new ProductDto()
        {
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.AddAsync(productToAdd);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Id.Should().NotBe(0);

        var productInDb = _context.Products.FirstOrDefault(b => b.Id == result.Object.Id);
        productInDb.Should().NotBeNull();
        productInDb.Should()
            .BeEquivalentTo(productToAdd, options => options.Excluding(x => x.Id));
    }


    [TestMethod]
    public async Task UpdateAsync_UpdatesProductInDatabase()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };

        var productToUpdate = new ProductDto()
        {
            Id = 2,
            Name = "Updated product",
            UpcCode = "987654321",
            Category = "Updated Category",
            Ranges = new List<string> { "Updated Range" },
            BrandId = 2,
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.UpdateAsync(productToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should().BeEquivalentTo(productToUpdate);

        var productInDb = _context.Products
            .FirstOrDefault(b => b.Id == 2);
        productInDb.Should().NotBeNull();
        productInDb.Should().BeEquivalentTo(productToUpdate);
    }

    [TestMethod]
    public async Task UpdateAsync_ReturnsNotFoundForNonExistentId()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
        };

        var productToUpdate = new ProductDto()
        {
            Id = 2,
            Name = "Updated Product",
            UpcCode = "987654321",
            Category = "Updated Category",
            Ranges = new List<string> { "Updated Range" },
            BrandId = 2,
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.UpdateAsync(productToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesProductFromDatabase()
    {
        // Arrange
        var products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().NotBeNull();
        result.Object.Should().BeEquivalentTo(products.First());

        var productInDb = _context.Products.FirstOrDefault(b => b.Id == 1);
        productInDb.Should().BeNull();
    }

    [TestMethod]
    public async Task DeleteAsync_ReturnsNotFoundForNonExistentId()
    {
        // Arrange
        var repository = new ProductRepository(_context, _mapper);

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().NotBeNull();
    }
}