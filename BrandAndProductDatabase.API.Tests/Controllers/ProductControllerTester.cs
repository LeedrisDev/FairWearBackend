using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.ProductBusiness;
using BrandAndProductDatabase.API.Controllers;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Response;
using BrandAndProductDatabase.API.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BrandAndProductDatabase.API.Tests.Controllers;

[TestClass]
public class ProductControllerTester
{
    private ProductController _controller;
    private IMapper _mapper;
    private Mock<IProductBusiness> _productBusinessMock;

    [TestInitialize]
    public void Initialize()
    {
        _productBusinessMock = new Mock<IProductBusiness>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _controller = new ProductController(_productBusinessMock.Object, _mapper);
    }

    [TestMethod]
    public async Task GetAllProductsAsync_ReturnsOk_WithProductResponseList()
    {
        // Arrange
        var productList = new ProcessingStatusResponse<IEnumerable<ProductDto>>()
        {
            Object = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = 1,
                    Name = "Product 1",
                    UpcCode = "123456789",
                    Category = "Category A",
                    Ranges = new List<string> { "Range A" },
                    BrandId = 1,
                },
                new ProductDto()
                {
                    Id = 2,
                    Name = "Product 2",
                    UpcCode = "987654321",
                    Category = "Category B",
                    Ranges = new List<string> { "Range B" },
                    BrandId = 2,
                },
            },
            Status = HttpStatusCode.OK
        };

        var expected = new List<ProductResponse>()
        {
            new ProductResponse()
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductResponse()
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            },
        };

        _productBusinessMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(productList);

        // Act
        var result = await _controller.GetAllProductsAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;

        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsOk_WhenProductExists()
    {
        // Arrange
        const int productId = 1;

        var businessResponse = new ProcessingStatusResponse<ProductDto>()
        {
            Object = new ProductDto()
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            Status = HttpStatusCode.OK
        };


        var productResponse = _mapper.Map<ProductResponse>(businessResponse.Object);

        _productBusinessMock.Setup(b => b.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetProductByIdAsync(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult?.Value.Should().BeEquivalentTo(productResponse);
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange

        var businessResponse = new ProcessingStatusResponse<ProductDto>()
        {
            Status = HttpStatusCode.NotFound
        };

        _productBusinessMock.Setup(b => b.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);
        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetProductByIdAsync(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateProductAsync_ReturnsCreatedResultWithProduct_WhenProductBusinessCreatesProduct()
    {
        // Arrange
        var product = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };
        var createdProduct = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };
        var businessResult = new ProcessingStatusResponse<ProductDto>()
        {
            Object = createdProduct,
            Status = HttpStatusCode.OK
        };

        _productBusinessMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(businessResult);

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.CreateProductAsync(product);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var createdResult = (OkObjectResult)result;
        createdResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        createdResult.Value.Should().BeEquivalentTo(_mapper.Map<ProductResponse>(createdProduct),
            options => options.Excluding(x => x.Id));
    }

    [TestMethod]
    public async Task CreateProductAsync_ReturnsBadRequestResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var product = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        var businessResponse = new ProcessingStatusResponse<ProductDto>()
        {
            Status = HttpStatusCode.BadRequest
        };

        _productBusinessMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(businessResponse);
        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.CreateProductAsync(product);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var badRequestResult = (ObjectResult)result;
        badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task UpdateProductAsync_ReturnsUpdatedProduct_WhenProductIsValid()
    {
        // Arrange
        var product = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };


        var updatedProduct = new ProductResponse
        {
            Id = 1,
            Name = "Updated product",
            UpcCode = "987654321",
            Category = "Updated Category",
            Ranges = new List<string> { "Updated Range" },
            BrandId = 1,
        };

        _productBusinessMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = _mapper.Map<ProductDto>(updatedProduct),
                Status = HttpStatusCode.OK
            });

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.UpdateProductAsync(product);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(_mapper.Map<ProductResponse>(updatedProduct));
    }

    [TestMethod]
    public async Task UpdateProductAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var product = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        _productBusinessMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Product with ID {product.Id} not found."
            });

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.UpdateProductAsync(product);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        notFoundResult.Value.Should().Be($"Product with ID {product.Id} not found.");
    }

    [TestMethod]
    public async Task DeleteProductAsync_ReturnsNoContentResult_WhenProductIsDeleted()
    {
        // Arrange
        const int productId = 1;

        var product = new ProductDto()
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1
        };

        _productBusinessMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = product,
                Status = HttpStatusCode.OK
            });

        _productBusinessMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.OK
            });

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.DeleteProductAsync(productId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var noContentResult = (ObjectResult)result;
        noContentResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task DeleteProductAsync_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        // Arrange
        const int productId = 1;
        _productBusinessMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(
            (new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound
            }));

        var controller = new ProductController(_productBusinessMock.Object, _mapper);

        // Act
        var result = await controller.DeleteProductAsync(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = (NotFoundResult)result;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}