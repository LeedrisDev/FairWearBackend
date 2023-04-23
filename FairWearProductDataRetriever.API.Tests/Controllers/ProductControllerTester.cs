using System.Net;
using FairWearProductDataRetriever.API.Business.ProductBusiness;
using FairWearProductDataRetriever.API.Controllers;
using FairWearProductDataRetriever.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ProductDataRetriever.Test.Controllers;

[TestClass]
public class ProductControllerTester
{
    private readonly Mock<IProductBusiness> _productBusinessMock;
    private readonly ProductController _productController;

    public ProductControllerTester()
    {
        _productBusinessMock = new Mock<IProductBusiness>();
        _productController = new ProductController(_productBusinessMock.Object);
    }

    [TestMethod]
    public async Task GetProduct_Returns_Ok_With_Product_Information()
    {
        // Arrange
        var barcode = "123456789";
        var productInformation = new ProductModel
        {
            UpcCode = barcode,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
            Ranges = new List<string>()
            {
                "Men", "Women"
            }
        };

        _productBusinessMock.Setup(x => x.GetProductInformation(barcode)).ReturnsAsync(new ProcessingStatusResponse<ProductModel>
        {
            Status = HttpStatusCode.OK,
            Object = productInformation
        });

        
        var response = await _productController.GetProduct(barcode);
        
        response.Should().BeOfType<OkObjectResult>();
        
        var result = response as OkObjectResult;
        result?.Value.Should().BeSameAs(productInformation);
        
    }
    
    [TestMethod]
    public async Task GetProduct_Returns_NotFound_When_Product_Is_Not_Found()
    {
        // Arrange
        var barcode = "1234567890";

        _productBusinessMock.Setup(x => x.GetProductInformation(barcode)).ReturnsAsync(new ProcessingStatusResponse<ProductModel>
        {
            Status = HttpStatusCode.NotFound,
            ErrorMessage = "Product not found"
        });

        // Act
        var response = await _productController.GetProduct(barcode);

        // Assert
        response.Should().BeOfType<NotFoundObjectResult>();
        
        var result = response as NotFoundObjectResult;
        result?.Value.Should().Be("Product not found");
    }
    
    

}