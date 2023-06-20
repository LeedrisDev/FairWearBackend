using System.Net;
using FairWearGateway.API.Models.Response;
using FairWearGateway.API.Utils.HttpClientWrapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace FairWearGateway.API.Tests.DataAccess.BrandData;

[TestClass]
public class BrandDataTester
{
    private Mock<IHttpClientWrapper> _httpClientWrapperMock;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
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

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Content = new StringContent(ToJsonString(brandResponse));

        _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
        var result = await brandData.GetBrandByIdAsync(brandId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandResponse);
    }

    // [TestMethod]
    // public async Task GetBrandByIdAsync_ReturnsErrorResponse()
    // {
    //     // Arrange
    //     var brandId = 1;
    //
    //     var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
    //     httpResponseMessage.ReasonPhrase = "Brand not found";
    //
    //     _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
    //         .ReturnsAsync(httpResponseMessage);
    //
    //     // Act
    //     var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
    //     var result = await brandData.GetBrandByIdAsync(brandId);
    //
    //     // Assert
    //     result.Status.Should().Be(HttpStatusCode.NotFound);
    //     result.ErrorMessage.Should().Be("Brand not found");
    // }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandResponse()
    {
        // Arrange
        var brandName = "Nike";
        var brandResponse = new BrandResponse
        {
            Id = 1,
            Name = brandName,
            Country = "USA"
            // Set other properties as needed
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Content = new StringContent(ToJsonString(brandResponse));

        _httpClientWrapperMock.Setup(m => m.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
        var result = await brandData.GetBrandByNameAsync(brandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsErrorResponse()
    {
        // Arrange
        var brandName = "Nike";

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        httpResponseMessage.ReasonPhrase = "Invalid request";

        _httpClientWrapperMock.Setup(m => m.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
            .ReturnsAsync(httpResponseMessage);

        // Act
        var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
        var result = await brandData.GetBrandByNameAsync(brandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.BadRequest);
        result.ErrorMessage.Should().Be("Invalid request");
    }

    private string ToJsonString<T>(T obj) where T : class
    {
        return JsonConvert.SerializeObject(obj);
    }
}