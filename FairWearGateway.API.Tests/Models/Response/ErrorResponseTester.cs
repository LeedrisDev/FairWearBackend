using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class ErrorResponseTester
{
    [TestMethod]
    public void BrandResponse_Ranges_ShouldBeSettable()
    {
        // Arrange
        var errorResponse = new ErrorResponse();

        // Act
        errorResponse.Message = "Error";

        // Assert
        errorResponse.Message.Should().BeEquivalentTo("Error");
    }
}