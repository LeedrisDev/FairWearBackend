using System.Net;
using FairWearGateway.API.DataAccess.ProductData;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;
using FluentAssertions;
using Moq;

namespace FairWearGateway.API.Tests.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTester
{
    private Mock<IProductData> _productDataMock;

    [TestInitialize]
    public void Initialize()
    {
        _productDataMock = new Mock<IProductData>();
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsProductResponse()
    {
        // Arrange
        var productId = 1;
        var productResponse = new ProductResponse
        {
            Id = productId,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1
        };
        var processingStatusResponse = new ProcessingStatusResponse<ProductResponse>()
        {
            Status = HttpStatusCode.OK,
            Object = productResponse
        };

        _productDataMock.Setup(m => m.GetProductByIdAsync(productId))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productDataMock.Object);
        var result = await productBusiness.GetProductByIdAsync(productId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productResponse);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        var businessResponse = new ProcessingStatusResponse<ProductResponse>()
        {
            Status = HttpStatusCode.NotFound
        };

        _productDataMock.Setup(m => m.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(businessResponse);

        // Act
        var productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productDataMock.Object);
        var result = await productBusiness.GetProductByIdAsync(productId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsProductInformationResponse()
    {
        // Arrange
        var upc = "123456789";

        var productScores = new ProductScoreResponse()
        {
            Moral = 2,
            Animal = 2,
            Environmental = 2
        };

        var productInformationResponse = new ProductInformationResponse
        {
            Name = "Product 1",
            Country = "Country 1",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Composition = new List<ProductCompositionResponse>(),
            Brand = "Brand 1"
        };
        var processingStatusResponse = new ProcessingStatusResponse<ProductInformationResponse>()
        {
            Status = HttpStatusCode.OK,
            Object = productInformationResponse
        };

        _productDataMock.Setup(m => m.GetProductByUpcAsync(upc))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productDataMock.Object);
        var result = await productBusiness.GetProductByUpcAsync(upc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInformationResponse);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var upc = "123456789";

        var processingStatusResponse = new ProcessingStatusResponse<ProductInformationResponse>()
        {
            Status = HttpStatusCode.NotFound,
        };

        _productDataMock.Setup(m => m.GetProductByUpcAsync(It.IsAny<string>()))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productDataMock.Object);
        var result = await productBusiness.GetProductByUpcAsync(upc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}