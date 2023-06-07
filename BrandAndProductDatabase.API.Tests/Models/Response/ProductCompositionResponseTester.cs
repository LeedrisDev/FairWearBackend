using BrandAndProductDatabase.API.Models.Response;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Response;

[TestClass]
public class ProductCompositionResponseTester
{
    [TestMethod]
    public void ProductCompositionResponse_SetAndGetPercentage_ShouldSetAndReturnPercentage()
    {
        // Arrange
        var productCompositionResponse = new ProductCompositionResponse();
        var percentage = 75;

        // Act
        productCompositionResponse.Percentage = percentage;
        var result = productCompositionResponse.Percentage;

        // Assert
        result.Should().Be(percentage);
    }

    [TestMethod]
    public void ProductCompositionResponse_SetAndGetComponent_ShouldSetAndReturnComponent()
    {
        // Arrange
        var productCompositionResponse = new ProductCompositionResponse();
        var component = "Cotton";

        // Act
        productCompositionResponse.Component = component;
        var result = productCompositionResponse.Component;

        // Assert
        result.Should().Be(component);
    }
}