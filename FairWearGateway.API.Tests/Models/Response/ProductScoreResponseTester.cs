using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class ProductScoreResponseTester
{
    [TestMethod]
    public void ProductScoreResponse_Moral_ShouldBeSettable()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();

        // Act
        productScoreResponse.Moral = 8;

        // Assert
        productScoreResponse.Moral.Should().Be(8);
    }

    [TestMethod]
    public void ProductScoreResponse_Animal_ShouldBeSettable()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();

        // Act
        productScoreResponse.Animal = 6;

        // Assert
        productScoreResponse.Animal.Should().Be(6);
    }

    [TestMethod]
    public void ProductScoreResponse_Environmental_ShouldBeSettable()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();

        // Act
        productScoreResponse.Environmental = 7;

        // Assert
        productScoreResponse.Environmental.Should().Be(7);
    }
}