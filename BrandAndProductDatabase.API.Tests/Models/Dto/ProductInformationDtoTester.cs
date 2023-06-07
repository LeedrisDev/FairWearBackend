using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Dto;

[TestClass]
public class ProductInformationDtoTester
{
    [TestMethod]
    public void ProductInformationDto_SetAndGetName_ShouldSetAndReturnName()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var name = "Product Name";

        // Act
        productInformationDto.Name = name;
        var result = productInformationDto.Name;

        // Assert
        result.Should().Be(name);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetCountry_ShouldSetAndReturnCountry()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var country = "Country Name";

        // Act
        productInformationDto.Country = country;
        var result = productInformationDto.Country;

        // Assert
        result.Should().Be(country);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetImage_ShouldSetAndReturnImage()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var imageUrl = "https://example.com/product-image.jpg";

        // Act
        productInformationDto.Image = imageUrl;
        var result = productInformationDto.Image;

        // Assert
        result.Should().Be(imageUrl);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetGlobalScore_ShouldSetAndReturnGlobalScore()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var globalScore = 9;

        // Act
        productInformationDto.GlobalScore = globalScore;
        var result = productInformationDto.GlobalScore;

        // Assert
        result.Should().Be(globalScore);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetScores_ShouldSetAndReturnScores()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var scores = new ProductScoreDto { Moral = 8, Animal = 5, Environmental = 7 };

        // Act
        productInformationDto.Scores = scores;
        var result = productInformationDto.Scores;

        // Assert
        result.Should().BeEquivalentTo(scores);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetComposition_ShouldSetAndReturnComposition()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var composition = new[]
        {
            new ProductCompositionDto { Percentage = 80, Component = "Cotton" },
            new ProductCompositionDto { Percentage = 20, Component = "Polyester" }
        };

        // Act
        productInformationDto.Composition = composition;
        var result = productInformationDto.Composition;

        // Assert
        result.Should().BeEquivalentTo(composition);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetAlternatives_ShouldSetAndReturnAlternatives()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var alternatives = new[] { "Alternative 1", "Alternative 2" };

        // Act
        productInformationDto.Alternatives = alternatives;
        var result = productInformationDto.Alternatives;

        // Assert
        result.Should().BeEquivalentTo(alternatives);
    }

    [TestMethod]
    public void ProductInformationDto_SetAndGetBrand_ShouldSetAndReturnBrand()
    {
        // Arrange
        var productInformationDto = new ProductInformationDto();
        var brand = "Brand Name";

        // Act
        productInformationDto.Brand = brand;
        var result = productInformationDto.Brand;

        // Assert
        result.Should().Be(brand);
    }
}