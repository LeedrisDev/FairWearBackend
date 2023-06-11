using System.Net;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.DataAccess.ProductData;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;
using Moq;

namespace BrandAndProductDatabase.API.Tests.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTester
{
    private Mock<IBrandRepository> _brandRepositoryMock = null!;
    private API.Business.ProductBusiness.ProductBusiness _productBusiness = null!;
    private Mock<IProductData> _productDataMock = null!;
    private Mock<IProductRepository> _productRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _productDataMock = new Mock<IProductData>();
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

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>
            {
                Object = productsInDb
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

        // Act
        var result = await productBusiness.GetAllProductsAsync();

        // // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(productsInDb);
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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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
                Status = HttpStatusCode.OK
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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

        _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productToUpdate
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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

        _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProductDto>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Status = HttpStatusCode.NotFound
            });

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK
            });
        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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
        _productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
            _brandRepositoryMock.Object, _productDataMock.Object);
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
        _productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
            _brandRepositoryMock.Object, _productDataMock.Object);
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
        _productBusiness = new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
            _brandRepositoryMock.Object, _productDataMock.Object);

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
            Composition = Array.Empty<ProductCompositionDto>(),
            Alternatives = Array.Empty<string>(),
            Brand = "Brand 1"
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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

        var productRetrieverDto = new ProductRetrieverDto()
        {
            UpcCode = upcCode,
            BrandName = "NorthFace",
            Name = "Etip Hardface Glove",
            Category = "Gloves",
            Ranges = new List<string>()
            {
                "Men", "Women"
            }
        };

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
            Composition = Array.Empty<ProductCompositionDto>(),
            Alternatives = Array.Empty<string>(),
            Brand = "NorthFace"
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = new List<ProductDto>()
            }
        );

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductRetrieverDto>()
            {
                Status = HttpStatusCode.OK,
                Object = productRetrieverDto
            });

        _brandRepositoryMock.Setup(x => x.GetBrandByNameAsync(It.IsAny<string>())).ReturnsAsync(
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
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

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

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = productsInDb
            });

        _productDataMock.Setup(x => x.GetProductByUpc(It.IsAny<string>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductRetrieverDto>()
            {
                Status = HttpStatusCode.NotFound
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object, _productDataMock.Object);

        // Act
        var result = await productBusiness.GetProductByUpcAsync(upcCode);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}