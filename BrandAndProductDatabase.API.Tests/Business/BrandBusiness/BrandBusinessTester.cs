using System.Net;
using BrandAndProductDatabase.API.DataAccess.BrandData;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using FluentAssertions;
using Moq;

namespace BrandAndProductDatabase.API.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private API.Business.BrandBusiness.BrandBusiness _brandBusiness;
    private Mock<IBrandRepository> _brandRepositoryMock = null!;
    private Mock<IBrandData> _brandDataMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _brandDataMock = new Mock<IBrandData>();
    }

    [TestMethod]
    public async Task GetAllBrandsAsync_ReturnsAllBrands()
    {
        // Arrange
        var brandsInDb = new List<BrandDto>
        {
            new BrandDto
            {
                Id = 1,
                Name = "Brand 1",
                Country = "Country 1",
                EnvironmentRating = 1,
                PeopleRating = 1,
                AnimalRating = 1,
                RatingDescription = "Rating 1",
                Categories = new List<string> { "Category 1" },
                Ranges = new List<string> { "Range 1" }
            },
            new BrandDto
            {
                Id = 2,
                Name = "Brand 2",
                Country = "Country 2",
                EnvironmentRating = 2,
                PeopleRating = 2,
                AnimalRating = 2,
                RatingDescription = "Rating 2",
                Categories = new List<string> { "Category 2" },
                Ranges = new List<string> { "Range 2" }
            }
        };

        _brandRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<BrandDto>>
            {
                Object = brandsInDb
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.GetAllBrandsAsync();

        // // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandsInDb);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsBrandIfExists()
    {
        // Arrange
        var brandInDb = new BrandDto
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>
            {
                Object = brandInDb
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.GetBrandByIdAsync(brandInDb.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandInDb);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var brandId = 1;

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.GetBrandByIdAsync(brandId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateBrandAsync_WithValidBrandDto_CreatesBrand()
    {
        // Arrange
        var brandDto = new BrandDto
        {
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        var createdBrandInDb = new BrandDto
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = createdBrandInDb
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.CreateBrandAsync(brandDto);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(createdBrandInDb);
    }

    [TestMethod]
    public async Task UpdateBrandAsync_WithValidBrandDto_UpdatesBrand()
    {
        // Arrange
        var brandInDb = new BrandDto
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

        var brandToUpdate = new BrandDto
        {
            Id = 1,
            Name = "Updated Brand",
            Country = "Updated Country",
            EnvironmentRating = 3,
            PeopleRating = 3,
            AnimalRating = 3,
            RatingDescription = "Updated Rating",
            Categories = new List<string> { "Updated Category" },
            Ranges = new List<string> { "Updated Range" }
        };

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brandInDb
            });

        _brandRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brandToUpdate
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.UpdateBrandAsync(brandToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandToUpdate);
    }

    [TestMethod]
    public async Task UpdateBrandAsync_WithInvalidBrand_ReturnsNull()
    {
        // Arrange
        var brandToUpdate = new BrandDto
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.UpdateBrandAsync(brandToUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task DeleteBrandAsync_WithValidId_DeletesBrand()
    {
        // Arrange
        var brandInDb = new BrandDto
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

        _brandRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brandInDb
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.DeleteBrandAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandInDb);
    }

    [TestMethod]
    public async Task DeleteBrandAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange

        _brandRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _brandDataMock.Object);

        // Act
        var result = await brandBusiness.DeleteBrandAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
    
    
}