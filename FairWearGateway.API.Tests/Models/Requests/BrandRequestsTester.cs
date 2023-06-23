using FairWearGateway.API.Models.Request;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Requests;

[TestClass]
public class BrandRequestsTester
{
    [TestMethod]
    public void BrandRequest_Name_ShouldBeSettable()
    {
        // Arrange
        var brandRequest = new BrandRequest
        {
            // Act
            Name = "Nike"
        };

        // Assert
        brandRequest.Name.Should().Be("Nike");
    }
}