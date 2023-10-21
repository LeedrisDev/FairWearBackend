using System.Net;
using BrandAndProduct.Service.DataAccess.BrandData;
using BrandAndProduct.Service.DataAccess.Filters;
using BrandAndProduct.Service.DataAccess.IRepositories;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using FluentAssertions;
using Moq;

namespace BrandAndProduct.Service.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private BrandAndProduct.Service.Business.BrandBusiness.BrandBusiness _brandBusiness = null!;
    private Mock<IBrandData> _brandDataMock = null!;
    private Mock<IBrandRepository> _brandRepositoryMock = null!;
    private Mock<IFilterFactory<IFilter>> _filterFactoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _brandDataMock = new Mock<IBrandData>();
        _filterFactoryMock = new Mock<IFilterFactory<IFilter>>();
        _brandBusiness = new BrandAndProduct.Service.Business.BrandBusiness.BrandBusiness(
            _brandRepositoryMock.Object, _brandDataMock.Object, _filterFactoryMock.Object);
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
                Country = "Country A",
                EnvironmentRating = 2,
                PeopleRating = 2,
                AnimalRating = 2,
                RatingDescription = "Rating 2"
            },
            new BrandDto
            {
                Id = 2,
                Name = "Brand 2",
                Country = "Country B",
                EnvironmentRating = 3,
                PeopleRating = 3,
                AnimalRating = 3,
                RatingDescription = "Rating 3"
            }
        };

        _brandRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<GenericFilter<IFilter>>()))
            .ReturnsAsync(new ProcessingStatusResponse<IEnumerable<BrandDto>> { Object = brandsInDb });

        // Act
        var filterDict = new Dictionary<string, string>();
        var result = await _brandBusiness.GetAllBrandsAsync(filterDict);

        // Assert
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
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto> { Object = brandInDb });

        // Act
        var result = await _brandBusiness.GetBrandByIdAsync(brandInDb.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandInDb);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsBrandDoesNotExists()
    {
        // Arrange

        _brandRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto> { Status = HttpStatusCode.NotFound });

        // Act
        var result = await _brandBusiness.GetBrandByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandIfExistsInRepository()
    {
        // Arrange
        var brandName = "BrandName";
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        var brandInDb = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        _brandRepositoryMock.Setup(x => x.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto> { Object = brandInDb });

        // Act
        var result = await _brandBusiness.GetBrandByNameAsync(brandName);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandInDb);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_CreatesBrandIfNotExistsInRepository()
    {
        // Arrange
        var brandName = "BrandName";
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        _brandRepositoryMock.Setup(x => x.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto> { Status = HttpStatusCode.NotFound });

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.OK,
            Object = new BrandDto
            {
                Id = 1,
                Name = treatedBrandName,
                Country = "Country A",
                EnvironmentRating = 2,
                PeopleRating = 2,
                AnimalRating = 2,
                RatingDescription = "Rating 2"
            }
        };

        _brandDataMock.Setup(x => x.GetBrandByName(It.IsAny<string>()))
            .Returns(brandDataResponse);

        _brandRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BrandDto>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.GetBrandByNameAsync(brandName);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandDataResponse.Object);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsNotFound()
    {
        // Arrange
        var brandName = "BrandName";

        _brandRepositoryMock.Setup(x => x.GetBrandByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProcessingStatusResponse<BrandDto> { Status = HttpStatusCode.NotFound });

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.NotFound,
        };

        _brandDataMock.Setup(x => x.GetBrandByName(It.IsAny<string>()))
            .Returns(brandDataResponse);
        // Act
        var result = await _brandBusiness.GetBrandByNameAsync(brandName);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateBrand_ReturnsSuccess()
    {
        // Arrange
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        var brand = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.OK,
            Object = brand
        };

        _brandRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BrandDto>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.CreateBrandAsync(brand);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandDataResponse.Object);
    }

    [TestMethod]
    public async Task CreateBrand_ReturnsError()
    {
        // Arrange
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        var brand = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.InternalServerError,
        };

        _brandRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BrandDto>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.CreateBrandAsync(brand);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.Object.Should().BeEquivalentTo(brandDataResponse.Object);
    }

    [TestMethod]
    public async Task UpdateBrand_ReturnsSuccess()
    {
        // Arrange
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        var brand = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        var updatedBrand = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Updated"
        };

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.OK,
            Object = updatedBrand
        };

        _brandRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<BrandDto>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.UpdateBrandAsync(brand);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandDataResponse.Object);
    }

    [TestMethod]
    public async Task UpdateBrand_ReturnsNotFound()
    {
        // Arrange
        var treatedBrandName = "brandname"; // Treatment applied to the input brand name

        var brand = new BrandDto
        {
            Id = 1,
            Name = treatedBrandName,
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.NotFound,
        };

        _brandRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<BrandDto>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.UpdateBrandAsync(brand);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task DeleteBrand_ReturnsSuccess()
    {
        // Arrange

        var brand = new BrandDto
        {
            Id = 1,
            Name = "Nike",
            Country = "Country A",
            EnvironmentRating = 2,
            PeopleRating = 2,
            AnimalRating = 2,
            RatingDescription = "Rating 2"
        };

        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.OK,
            Object = brand
        };

        _brandRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.DeleteBrandAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandDataResponse.Object);
    }

    [TestMethod]
    public async Task DeleteBrand_ReturnsNotFound()
    {
        // Arrange
        var brandDataResponse = new ProcessingStatusResponse<BrandDto>
        {
            Status = HttpStatusCode.NotFound,
        };

        _brandRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(brandDataResponse);

        // Act
        var result = await _brandBusiness.DeleteBrandAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(HttpStatusCode.NotFound);
    }
}