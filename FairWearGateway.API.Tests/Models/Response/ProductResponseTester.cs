using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class ProductResponseTester
{
    [TestMethod]
    public void ProductResponse_Id_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();

        // Act
        productResponse.Id = 1;

        // Assert
        productResponse.Id.Should().Be(1);
    }

    [TestMethod]
    public void ProductResponse_UpcCode_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();

        // Act
        productResponse.UpcCode = "1234567890";

        // Assert
        productResponse.UpcCode.Should().Be("1234567890");
    }

    [TestMethod]
    public void ProductResponse_Name_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();

        // Act
        productResponse.Name = "T-Shirt";

        // Assert
        productResponse.Name.Should().Be("T-Shirt");
    }

    [TestMethod]
    public void ProductResponse_Category_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();

        // Act
        productResponse.Category = "Apparel";

        // Assert
        productResponse.Category.Should().Be("Apparel");
    }

    [TestMethod]
    public void ProductResponse_Ranges_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();
        var ranges = new List<string> { "Range1", "Range2" };

        // Act
        productResponse.Ranges = ranges;

        // Assert
        productResponse.Ranges.Should().BeEquivalentTo(ranges);
    }

    [TestMethod]
    public void ProductResponse_BrandId_ShouldBeSettable()
    {
        // Arrange
        var productResponse = new ProductResponse();

        // Act
        productResponse.BrandId = 1;

        // Assert
        productResponse.BrandId.Should().Be(1);
    }
}