using System.Net;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Utils;
using BrandAndProductDatabase.API.Utils.HttpClientWrapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace BrandAndProductDatabase.API.Tests.DataAccess.BrandData;

[TestClass]
public class BrandDataTester
{
    private API.DataAccess.BrandData.BrandData _brandData;
    private Mock<IHttpClientWrapper> _httpClientWrapperMock;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
        _brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandDto()
    {
        // Arrange
        var expectedName = "ExampleBrand";
        var expectedBrandDto = new BrandDto
        {
            Id = 1,
            Name = expectedName,
            Country = "USA",
            EnvironmentRating = 5,
            PeopleRating = 4,
            AnimalRating = 3,
            RatingDescription = "Description 1",
            Categories = new List<string> { "Category 1", "Category 2" },
            Ranges = new List<string> { "Range 1", "Range 2" }
        };

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedBrandDto))
        };

        _httpClientWrapperMock
            .Setup(mock => mock.PostAsync(
                AppConstants.GoodOnYouScrapperUrl,
                It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _brandData.GetBrandByNameAsync(expectedName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(expectedBrandDto);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsError()
    {
        // Arrange
        var expectedName = "ExampleBrand";

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        _httpClientWrapperMock
            .Setup(mock => mock.PostAsync(
                AppConstants.GoodOnYouScrapperUrl,
                It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _brandData.GetBrandByNameAsync(expectedName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsErrorForDeserializationFailure()
    {
        // Arrange  
        var expectedName = "ExampleBrand";

        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
        };

        _httpClientWrapperMock
            .Setup(mock => mock.PostAsync(
                AppConstants.GoodOnYouScrapperUrl,
                It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _brandData.GetBrandByNameAsync(expectedName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("Error deserializing brand.");
    }
}