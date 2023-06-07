using BrandAndProductDatabase.API.Models.Request;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Request;

[TestClass]
public class BrandRequestTester
{
    [TestMethod]
    public void BrandRequest_SetAndGetBrandName_ShouldSetAndReturnBrandName()
    {
        // Arrange
        var brandRequest = new BrandRequest();
        var brandName = "Brand Name";

        // Act
        brandRequest.Name = brandName;
        var result = brandRequest.Name;

        // Assert
        result.Should().Be(brandName);
    }
}