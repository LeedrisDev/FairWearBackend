using BrandAndProductDatabase.API.Models.Response;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Response;

[TestClass]
public class ProductResponseTester
{
    [TestMethod]
    public void Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedId = 1;

        // Act
        productResponse.Id = expectedId;

        // Assert
        productResponse.Id.Should().Be(expectedId);
    }

    [TestMethod]
    public void UpcCode_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedUpcCode = "1234567890";

        // Act
        productResponse.UpcCode = expectedUpcCode;

        // Assert
        productResponse.UpcCode.Should().Be(expectedUpcCode);
    }

    [TestMethod]
    public void Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedName = "Test Product";

        // Act
        productResponse.Name = expectedName;

        // Assert
        productResponse.Name.Should().Be(expectedName);
    }

    [TestMethod]
    public void Category_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedCategory = "TestCategory";

        // Act
        productResponse.Category = expectedCategory;

        // Assert
        productResponse.Category.Should().Be(expectedCategory);
    }

    [TestMethod]
    public void Ranges_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedRanges = new List<string> { "Range1", "Range2" };

        // Act
        productResponse.Ranges = expectedRanges;

        // Assert
        productResponse.Ranges.Should().BeEquivalentTo(expectedRanges);
    }

    [TestMethod]
    public void BrandId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var expectedBrandId = 1;

        // Act
        productResponse.BrandId = expectedBrandId;

        // Assert
        productResponse.BrandId.Should().Be(expectedBrandId);
    }
}