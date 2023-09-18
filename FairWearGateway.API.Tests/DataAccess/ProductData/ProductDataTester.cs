using System.Net;
using BrandAndProduct.Service.Protos;
using FluentAssertions;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Moq;

namespace FairWearGateway.API.Tests.DataAccess.ProductData;

[TestClass]
public class ProductDataTester
{
    private Mock<GrpcClientFactory> _mockGrpcClientFactory;
    private Mock<ProductService.ProductServiceClient> _mockProductServiceClient;
    private API.DataAccess.ProductData.ProductData _productData;

    [TestInitialize]
    public void Setup()
    {
        _mockGrpcClientFactory = new Mock<GrpcClientFactory>();
        _mockProductServiceClient = new Mock<ProductService.ProductServiceClient>();
        _mockGrpcClientFactory
            .Setup(f => f.CreateClient<ProductService.ProductServiceClient>("ProductService"))
            .Returns(_mockProductServiceClient.Object);
        _productData = new API.DataAccess.ProductData.ProductData(_mockGrpcClientFactory.Object);
    }

    [TestMethod]
    public async Task GetProductById_Should_Return_Correct_Product_When_Id_Exists()
    {
        // Arrange
        int testProductId = 1;
        var testProductResponse = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            BrandId = 1
        };

        testProductResponse.Ranges.AddRange(new List<string> { "Range A" });

        _mockProductServiceClient
            .Setup(c => c.GetProductByIdAsync(It.IsAny<ProductByIdRequest>(), null, null, new CancellationToken()))
            .Returns(testProductResponse);

        // Act
        var result = _productData.GetProductById(testProductId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(testProductResponse);
    }

    [TestMethod]
    public async Task GetProductById_Should_Return_NotFound_When_Id_Does_Not_Exists()
    {
        // Arrange
        int testProductId = 1;

        _mockProductServiceClient
            .Setup(c => c.GetProductByIdAsync(It.IsAny<ProductByIdRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.NotFound,
                $"Product with id {testProductId} could not be found")));

        // Act
        var result = _productData.GetProductById(testProductId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Product with id {testProductId} could not be found");
    }

    [TestMethod]
    public async Task GetProductById_Should_Return_InternalServerError_On_Exception()
    {
        // Arrange
        int testProductId = 1;

        _mockProductServiceClient
            .Setup(c => c.GetProductByIdAsync(It.IsAny<ProductByIdRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal,
                "Internal Server Error")));

        // Act
        var result = _productData.GetProductById(testProductId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("Internal Server Error");
    }

    [TestMethod]
    public async Task GetProductByUpc_Should_Return_Correct_Product_When_Upc_Exists()
    {
        // Arrange
        string testProductUpc = "123456789012";

        var productScores = new ProductScoreResponse()
        {
            Moral = 2,
            Animal = 2,
            Environmental = 2
        };

        var testProductInformationResponse = new ProductInformationResponse
        {
            Name = "Product 1",
            Country = "Country 1",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Brand = "Brand 1"
        };

        testProductInformationResponse.Composition.AddRange(new List<ProductCompositionResponse>());

        _mockProductServiceClient
            .Setup(c => c.GetProductByUpcAsync(It.IsAny<ProductByUpcRequest>(), null, null, new CancellationToken()))
            .Returns(testProductInformationResponse);

        // Act
        var result = _productData.GetProductByUpc(testProductUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(testProductInformationResponse);
    }

    [TestMethod]
    public async Task GetProductByUpc_Should_Return_NotFound_When_Upc_Does_Not_Exists()
    {
        // Arrange
        string testProductUpc = "123456789012";

        _mockProductServiceClient
            .Setup(c => c.GetProductByUpcAsync(It.IsAny<ProductByUpcRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.NotFound,
                $"Product with barcode {testProductUpc} could not be found")));

        // Act
        var result = _productData.GetProductByUpc(testProductUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Product with barcode {testProductUpc} could not be found");
    }

    [TestMethod]
    public async Task GetProductByUpc_Should_Return_InternalServerError_On_Exception()
    {
        // Arrange
        string testProductUpc = "123456789012";

        _mockProductServiceClient
            .Setup(c => c.GetProductByUpcAsync(It.IsAny<ProductByUpcRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal,
                "Internal Server Error")));

        // Act
        var result = _productData.GetProductByUpc(testProductUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("Internal Server Error");
    }

    [TestMethod]
    public async Task GetAllProducts_ReturnsCorrectProducts()
    {
        // Arrange

        var expectedProductResponses = new List<ProductResponse>
        {
            new ProductResponse
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                BrandId = 1
            },
            new ProductResponse
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "123456789",
                Category = "Category A",
                BrandId = 1
            }
        };

        var asyncResponseCallMock = new Mock<IAsyncStreamReader<ProductResponse>>();
        var moveNextResponses = new Queue<bool>();
        foreach (var _ in expectedProductResponses)
        {
            moveNextResponses.Enqueue(true);
        }

        moveNextResponses.Enqueue(false); // End of enumeration

        asyncResponseCallMock.Setup(a => a.MoveNext(CancellationToken.None))
            .ReturnsAsync(moveNextResponses.Dequeue);

        int index = 0;
        asyncResponseCallMock.Setup(a => a.Current)
            .Returns(() => expectedProductResponses[index++]);

        var asyncResponse =
            new AsyncServerStreamingCall<ProductResponse>(asyncResponseCallMock.Object, null, null, null, null);


        _mockProductServiceClient
            .Setup(b => b.GetAllProductsAsync(It.IsAny<ProductFilterList>(), null, default, default))
            .Returns(asyncResponse);


        // Act
        var actualProductResponse = await _productData.GetAllProducts(new Dictionary<string, string>());

        // Assert
        actualProductResponse.Status.Should().Be(HttpStatusCode.OK);
        actualProductResponse.Object.Should().BeEquivalentTo(expectedProductResponses);
    }

    [TestMethod]
    public async Task GetAllProducts_ReturnsInternalServerError_WhenRpcFails()
    {
        // Arrange
        var rpcException = new RpcException(new Status(StatusCode.Internal, "Internal Server error"));

        _mockProductServiceClient
            .Setup(b => b.GetAllProductsAsync(It.IsAny<ProductFilterList>(), null, default, default))
            .Throws(rpcException);

        // Act
        var actualResponse = await _productData.GetAllProducts(new Dictionary<string, string>());

        // Assert
        actualResponse.Status.Should().Be(HttpStatusCode.InternalServerError);
    }
}