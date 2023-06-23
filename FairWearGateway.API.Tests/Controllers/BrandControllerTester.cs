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
}