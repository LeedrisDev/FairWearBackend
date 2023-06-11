using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Dto;

[TestClass]
public class ProductScoreDtoTester
{
    [TestMethod]
    public void ProductScoreDto_SetAndGetMoral_ShouldSetAndReturnMoral()
    {
        // Arrange
        var productScoreDto = new ProductScoreDto();
        var moral = 8;

        // Act
        productScoreDto.Moral = moral;
        var result = productScoreDto.Moral;

        // Assert
        result.Should().Be(moral);
    }

    [TestMethod]
    public void ProductScoreDto_SetAndGetAnimal_ShouldSetAndReturnAnimal()
    {
        // Arrange
        var productScoreDto = new ProductScoreDto();
        var animal = 5;

        // Act
        productScoreDto.Animal = animal;
        var result = productScoreDto.Animal;

        // Assert
        result.Should().Be(animal);
    }

    [TestMethod]
    public void ProductScoreDto_SetAndGetEnvironmental_ShouldSetAndReturnEnvironmental()
    {
        // Arrange
        var productScoreDto = new ProductScoreDto();
        var environmental = 7;

        // Act
        productScoreDto.Environmental = environmental;
        var result = productScoreDto.Environmental;

        // Assert
        result.Should().Be(environmental);
    }   
}