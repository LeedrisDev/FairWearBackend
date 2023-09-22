using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Business.ProductBusiness;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Protos;
using BrandAndProduct.Service.Tests.TestHelpers;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService = BrandAndProduct.Service.Services.ProductService;

namespace BrandAndProduct.Service.Tests.Services;

[TestClass]
public class ProductServiceTester
{
    private CancellationTokenSource _cancellationTokenSource = null!;
    private ServerCallContext _context = null!;
    private Mock<ILogger<ProductService>> _logger = null!;
    private IMapper _mapper = null!;
    private Mock<IProductBusiness> _productBusinessMock = null!;
    private ProductService _productService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _productBusinessMock = new Mock<IProductBusiness>();
        _logger = new Mock<ILogger<ProductService>>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _productService = new ProductService(_productBusinessMock.Object, _logger.Object, _mapper);
        _cancellationTokenSource = new CancellationTokenSource();

        _context = TestServerCallContext.Create(
            method: nameof(ProductService)
            , host: "localhost"
            , deadline: DateTime.Now.AddMinutes(30)
            , requestHeaders: new Metadata()
            , cancellationToken: _cancellationTokenSource.Token
            , peer: "10.0.0.25:5001"
            , authContext: null
            , contextPropagationToken: null
            , writeHeadersFunc: (metadata) => Task.CompletedTask
            , writeOptionsGetter: () => new WriteOptions()
            , writeOptionsSetter: (writeOptions) => { }
        );
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
                BrandId = 1,
            },
            new ProductResponse()
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                BrandId = 2,
            },
        };

        expected[0].Ranges.AddRange(new List<string> { "Range A" });
        expected[1].Ranges.AddRange(new List<string> { "Range B" });

        _productBusinessMock.Setup(x => x.GetAllProductsAsync(It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(productList);

        var request = new ProductFilterList();
        var responseStream = new TestServerStreamWriter<ProductResponse>(_context);

        // Act
        using var call = _productService.GetAllProductsAsync(request, responseStream, _context);
        call.IsCompletedSuccessfully.Should().BeTrue(); //Method should run until cancelled.
        _cancellationTokenSource.Cancel();

        await call;
        responseStream.Complete();

        var allProducts = new List<ProductResponse>();
        await foreach (var product in responseStream.ReadAllAsync())
        {
            allProducts.Add(product);
        }

        // Assert
        allProducts.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void GetAllProductsAsync_ReturnsErrorStatusCode_WhenProductBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<IEnumerable<ProductDto>>()
        {
            Status = HttpStatusCode.InternalServerError,
            ErrorMessage = "Internal server error"
        };

        var request = new ProductFilterList();

        _productBusinessMock.Setup(b => b.GetAllProductsAsync(It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(businessResult);

        var responseStream = new TestServerStreamWriter<ProductResponse>(_context);

        // Act
        using var call = _productService.GetAllProductsAsync(request, responseStream, _context);
        call.IsCompletedSuccessfully.Should().BeFalse(); //Method should FAIL
    }


    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsOk_WhenProductExists()
    {
        // Arrange

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

        var request = new ProductByIdRequest()
        {
            Id = 1
        };

        // Act
        var result = await _productService.GetProductByIdAsync(request, _context);

        // Assert
        result.Should().BeEquivalentTo(productResponse);
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
        var request = new ProductByIdRequest()
        {
            Id = 1
        };

        // Act
        try
        {
            await _productService.GetProductByIdAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }
    }

    [TestMethod]
    public async Task GetProductByIdAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange

        _productBusinessMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.InternalServerError,
                MessageObject = { Message = "An error occurred." }
            }
        );

        var request = new ProductByIdRequest()
        {
            Id = 1
        };

        // Act
        try
        {
            await _productService.GetProductByIdAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred.");
        }
    }


    [TestMethod]
    public async Task CreateProductAsync_ReturnsCreatedResultWithProduct_WhenProductBusinessCreatesProduct()
    {
        // Arrange
        var request = new ProductRequest()
        {
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            BrandId = 1,
        };

        request.Ranges.AddRange(new List<string> { "Range A" });

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

        var expected = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            BrandId = 1,
        };

        expected.Ranges.AddRange(new List<string> { "Range A" });

        _productBusinessMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(businessResult);

        // Act
        var result = await _productService.CreateProductAsync(request, _context);

        // Assert
        result.Should().Be(expected);
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
            BrandId = 1,
        };

        product.Ranges.AddRange(new List<string> { "Range A" });

        var businessResponse = new ProcessingStatusResponse<ProductDto>()
        {
            Status = HttpStatusCode.BadRequest
        };

        _productBusinessMock.Setup(x => x.CreateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(businessResponse);

        var request = new ProductRequest()
        {
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            BrandId = 1,
        };

        request.Ranges.AddRange(new List<string> { "Range A" });

        // Act
        try
        {
            await _productService.CreateProductAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.Internal);
        }
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
            BrandId = 1,
        };
        product.Ranges.AddRange(new List<string> { "Range A" });

        var updatedProduct = new ProductResponse
        {
            Id = 1,
            Name = "Updated product",
            UpcCode = "987654321",
            Category = "Updated Category",
            BrandId = 1,
        };
        updatedProduct.Ranges.AddRange(new List<string> { "Updated Range" });

        _productBusinessMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = _mapper.Map<ProductDto>(updatedProduct),
                Status = HttpStatusCode.OK
            });

        // Act
        var result = await _productService.UpdateProductAsync(product, _context);

        // Assert
        result.Should().Be(updatedProduct);
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
            BrandId = 1,
        };

        product.Ranges.AddRange(new List<string> { "Range A" });

        _productBusinessMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Product with ID {product.Id} not found."
            });

        // Act
        try
        {
            await _productService.UpdateProductAsync(product, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be($"Product with ID {product.Id} not found.");
        }
    }

    [TestMethod]
    public async Task UpdateProductAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        var product = new ProductResponse
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            BrandId = 1,
        };

        product.Ranges.AddRange(new List<string> { "Range A" });


        _productBusinessMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.InternalServerError,
                MessageObject = { Message = "An error occurred." }
            }
        );

        // Act
        try
        {
            await _productService.UpdateProductAsync(product, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred.");
        }
    }

    [TestMethod]
    public void DeleteProductAsync_ReturnsNoContentResult_WhenProductIsDeleted()
    {
        // Arrange
        var productId = 1;
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

        var request = new ProductByIdRequest()
        {
            Id = productId
        };

        // Act
        var result = _productService.DeleteProductAsync(request, _context);

        // Assert
        result.IsCompletedSuccessfully.Should().BeTrue();
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

        var request = new ProductByIdRequest()
        {
            Id = productId
        };

        // Act
        try
        {
            await _productService.DeleteProductAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsOkResults()
    {
        // Arrange
        const String upcCode = "12345";

        var productScoresDto = new ProductScoreDto
        {
            Moral = 2,
            Animal = 2,
            Environmental = 3
        };

        var productCompositionDto = new List<ProductCompositionDto>
        {
            new()
            {
                Percentage = 77,
                Component = "viscose"
            },
            new()
            {
                Percentage = 23,
                Component = "polyamide"
            }
        };
        var productInformationDto = new ProductInformationDto()
        {
            Name = "White shirt",
            Country = "Bangladesh",
            Image = "image",
            Scores = productScoresDto,
            GlobalScore = (productScoresDto.Animal + productScoresDto.Environmental + productScoresDto.Moral) / 3,
            Composition = productCompositionDto.ToArray(),
            Brand = "Bershka"
        };

        var productScores = new ProductScoreResponse()
        {
            Moral = 2,
            Animal = 2,
            Environmental = 3
        };

        var productComposition = new List<ProductCompositionResponse>
        {
            new()
            {
                Percentage = 77,
                Component = "viscose"
            },
            new()
            {
                Percentage = 23,
                Component = "polyamide"
            }
        };
        var productInformation = new ProductInformationResponse()
        {
            Name = "White shirt",
            Country = "Bangladesh",
            Image = "image",
            Scores = productScores,
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Brand = "Bershka"
        };

        productInformation.Composition.AddRange(productComposition);

        _productBusinessMock.Setup(x => x.GetProductByUpcAsync(It.IsAny<String>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductInformationDto>()
            {
                Object = productInformationDto
            }
        );

        var request = new ProductByUpcRequest()
        {
            UpcCode = upcCode
        };
        // Act
        var result = await _productService.GetProductByUpcAsync(request, _context);

        // Assert
        result.Should().Be(productInformation);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_NonExistingProduct_ReturnsNotFoundResult()
    {
        // Arrange
        const String upcCode = "12345";

        _productBusinessMock.Setup(x => x.GetProductByUpcAsync(It.IsAny<String>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = HttpStatusCode.NotFound,
                MessageObject = { Message = "Item not found" }
            }
        );
        var request = new ProductByUpcRequest()
        {
            UpcCode = upcCode
        };

        // Act
        try
        {
            await _productService.GetProductByUpcAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be("Item not found");
        }
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        const String upcCode = "12345";

        _productBusinessMock.Setup(x => x.GetProductByUpcAsync(It.IsAny<String>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductInformationDto>()
            {
                Status = HttpStatusCode.InternalServerError,
                MessageObject = { Message = "An error occurred." }
            }
        );

        var request = new ProductByUpcRequest()
        {
            UpcCode = upcCode
        };

        // Act
        try
        {
            await _productService.GetProductByUpcAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred.");
        }
    }
}