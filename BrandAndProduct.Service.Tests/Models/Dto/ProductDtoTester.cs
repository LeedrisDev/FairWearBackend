using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Models.Entity;
using FluentAssertions;

namespace BrandAndProduct.Service.Tests.Models.Dto;

[TestClass]
public class ProductDtoTester
{
    [TestMethod]
    public void ProductDto_Id_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto
        {
            // Act
            Id = 42
        };

        // Assert
        product.Id.Should().Be(42);
    }

    [TestMethod]
    public void ProductDto_UpcCode_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto
        {
            // Act
            UpcCode = "123456789012"
        };

        // Assert
        product.UpcCode.Should().Be("123456789012");
    }

    [TestMethod]
    public void ProductDto_Name_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto
        {
            // Act
            Name = "Test Product"
        };

        // Assert
        product.Name.Should().Be("Test Product");
    }

    [TestMethod]
    public void ProductDto_Category_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto
        {
            // Act
            Category = "Test Category"
        };

        // Assert
        product.Category.Should().Be("Test Category");
    }

    [TestMethod]
    public void ProductDto_Ranges_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto();
        var ranges = new List<string> { "Range 1", "Range 2" };

        // Act
        product.Ranges = ranges;

        // Assert
        product.Ranges.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(ranges);
    }

    [TestMethod]
    public void ProductDto_BrandId_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto
        {
            // Act
            BrandId = 42
        };

        // Assert
        product.BrandId.Should().Be(42);
    }

    [TestMethod]
    public void ProductDto_BrandEntity_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductDto();
        var brandEntity = new BrandEntity { Id = 1, Name = "Test Brand" };

        // Act
        product.BrandEntity = brandEntity;

        // Assert
        product.BrandEntity.Should().NotBeNull().And.BeAssignableTo<BrandEntity>().And.BeEquivalentTo(brandEntity);
    }
}