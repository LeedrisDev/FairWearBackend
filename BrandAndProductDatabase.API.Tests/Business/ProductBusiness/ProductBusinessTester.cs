using System.Net;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;
using Moq;

namespace BrandAndProductDatabase.API.Tests.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTester
{
    private Mock<IBrandRepository> _brandRepositoryMock;
    private API.Business.ProductBusiness.ProductBusiness _productBusiness;
    private Mock<IProductRepository> _productRepositoryMock;

    [TestInitialize]
    public void Initialize()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _brandRepositoryMock = new Mock<IBrandRepository>();
    }

    [TestMethod]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
        var productsInDb = new List<ProductDto>
        {
            new ProductDto
            {
                Id = 1,
                Name = "Product 1",
                UpcCode = "123456789",
                Category = "Category A",
                Ranges = new List<string> { "Range A" },
                BrandId = 1,
            },
            new ProductDto
            {
                Id = 2,
                Name = "Product 2",
                UpcCode = "987654321",
                Category = "Category B",
                Ranges = new List<string> { "Range B" },
                BrandId = 2,
            }
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<ProductDto>>()
            {
                Object = productsInDb
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

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
            BrandId = 1,
        };

        _productRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<ProductDto>()
            {
                Object = productInDb
            });

        var productBusiness =
            new API.Business.ProductBusiness.ProductBusiness(_productRepositoryMock.Object,
                _brandRepositoryMock.Object);

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
                _brandRepositoryMock.Object);

        // Act
        var result = await productBusiness.DeleteProductAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}