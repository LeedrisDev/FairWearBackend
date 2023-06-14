using BrandAndProductDatabase.API.Models.Response;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Response;

[TestClass]
public class ProductInformationResponseTester
{
    [TestMethod]
    public void ProductInformationResponse_SetAndGetName_ShouldSetAndReturnName()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var name = "Product Name";

        // Act
        productInformationResponse.Name = name;
        var result = productInformationResponse.Name;

        // Assert
        result.Should().Be(name);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetCountry_ShouldSetAndReturnCountry()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var country = "Country Name";

        // Act
        productInformationResponse.Country = country;
        var result = productInformationResponse.Country;

        // Assert
        result.Should().Be(country);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetImage_ShouldSetAndReturnImage()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var imageUrl = "https://example.com/product-image.jpg";

        // Act
        productInformationResponse.Image = imageUrl;
        var result = productInformationResponse.Image;

        // Assert
        result.Should().Be(imageUrl);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetGlobalScore_ShouldSetAndReturnGlobalScore()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var globalScore = 9;

        // Act
        productInformationResponse.GlobalScore = globalScore;
        var result = productInformationResponse.GlobalScore;

        // Assert
        result.Should().Be(globalScore);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetScores_ShouldSetAndReturnScores()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var scores = new ProductScoreResponse { Moral = 8, Animal = 5, Environmental = 7 };

        // Act
        productInformationResponse.Scores = scores;
        var result = productInformationResponse.Scores;

        // Assert
        result.Should().BeEquivalentTo(scores);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetComposition_ShouldSetAndReturnComposition()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var composition = new[]
        {
            new ProductCompositionResponse { Percentage = 80, Component = "Cotton" },
            new ProductCompositionResponse { Percentage = 20, Component = "Polyester" }
        };

        // Act
        productInformationResponse.Composition = composition;
        var result = productInformationResponse.Composition;

        // Assert
        result.Should().BeEquivalentTo(composition);
    }

    [TestMethod]
    public void ProductInformationResponse_SetAndGetBrand_ShouldSetAndReturnBrand()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var brand = "Brand Name";

        // Act
        productInformationResponse.Brand = brand;
        var result = productInformationResponse.Brand;

        // Assert
        result.Should().Be(brand);
    }
}