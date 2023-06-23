using System.Net;
using FairWearGateway.API.Business.ProductBusiness;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FairWearGateway.API.Tests.Controllers;

[TestClass]
public class ProductControllerTester
{
    private Mock<IProductBusiness> _productBusinessMock;

    [TestInitialize]
    public void Initialize()
    {
        _productBusinessMock = new Mock<IProductBusiness>();
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsOkResultWithProductResponse()
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

        _productBusinessMock.Setup(m => m.GetProductByIdAsync(productId))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByIdAsync(productId);
        var okResult = actionResult as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(productResponse);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        var processingStatusResponse = new ProcessingStatusResponse<ProductResponse>()
        {
            Status = HttpStatusCode.NotFound,
        };

        _productBusinessMock.Setup(m => m.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByIdAsync(productId);
        var notFoundResult = actionResult as NotFoundObjectResult;

        // Assert
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        var productId = 1;
        var processingStatusResponse = new ProcessingStatusResponse<ProductResponse>()
        {
            Status = HttpStatusCode.InternalServerError,
        };


        _productBusinessMock.Setup(m => m.GetProductByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByIdAsync(productId);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)actionResult;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsOkResultWithProductInformationResponse()
    {
        // Arrange
        var upc = "123456";

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

        _productBusinessMock.Setup(m => m.GetProductByUpcAsync(upc))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByUpcAsync(upc);
        var okResult = actionResult as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(productInformationResponse);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var upc = "123456";

        var processingStatusResponse = new ProcessingStatusResponse<ProductInformationResponse>()
        {
            Status = HttpStatusCode.NotFound,
        };

        _productBusinessMock.Setup(m => m.GetProductByUpcAsync(It.IsAny<string>()))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByUpcAsync(upc);
        var notFoundResult = actionResult as NotFoundObjectResult;

        // Assert
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }


    [TestMethod]
    public async Task GetProductByUpcAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        var upc = "123456";

        var processingStatusResponse = new ProcessingStatusResponse<ProductInformationResponse>()
        {
            Status = HttpStatusCode.InternalServerError,
        };

        _productBusinessMock.Setup(m => m.GetProductByUpcAsync(It.IsAny<string>()))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var productsController = new API.Controllers.ProductsController(_productBusinessMock.Object);
        var actionResult = await productsController.GetProductByUpcAsync(upc);

        // Assert
        actionResult.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)actionResult;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}