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
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().BeEquivalentTo(brandResponse);
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
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}