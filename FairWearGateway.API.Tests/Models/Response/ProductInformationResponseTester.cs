using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class ProductInformationResponseTester
{
    [TestMethod]
    public void ProductInformationResponse_Name_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();

        // Act
        productInformationResponse.Name = "T-Shirt";

        // Assert
        productInformationResponse.Name.Should().Be("T-Shirt");
    }

    [TestMethod]
    public void ProductInformationResponse_Country_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();

        // Act
        productInformationResponse.Country = "USA";

        // Assert
        productInformationResponse.Country.Should().Be("USA");
    }

    [TestMethod]
    public void ProductInformationResponse_Image_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();

        // Act
        productInformationResponse.Image = "https://example.com/image.jpg";

        // Assert
        productInformationResponse.Image.Should().Be("https://example.com/image.jpg");
    }

    [TestMethod]
    public void ProductInformationResponse_GlobalScore_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();

        // Act
        productInformationResponse.GlobalScore = 9;

        // Assert
        productInformationResponse.GlobalScore.Should().Be(9);
    }

    [TestMethod]
    public void ProductInformationResponse_Scores_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var scores = new ProductScoreResponse { Moral = 8, Animal = 6, Environmental = 7 };

        // Act
        productInformationResponse.Scores = scores;

        // Assert
        productInformationResponse.Scores.Should().BeEquivalentTo(scores);
    }

    [TestMethod]
    public void ProductInformationResponse_Composition_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();
        var composition = new List<ProductCompositionResponse>
        {
            new ProductCompositionResponse { Percentage = 50, Component = "Cotton" },
            new ProductCompositionResponse { Percentage = 30, Component = "Polyester" }
        };

        // Act
        productInformationResponse.Composition = composition;

        // Assert
        productInformationResponse.Composition.Should().BeEquivalentTo(composition);
    }

    [TestMethod]
    public void ProductInformationResponse_Brand_ShouldBeSettable()
    {
        // Arrange
        var productInformationResponse = new ProductInformationResponse();

        // Act
        productInformationResponse.Brand = "Nike";

        // Assert
        productInformationResponse.Brand.Should().Be("Nike");
    }
}