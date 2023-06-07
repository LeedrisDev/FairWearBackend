using BrandAndProductDatabase.API.Models.Response;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Response;

[TestClass]
public class ProductScoreResponseTester
{
    [TestMethod]
    public void ProductScoreResponse_SetAndGetMoral_ShouldSetAndReturnMoral()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();
        var moral = 8;

        // Act
        productScoreResponse.Moral = moral;
        var result = productScoreResponse.Moral;

        // Assert
        result.Should().Be(moral);
    }

    [TestMethod]
    public void ProductScoreResponse_SetAndGetAnimal_ShouldSetAndReturnAnimal()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();
        var animal = 5;

        // Act
        productScoreResponse.Animal = animal;
        var result = productScoreResponse.Animal;

        // Assert
        result.Should().Be(animal);
    }

    [TestMethod]
    public void ProductScoreResponse_SetAndGetEnvironmental_ShouldSetAndReturnEnvironmental()
    {
        // Arrange
        var productScoreResponse = new ProductScoreResponse();
        var environmental = 7;

        // Act
        productScoreResponse.Environmental = environmental;
        var result = productScoreResponse.Environmental;

        // Assert
        result.Should().Be(environmental);
    }   
}