using System.Net;
using FairWearGateway.API.DataAccess.BrandData;
using FairWearGateway.API.Models;
using FairWearGateway.API.Models.Response;
using FluentAssertions;
using Moq;

namespace FairWearGateway.API.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private Mock<IBrandData> _brandDataMock;

    [TestInitialize]
    public void Initialize()
    {
        _brandDataMock = new Mock<IBrandData>();
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsBrandResponse()
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

        _brandDataMock.Setup(m => m.GetBrandByIdAsync(brandId))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandDataMock.Object);
        var result = await brandBusiness.GetBrandByIdAsync(brandId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandResponse()
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

        _brandDataMock.Setup(m => m.GetBrandByNameAsync(brandName))
            .ReturnsAsync(processingStatusResponse);

        // Act
        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandDataMock.Object);
        var result = await brandBusiness.GetBrandByNameAsync(brandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandResponse);
    }
}