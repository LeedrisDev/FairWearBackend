using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Dto;

public class ProductCompositionDtoTester
{
    [TestClass]
    public class ProductCompositionDtoTests
    {
        [TestMethod]
        public void ProductCompositionDto_SetAndGetPercentage_ShouldSetAndReturnPercentage()
        {
            // Arrange
            var productCompositionDto = new ProductCompositionDto();
            var percentage = 75;

            // Act
            productCompositionDto.Percentage = percentage;
            var result = productCompositionDto.Percentage;

            // Assert
            result.Should().Be(percentage);
        }

        [TestMethod]
        public void ProductCompositionDto_SetAndGetComponent_ShouldSetAndReturnComponent()
        {
            // Arrange
            var productCompositionDto = new ProductCompositionDto();
            var component = "Cotton";

            // Act
            productCompositionDto.Component = component;
            var result = productCompositionDto.Component;

            // Assert
            result.Should().Be(component);
        }
    }   
}