using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.BrandBusiness;
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
public class BrandControllerTester
{
    private Mock<IBrandBusiness> _brandBusinessMock;
    private BrandController _controller;
    private IMapper _mapper;

    [TestInitialize]
    public void Initialize()
    {
        _brandBusinessMock = new Mock<IBrandBusiness>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _controller = new BrandController(_brandBusinessMock.Object, _mapper);
    }

    [TestMethod]
    public async Task GetAllBrandsAsync_ReturnsOk_WithBrandResponseList()
    {
        // Arrange
        var brandList = new ProcessingStatusResponse<IEnumerable<BrandDto>>()
        {
            Object = new List<BrandDto>()
            {
                new BrandDto()
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
                new BrandDto()
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
                },
            },
            Status = HttpStatusCode.OK
        };

        var expected = new List<BrandResponse>()
        {
            new BrandResponse()
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
            new BrandResponse()
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
            },
        };

        _brandBusinessMock.Setup(x => x.GetAllBrandsAsync()).ReturnsAsync(brandList);

        // Act
        var result = await _controller.GetAllBrandsAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;

        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetAllBrandsAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<IEnumerable<BrandDto>>()
        {
            Status = HttpStatusCode.InternalServerError
        };

        _brandBusinessMock.Setup(x => x.GetAllBrandsAsync()).ReturnsAsync(businessResult);

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetAllBrandsAsync();

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }


    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsOk_WhenBrandExists()
    {
        // Arrange
        int brandId = 1;

        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Object = new BrandDto()
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
            Status = HttpStatusCode.OK
        };


        var brandResponse = _mapper.Map<BrandResponse>(businessResponse.Object);

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetBrandByIdAsync(brandId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult?.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult?.Value.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange

        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.NotFound
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);
        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetBrandByIdAsync(1);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.InternalServerError
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResult);

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.GetBrandByIdAsync(1);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task CreateBrandAsync_ReturnsCreatedResultWithBrand_WhenBrandBusinessCreatesBrand()
    {
        // Arrange
        var brand = new BrandResponse
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
        var createdBrand = new BrandDto
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
        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Object = createdBrand,
            Status = HttpStatusCode.OK
        };

        _brandBusinessMock.Setup(x => x.CreateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(businessResult);

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.CreateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var createdResult = (OkObjectResult)result;
        createdResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        createdResult.Value.Should().BeEquivalentTo(_mapper.Map<BrandResponse>(createdBrand),
            options => options.Excluding(x => x.Id));
    }

    [TestMethod]
    public async Task CreateBrandAsync_ReturnsBadRequestResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.BadRequest
        };

        _brandBusinessMock.Setup(x => x.CreateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(businessResponse);
        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.CreateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var badRequestResult = (ObjectResult)result;
        badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task CreateBrandAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.InternalServerError
        };

        _brandBusinessMock.Setup(x => x.CreateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(businessResult);

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.CreateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsUpdatedBrand_WhenBrandIsValid()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };


        var updatedBrand = new BrandResponse
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

        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = _mapper.Map<BrandDto>(updatedBrand),
                Status = HttpStatusCode.OK
            });

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.UpdateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(_mapper.Map<BrandResponse>(updatedBrand));
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
                MessageObject = { Message = $"Brand with ID {brand.Id} not found."}
            });

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.UpdateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        notFoundResult.Value.Should().BeEquivalentTo(new ErrorResponse()
        {
            Message = ($"Brand with ID {brand.Id} not found.")
        });
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var brand = new BrandResponse
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

        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.InternalServerError,
            });

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.UpdateBrandAsync(brand);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsNoContentResult_WhenBrandIsDeleted()
    {
        // Arrange
        const int brandId = 1;

        var brand = new BrandDto()
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brand,
                Status = HttpStatusCode.OK
            });

        _brandBusinessMock.Setup(x => x.DeleteBrandAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK
            });

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.DeleteBrandAsync(brandId);

        // Assert
        result.Should().BeOfType<OkResult>();
        var noContentResult = (OkResult)result;
        noContentResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsNotFoundResult_WhenBrandDoesNotExist()
    {
        // Arrange
        const int brandId = 1;
        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            (new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound
            }));

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.DeleteBrandAsync(brandId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        const int brandId = 1;
        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            (new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.InternalServerError
            }));

        var controller = new BrandController(_brandBusinessMock.Object, _mapper);

        // Act
        var result = await controller.DeleteBrandAsync(brandId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}