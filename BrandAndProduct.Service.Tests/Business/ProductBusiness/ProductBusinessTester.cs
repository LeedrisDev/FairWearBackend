using System.Net;
using BrandAndProduct.Service.DataAccess.BrandData;
using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.DataAccess.ProductData;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Services;
using FluentAssertions;
using Moq;
using ProductDataRetriever.Service.Protos;

namespace BrandAndProduct.Service.Tests.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTester
{
    private Mock<IBrandData> _brandDataMock = null!;
    private Mock<IBrandRepository> _brandRepositoryMock = null!;
    private Mock<IFilterFactory<IFilter>> _filterFactoryMock = null!;
    private Mock<IIntegrationEventRepository> _integrationEventRepositoryMock = null!;
    private Mock<IIntegrationEventSenderService> _integrationEventSenderServiceMock = null!;
    private BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness _productBusiness = null!;
    private Mock<IProductData> _productDataMock = null!;
    private Mock<IProductRepository> _productRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _productDataMock = new Mock<IProductData>();
        _brandDataMock = new Mock<IBrandData>();
        _filterFactoryMock = new Mock<IFilterFactory<IFilter>>();
        _integrationEventRepositoryMock = new Mock<IIntegrationEventRepository>();
        _integrationEventSenderServiceMock = new Mock<IIntegrationEventSenderService>();
    }

    [TestMethod]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var productsInDb = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1
            },
            new()
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2
            }
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>
            {
                Object = productsInDb
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var filterDict = new Dictionary<string, string>();
        var result = await productBusiness.GetAllProductsAsync(filterDict);

        // // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productsInDb);
    }

    [TestMethod]
    public async Task GetAllProductsAsync_ReturnsError()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>
            {
                Status = HttpStatusCode.InternalServerError,
                ErrorMessage = "internal server error"
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var filterDict = new Dictionary<string, string>();
        var result = await productBusiness.GetAllProductsAsync(filterDict);

        // // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("internal server error");
    }

    [TestMethod]
    public async Task GetProductByIdAsync_ReturnsProductIfExists()
    {
        // Arrange
        var productInDb = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productInDb
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByIdAsync(productInDb.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInDb);
    }


    [TestMethod]
    public async Task GetProductByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var productId = 1;

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }


    [TestMethod]
    public async Task CreateProductAsync_WithValidProductDto_CreatesProduct()
    {
        // Arrange
        var productDto = new ProductDto
        {
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        var createdProductInDb = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = createdProductInDb
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK,
                Object = new BrandDto
                {
                    Id = 1,
                    Name = "Brand 1",
                    Country = "Country A",
                    EnvironmentRating = 2,
                    PeopleRating = 2,
                    AnimalRating = 2,
                    RatingDescription = "Rating 2"
                },
            });

        _integrationEventRepositoryMock.Setup(x => x.AddAsync(It.IsAny<IntegrationEventDto>())).ReturnsAsync(
            new ProcessingStatusResponse<IntegrationEventDto>()
            {
                Status = HttpStatusCode.OK
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.CreateProductAsync(productDto);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(createdProductInDb);
    }

    [TestMethod]
    public async Task UpdateProductAsync_WithValidProductDto_UpdatesProduct()
    {
        // Arrange
        var productInDb = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1,
        };

        var productToUpdate = new ProductDto
        {
            Id = 1,
            Name = "Updated product",
            UpcCode = "987654321",
            Category = "Updated Category",
            Ranges = new List<string> { "Updated Range" },
            BrandId = 1,
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productInDb
            });

        _productRepositoryMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productToUpdate
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK,
                Object = new BrandDto
                {
                    Id = 1,
                    Name = "Brand 1",
                    Country = "Country A",
                    EnvironmentRating = 2,
                    PeopleRating = 2,
                    AnimalRating = 2,
                    RatingDescription = "Rating 2"
                },
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);
        // Act
        var result = await productBusiness.UpdateProductAsync(productToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productToUpdate);
    }


    [TestMethod]
    public async Task UpdateProductAsync_WithInvalidProduct_ReturnsNull()
    {
        // Arrange
        var productToUpdate = new ProductDto
        {
            Id = 1,
            Name = "Updated product",
            UpcCode = "987654321",
            Category = "Updated Category",
            Ranges = new List<string> { "Updated Range" },
            BrandId = 1,
        };

        _productRepositoryMock.Setup(x => x.UpdateProductAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK,
                Object = new BrandDto
                {
                    Id = 1,
                    Name = "Brand 1",
                    Country = "Country A",
                    EnvironmentRating = 2,
                    PeopleRating = 2,
                    AnimalRating = 2,
                    RatingDescription = "Rating 2"
                },
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.UpdateProductAsync(productToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task DeleteProductAsync_WithValidId_DeletesProduct()
    {
        // Arrange
        var productInDb = new ProductDto
        {
            Id = 1,
            Name = "Product 1",
            UpcCode = "123456789",
            Category = "Category A",
            Ranges = new List<string> { "Range A" },
            BrandId = 1
        };

        _productRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productInDb
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.DeleteProductAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInDb);
    }

    [TestMethod]
    public async Task DeleteProductAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound,
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.DeleteProductAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateProductAsync_WithBrandDoesNotExist_ThenReturnsBadRequestResponse()
    {
        // Arrange
        _productRepositoryMock = new Mock<IProductRepository>();
        _productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        const int nonExistentBrandId = 123;
        var productDto = new ProductDto { BrandId = nonExistentBrandId };
        _brandRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistentBrandId))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto>
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Brand with Id {nonExistentBrandId} does not exist."
            });

        // Act
        var result = await _productBusiness.CreateProductAsync(productDto);

        // Assert
        result.Status.Should().Be(HttpStatusCode.BadRequest);
        result.ErrorMessage.Should().Be($"Brand with Id {nonExistentBrandId} does not exist.");
        _productRepositoryMock.Verify(repo => repo.AddAsync(productDto), Times.Never);
    }

    [TestMethod]
    public async Task CreateProductAsync_BrandExists_CallsProductRepository()
    {
        // Arrange
        _productRepositoryMock = new Mock<IProductRepository>();
        _productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);
        const int existingBrandId = 456;
        var productDto = new ProductDto { BrandId = existingBrandId };
        var brandRepositoryResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.OK,
            Object = new BrandDto { Id = existingBrandId }
        };
        var expectedResponse = new ProcessingStatusResponse<ProductDto>
        {
            Status = HttpStatusCode.Created,
            Object = productDto
        };

        _brandRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingBrandId))
            .ReturnsAsync(brandRepositoryResponse);
        _productRepositoryMock
            .Setup(repo => repo.AddAsync(productDto))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _productBusiness.CreateProductAsync(productDto);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _productRepositoryMock.Verify(repo => repo.AddAsync(productDto), Times.Once);
    }

    [TestMethod]
    public async Task UpdateProductAsync_WithBrandDoesNotExist_ThenReturnsBadRequestResponse()
    {
        // Arrange
        _productRepositoryMock = new Mock<IProductRepository>();
        _productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        const int nonExistentBrandId = 123;
        var productDto = new ProductDto { BrandId = nonExistentBrandId };
        _brandRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistentBrandId))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto>
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = $"Brand with Id {nonExistentBrandId} does not exist."
            });

        // Act
        var result = await _productBusiness.UpdateProductAsync(productDto);

        // Assert
        result.Status.Should().Be(HttpStatusCode.BadRequest);
        result.ErrorMessage.Should().Be($"Brand with Id {nonExistentBrandId} does not exist.");
        _productRepositoryMock.Verify(repo => repo.AddAsync(productDto), Times.Never);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsOkResponse()
    {
        // Arrange
        var upcCode = "123456789";

        var productsInDb = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1
            },
            new()
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2
            }
        };

        var brandInDb = new BrandDto()
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2",
            Categories = new List<string> { "Category 2" },
            Ranges = new List<string> { "Range 2" }
        };

        var productScores = new ProductScoreDto
        {
            Moral = 2,
            Animal = 2,
            Environmental = 2
        };

        var productInformation = new ProductInformationDto()
        {
            Name = "Product 1",
            Country = "Country 1",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Composition = new List<ProductCompositionDto>(),
            Brand = "Brand 1"
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = productsInDb
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brandInDb
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInformation);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_WithProductNotInDb_ReturnsOkResponse()
    {
        // Arrange
        var upcCode = "123";

        var productsInDb = new List<ProductDto>();

        var productScrapperResponse = new ProductScrapperResponse()
        {
            UpcCode = upcCode,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
        };
        productScrapperResponse.Ranges.AddRange(
            new List<string>()
            {
                "Men", "Women"
            }
        );

        var brand = new BrandDto()
        {
            Id = 1,
            Name = "NorthFace",
            Country = "USA",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating",
            Categories = new List<string> { "Category" },
            Ranges = new List<string> { "Range" }
        };

        var product = new ProductDto()
        {
            Id = 1,
            Name = "Etip Hardface Glove",
            UpcCode = upcCode,
            Category = "Category",
            Ranges = new List<string> { "Range" },
            BrandId = 1
        };

        var productScores = new ProductScoreDto
        {
            Moral = 1,
            Animal = 1,
            Environmental = 1
        };

        var productInformation = new ProductInformationDto()
        {
            Name = "Etip Hardface Glove",
            Country = "USA",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Composition = new List<ProductCompositionDto>(),
            Brand = "NorthFace"
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = new List<ProductDto>()
            }
        );

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<ProductScrapperResponse>()
            {
                Status = HttpStatusCode.OK,
                Object = productScrapperResponse
            });

        _brandDataMock.Setup(x => x.GetBrandByName(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brand
            });

        _brandRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brand
            });

        _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = product
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productInformation);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_ReturnsNotFound()
    {
        // Arrange
        var upcCode = "123";

        var productsInDb = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1
            },
            new()
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2
            }
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = productsInDb
            });

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<ProductScrapperResponse>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Not Found"
            }
        );

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_GetAllError_ReturnsError()
    {
        // Arrange
        var upcCode = "123";

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Status = HttpStatusCode.InternalServerError,
            });

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_GetByUpcFails_ReturnsError()
    {
        // Arrange
        var upcCode = "123";

        var productsInDb = new List<ProductDto>
        {
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = productsInDb
            });

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<ProductScrapperResponse>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Not found"
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Not Found"
            }
        );

        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetProductByUpcAsync_GetByName_ReturnsError()
    {
        // Arrange
        var upcCode = "123";

        var productsInDb = new List<ProductDto>();

        var productScrapperResponse = new ProductScrapperResponse()
        {
            UpcCode = upcCode,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
        };
        productScrapperResponse.Ranges.AddRange(
            new List<string>()
            {
                "Men", "Women"
            }
        );

        var brand = new BrandDto()
        {
            Id = 1,
            Name = "NorthFace",
            Country = "USA",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating",
            Categories = new List<string> { "Category" },
            Ranges = new List<string> { "Range" }
        };

        var product = new ProductDto()
        {
            Id = 1,
            Name = "Etip Hardface Glove",
            UpcCode = upcCode,
            Category = "Category",
            Ranges = new List<string> { "Range" },
            BrandId = 1
        };

        var productScores = new ProductScoreDto
        {
            Moral = 1,
            Animal = 1,
            Environmental = 1
        };

        var productInformation = new ProductInformationDto()
        {
            Name = "Etip Hardface Glove",
            Country = "USA",
            Image = "No image found",
            GlobalScore = (productScores.Animal + productScores.Environmental + productScores.Moral) / 3,
            Scores = productScores,
            Composition = new List<ProductCompositionDto>(),
            Brand = "NorthFace"
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>())).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = new List<ProductDto>()
            }
        );

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<ProductScrapperResponse>()
            {
                Status = HttpStatusCode.OK,
                Object = productScrapperResponse
            });

        _brandDataMock.Setup(x => x.GetBrandByName(It.IsAny<string>())).Returns(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound
            });


        var productBusiness =
            new BrandAndProduct.Service.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object, _brandDataMock.Object, _filterFactoryMock.Object,
                _integrationEventRepositoryMock.Object, _integrationEventSenderServiceMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}