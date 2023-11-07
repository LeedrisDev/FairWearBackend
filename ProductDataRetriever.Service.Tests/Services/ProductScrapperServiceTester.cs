using System.Net;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Testing;
using Moq;
using ProductDataRetriever.Service.Business.ProductBusiness;
using ProductDataRetriever.Service.Models;
using ProductDataRetriever.Service.Protos;
using ProductScrapperService = ProductDataRetriever.Service.Services.ProductScrapperService;

namespace ProductDataRetriever.Test.Services;

[TestClass]
public class ProductScrapperServiceTester
{
    private ServerCallContext _context = null!;
    private Mock<IProductBusiness> _productBusinessMock = null!;
    private ProductScrapperService _productScrapperService = null!;


    [TestInitialize]
    public void Initialize()
    {
        _productBusinessMock = new Mock<IProductBusiness>();
        _productScrapperService = new ProductScrapperService(_productBusinessMock.Object);
        _context = TestServerCallContext.Create(
            method: nameof(ProductScrapperServiceTester)
            , host: "localhost"
            , deadline: DateTime.Now.AddMinutes(30)
            , requestHeaders: new Metadata()
            , cancellationToken: CancellationToken.None
            , peer: "10.0.0.25:5001"
            , authContext: null
            , contextPropagationToken: null
            , writeHeadersFunc: (metadata) => Task.CompletedTask
            , writeOptionsGetter: () => new WriteOptions()
            , writeOptionsSetter: (writeOptions) => { }
        );
    }

    [TestMethod]
    public async Task GetProduct_Returns_Ok_With_Product_Information()
    {
        // Arrange
        var barcode = "123456789";
        var productInformation = new ProductScrapperResponse
        {
            UpcCode = barcode,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
        };

        productInformation.Ranges.AddRange(
            new List<string>()
            {
                "Men", "Women"
            });

        _productBusinessMock.Setup(x => x.GetProductInformation(barcode)).ReturnsAsync(
            new ProcessingStatusResponse<ProductScrapperResponse>
            {
                Status = HttpStatusCode.OK,
                Object = productInformation
            });

        var request = new ProductScrapperRequest
        {
            UpcCode = barcode
        };

        // Act
        var response = await _productScrapperService.GetProduct(request, _context);

        // Assert
        response.Should().Be(productInformation);
    }

    [TestMethod]
    public async Task GetProduct_Returns_NotFound_When_Product_Is_Not_Found()
    {
        // Arrange
        var barcode = "1234567890";

        _productBusinessMock.Setup(x => x.GetProductInformation(barcode)).ReturnsAsync(
            new ProcessingStatusResponse<ProductScrapperResponse>
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Product not found"
            });

        var request = new ProductScrapperRequest
        {
            UpcCode = barcode
        };

        // Act
        try
        {
            await _productScrapperService.GetProduct(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be("Product not found");
        }
    }

    [TestMethod]
    public async Task GetProduct_Returns_InternalServerError_When_An_Error_Occurs()
    {
        // Arrange
        var barcode = "1234567890";

        _productBusinessMock.Setup(x => x.GetProductInformation(barcode)).ReturnsAsync(
            new ProcessingStatusResponse<ProductScrapperResponse>
            {
                Status = HttpStatusCode.InternalServerError,
                ErrorMessage = "An error occurred while retrieving product information."
            });

        var request = new ProductScrapperRequest
        {
            UpcCode = barcode
        };


        // Act
        try
        {
            await _productScrapperService.GetProduct(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred while retrieving product information.");
        }
    }

    // [TestMethod]
    // public async Task GetProduct_Returns_BadRequest_When_Barcode_Invalid()
    // {
    //     var barcode = "123sdas";
    //
    //     var request = new ProductScrapperRequest
    //     {
    //         UpcCode = barcode
    //     };
    //     
    //     // Act
    //     try
    //     {
    //         await _productScrapperService.GetProduct(request, _context);
    //     }
    //     catch (RpcException e)
    //     {
    //         // Assert
    //         e.Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    //         e.Status.Detail.Should().Be("Barcode must be composed of only numbers.");
    //     }
    //     
    // }
}