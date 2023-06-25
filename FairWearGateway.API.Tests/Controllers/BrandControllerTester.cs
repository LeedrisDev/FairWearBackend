using System.Net;
using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Request;
using FairWearGateway.API.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FairWearGateway.API.Tests.Controllers;

[TestClass]
public class BrandControllerTester
{
    private Mock<IBrandBusiness> _brandBusinessMock;

    [TestInitialize]
    public void Initialize()
    {
        _brandBusinessMock = new Mock<IBrandBusiness>();
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsOkResultWithBrandResponse()
    {
        // Arrange
        var brandId = 1;
        var brandResponse = new BrandResponse
        {
            Id = brandId,
            Name = "Nike",
            Country = "USA",
            EnvironmentRating = 5,
            PeopleRating = 4,
            AnimalRating = 3,
            RatingDescription = "Description 1",
            Categories = new List<string> { "Category 1", "Category 2" },
            Ranges = new List<string> { "Range 1", "Range 2" }
        };
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.OK,
            Object = brandResponse
        };

        _brandBusinessMock.Setup(m => m.GetBrandByIdAsync(brandId))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);
        var actionResult = await brandsController.GetBrandByIdAsync(brandId);
        var okResult = actionResult as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange

        var businessResponse = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.NotFound
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);

        // Act
        var result = await brandsController.GetBrandByIdAsync(1);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.InternalServerError
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResult);


        // Act
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);
        var result = await brandsController.GetBrandByIdAsync(1);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsOkResultWithBrandResponse()
    {
        // Arrange
        var brandName = "Nike";
        var brandResponse = new BrandResponse
        {
            Id = 1,
            Name = brandName,
            Country = "USA",
            EnvironmentRating = 5,
            PeopleRating = 4,
            AnimalRating = 3,
            RatingDescription = "Description 1",
            Categories = new List<string> { "Category 1", "Category 2" },
            Ranges = new List<string> { "Range 1", "Range 2" }
        };
        var processingStatusResponse = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.OK,
            Object = brandResponse
        };
        var brandRequest = new BrandRequest
        {
            Name = brandName
        };

        _brandBusinessMock.Setup(m => m.GetBrandByNameAsync(brandName))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);
        var actionResult = await brandsController.GetBrandByNameAsync(brandRequest);
        var okResult = actionResult as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_NonExistingBrand_ReturnsNotFoundResult()
    {
        // Arrange
        var brandName = "BrandName";
        var brandRequest = new BrandRequest { Name = brandName };

        var businessResult = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.NotFound,
            MessageObject = { Message = $"Brand with Name {brandName} not found." }
        };
        _brandBusinessMock.Setup(b => b.GetBrandByNameAsync(brandName))
            .ReturnsAsync(businessResult);

        // Act
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);
        var result = await brandsController.GetBrandByNameAsync(brandRequest);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = (NotFoundObjectResult)result;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        notFoundResult.Value.Should().BeEquivalentTo(new ErrorResponse()
        {
            Message = $"Brand with Name {brandName} not found."
        });
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        var brandName = "BrandName";
        var brandRequest = new BrandRequest { Name = brandName };

        var businessResult = new ProcessingStatusResponse<BrandResponse>()
        {
            Status = HttpStatusCode.InternalServerError,
            MessageObject = { Message = "An error occurred." }
        };

        _brandBusinessMock.Setup(b => b.GetBrandByNameAsync(brandName))
            .ReturnsAsync(businessResult);

        // Act
        var brandsController = new API.Controllers.BrandsController(_brandBusinessMock.Object);
        var result = await brandsController.GetBrandByNameAsync(brandRequest);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        objectResult.Value.Should().BeEquivalentTo(new ErrorResponse()
        {
            Message = "An error occurred."
        });
    }
}