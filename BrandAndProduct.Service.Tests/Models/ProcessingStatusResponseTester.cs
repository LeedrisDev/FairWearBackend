using System.Net;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Response;
using FluentAssertions;

namespace BrandAndProduct.Service.Tests.Models;

[TestClass]
public class ProcessingStatusResponseTester
{
    [TestMethod]
    public void ProcessingStatusResponse_Status_ShouldBeSettable()
    {
        // Arrange
        var response = new ProcessingStatusResponse<string>();

        // Act
        response.Status = HttpStatusCode.BadRequest;

        // Assert
        response.Status.Should().Be(HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public void ProcessingStatusResponse_ErrorMessage_ShouldBeSettable()
    {
        // Arrange
        var response = new ProcessingStatusResponse<string>();

        // Act
        response.ErrorMessage = "Test Error Message";

        // Assert
        response.ErrorMessage.Should().Be("Test Error Message");
        response.MessageObject.Should().NotBeNull().And.BeOfType<ErrorResponse>().And
            .Match<ErrorResponse>(e => e.Message == "Test Error Message");
    }

    [TestMethod]
    public void ProcessingStatusResponse_Object_ShouldBeSettable()
    {
        // Arrange
        var response = new ProcessingStatusResponse<string>();

        // Act
        response.Object = "Test Object";

        // Assert
        response.Object.Should().Be("Test Object");
    }
}