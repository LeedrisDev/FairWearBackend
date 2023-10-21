using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using FluentAssertions;

namespace BrandAndProduct.Service.Tests.Models.Dto;

[TestClass]
public class BrandDtoTester
{
    [TestMethod]
    public void BrandDto_Id_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            Id = 42
        };

        // Assert
        brand.Id.Should().Be(42);
    }

    [TestMethod]
    public void BrandDto_Name_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            Name = "Test Brand"
        };

        // Assert
        brand.Name.Should().Be("Test Brand");
    }

    [TestMethod]
    public void BrandDto_Country_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            Country = "Test Country"
        };

        // Assert
        brand.Country.Should().Be("Test Country");
    }

    [TestMethod]
    public void BrandDto_EnvironmentRating_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            EnvironmentRating = 3
        };

        // Assert
        brand.EnvironmentRating.Should().Be(3);
    }

    [TestMethod]
    public void BrandDto_PeopleRating_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            PeopleRating = 4
        };

        // Assert
        brand.PeopleRating.Should().Be(4);
    }

    [TestMethod]
    public void BrandDto_AnimalRating_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            AnimalRating = 5
        };

        // Assert
        brand.AnimalRating.Should().Be(5);
    }

    [TestMethod]
    public void BrandDto_RatingDescription_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto
        {
            // Act
            RatingDescription = "Test Rating Description"
        };

        // Assert
        brand.RatingDescription.Should().Be("Test Rating Description");
    }

    [TestMethod]
    public void BrandDto_Categories_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto();
        var categories = new List<string> { "Category 1", "Category 2" };

        // Act
        brand.Categories = categories;

        // Assert
        brand.Categories.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(categories);
    }

    [TestMethod]
    public void BrandDto_Ranges_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto();
        var ranges = new List<string> { "Range 1", "Range 2" };

        // Act
        brand.Ranges = ranges;

        // Assert
        brand.Ranges.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(ranges);
    }

    [TestMethod]
    public void BrandDto_Products_ShouldBeSettable()
    {
        // Arrange
        var brand = new BrandDto();
        var products = new List<ProductEntity>
        {
            new ProductEntity { Id = 1, Name = "Product 1" },
            new ProductEntity { Id = 2, Name = "Product 2" }
        };

        // Act
        brand.Products = products;

        // Assert
        brand.Products.Should().NotBeNull().And.BeAssignableTo<IEnumerable<ProductEntity>>().And
            .BeEquivalentTo(products);
    }
}