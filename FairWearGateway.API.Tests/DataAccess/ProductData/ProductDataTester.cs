using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FluentAssertions;
using Grpc.Core;
using Moq;

namespace FairWearGateway.API.Tests.DataAccess.ProductData;

// [TestClass]
public class ProductDataTests
{
    private Mock<ProductService.ProductServiceClient> mockClient;
    private API.DataAccess.ProductData.ProductData productData;

    [TestInitialize]
    public void TestInitialize()
    {
        mockClient = new Mock<ProductService.ProductServiceClient>();
        // productData = new API.DataAccess.ProductData.ProductData { };
    }

    [TestMethod]
    public void GetProductById_WhenRpcExceptionIsThrown_ReturnsErrorResponse()
    {
        // Arrange
        var productId = 123;
        var expectedErrorMessage = "Product with id 123 could not be found";
        mockClient.Setup(c => c.GetProductByIdAsync(It.IsAny<ProductByIdRequest>(), null, null, default))
            .Throws(new RpcException(new Status(StatusCode.NotFound, "Not Found")));

        // Act
        var response = productData.GetProductById(productId);

        // Assert
        response.Status.Should().Be(HttpStatusCode.NotFound);
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Object.Should().BeNull();
    }

    [TestMethod]
    public void GetProductByUpc_WhenRpcExceptionIsThrown_ReturnsErrorResponse()
    {
        // Arrange
        var upc = "1234567890";
        var expectedErrorMessage = "Product with barcode 1234567890 could not be found";
        mockClient.Setup(c => c.GetProductByUpcAsync(It.IsAny<ProductByUpcRequest>(), null, null, default))
            .Throws(new RpcException(new Status(StatusCode.NotFound, "Not Found")));

        // Act
        var response = productData.GetProductByUpc(upc);

        // Assert
        response.Status.Should().Be(HttpStatusCode.NotFound);
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Object.Should().BeNull();
    }
}