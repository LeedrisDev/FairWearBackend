using BrandAndProductDatabase.API.Models.Entity;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Entity;

[TestClass]
public class ProductEntityTester
{
    [TestMethod]
    public void ProductEntity_Id_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();

        // Act
        product.Id = 42;

        // Assert
        product.Id.Should().Be(42);
    }

    [TestMethod]
    public void ProductEntity_UpcCode_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();

        // Act
        product.UpcCode = "1234567890";

        // Assert
        product.UpcCode.Should().Be("1234567890");
    }

    [TestMethod]
    public void ProductEntity_Name_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();

        // Act
        product.Name = "Test Product";

        // Assert
        product.Name.Should().Be("Test Product");
    }

    [TestMethod]
    public void ProductEntity_Category_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();

        // Act
        product.Category = "Test Category";

        // Assert
        product.Category.Should().Be("Test Category");
    }

    [TestMethod]
    public void ProductEntity_Ranges_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();
        var ranges = new List<string> { "Range 1", "Range 2" };

        // Act
        product.Ranges = ranges;

        // Assert
        product.Ranges.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(ranges);
    }

    [TestMethod]
    public void ProductEntity_BrandId_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();

        // Act
        product.BrandId = 42;

        // Assert
        product.BrandId.Should().Be(42);
    }

    [TestMethod]
    public void ProductEntity_BrandEntity_ShouldBeSettable()
    {
        // Arrange
        var product = new ProductEntity();
        var brand = new BrandEntity { Id = 1, Name = "Test Brand" };

        // Act
        product.BrandEntity = brand;

        // Assert
        product.BrandEntity.Should().NotBeNull().And.BeOfType<BrandEntity>().And.BeEquivalentTo(brand);
    }
}