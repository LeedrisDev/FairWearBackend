using System.Net;
using FairWearGateway.API.Models.Response;
using FairWearGateway.API.Utils.HttpClientWrapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace FairWearGateway.API.Tests.DataAccess.ProductData;

[TestClass]
public class ProductDataTester
{
    private Mock<IHttpClientWrapper> _httpClientWrapperMock;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
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

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(productResponse));

        _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var productData = new API.DataAccess.ProductData.ProductData(_httpClientWrapperMock.Object);
        var result = await productData.GetProductByIdAsync(productId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productResponse);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsErrorResponse()
    {
        // Arrange
        var productId = 1;

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
        httpResponseMessage.ReasonPhrase = "Product not found";

        _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var productData = new API.DataAccess.ProductData.ProductData(_httpClientWrapperMock.Object);
        var result = await productData.GetProductByIdAsync(productId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be("Product not found");
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsProductInfoResponse()
    {
        // Arrange
        var upc = "1234567890";

        var productScores = new ProductScoreResponse()
        {
            Moral = 2,
            Animal = 2,
            Environmental = 2
        };

        var productInfoResponse = new ProductInformationResponse
        {
            Name = "Product 1",
            Country = "Country 1",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Composition = new List<ProductCompositionResponse>(),
            Brand = "Brand 1"
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(productInfoResponse));

        _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var productData = new API.DataAccess.ProductData.ProductData(_httpClientWrapperMock.Object);
        var result = await productData.GetProductByUpcAsync(upc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInfoResponse);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsErrorResponse()
    {
        // Arrange
        var upc = "1234567890";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        httpResponseMessage.ReasonPhrase = "Invalid UPC";

        _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var productData = new API.DataAccess.ProductData.ProductData(_httpClientWrapperMock.Object);
        var result = await productData.GetProductByUpcAsync(upc);

        // Assert
        result.Status.Should().Be(HttpStatusCode.BadRequest);
        result.ErrorMessage.Should().Be("Invalid UPC");
    }
}