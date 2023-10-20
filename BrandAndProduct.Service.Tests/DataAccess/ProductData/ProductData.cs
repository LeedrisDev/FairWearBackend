using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Moq;
using ProductDataRetriever.Service.Protos;

namespace BrandAndProduct.Service.Tests.DataAccess.ProductData;

[TestClass]
public class ProductData
{
    private IMapper _mapper;
    private Mock<GrpcClientFactory> _mockGrpcClientFactory;
    private Mock<ProductScrapperService.ProductScrapperServiceClient> _mockProductServiceClient;
    private BrandAndProduct.Service.DataAccess.ProductData.ProductData _productData;


    [TestInitialize]
    public void Initialize()
    {
        _mockGrpcClientFactory = new Mock<GrpcClientFactory>();
        _mockProductServiceClient = new Mock<ProductScrapperService.ProductScrapperServiceClient>();
        _mockGrpcClientFactory
            .Setup(f => f.CreateClient<ProductScrapperService.ProductScrapperServiceClient>("ProductService"))
            .Returns(_mockProductServiceClient.Object);

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _productData =
            new BrandAndProduct.Service.DataAccess.ProductData.ProductData(_mockGrpcClientFactory.Object);
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsProductRetrieverDto()
    {
        // Arrange
        var expectedUpc = "123456789";
        var expectedProductRetriever = new ProductScrapperResponse()
        {
            UpcCode = expectedUpc,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
        };

        expectedProductRetriever.Ranges.AddRange(
            new List<string>()
            {
                "Men", "Women"
            });

        _mockProductServiceClient
            .Setup(c => c.GetProduct(It.IsAny<ProductScrapperRequest>(), null, null, new CancellationToken()))
            .Returns(expectedProductRetriever);

        // Act
        var result = _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(expectedProductRetriever);
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsNotFoundForNonExistentProduct()
    {
        // Arrange
        var expectedUpc = "123456789";
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _mockProductServiceClient
            .Setup(c => c.GetProduct(It.IsAny<ProductScrapperRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(
                new Status(StatusCode.NotFound, ($"Product with barcode {expectedUpc} not found."))));


        // Act
        var result = _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Product with barcode {expectedUpc} not found.");
    }

    [TestMethod]
    public async Task GetProductByUpc_ReturnsServiceUnavailableForUnsuccessfulRequest()
    {
        // Arrange
        var expectedUpc = "123456789";

        _mockProductServiceClient
            .Setup(c => c.GetProduct(It.IsAny<ProductScrapperRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal, ($"Barcode service is unavailable."))));


        // Act
        var result = _productData.GetProductByUpc(expectedUpc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be($"Barcode service is unavailable.");
    }
}