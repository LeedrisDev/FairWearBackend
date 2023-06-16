using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class ProductCompositionResponseTester
{
    [TestMethod]
    public void ProductCompositionResponse_Percentage_ShouldBeSettable()
    {
        // Arrange
        var productCompositionResponse = new ProductCompositionResponse();

        // Act
        productCompositionResponse.Percentage = 50;

        // Assert
        productCompositionResponse.Percentage.Should().Be(50);
    }

    [TestMethod]
    public void ProductCompositionResponse_Component_ShouldBeSettable()
    {
        // Arrange
        var productCompositionResponse = new ProductCompositionResponse();

        // Act
        productCompositionResponse.Component = "Cotton";

        // Assert
        productCompositionResponse.Component.Should().Be("Cotton");
    }
}