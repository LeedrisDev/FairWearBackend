using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FluentAssertions;
using Grpc.Core;
using Moq;

namespace FairWearGateway.API.Tests.DataAccess.BrandData;

// [TestClass]
public class BrandDataTests
{
    private API.DataAccess.BrandData.BrandData brandData;
    private Mock<BrandService.BrandServiceClient> mockClient;

    [TestInitialize]
    public void TestInitialize()
    {
        mockClient = new Mock<BrandService.BrandServiceClient>();
        brandData = new API.DataAccess.BrandData.BrandData { };
    }

    [TestMethod]
    public void GetBrandById_WhenRpcExceptionIsThrown_ReturnsErrorResponse()
    {
        // Arrange
        var brandId = 123;
        var expectedErrorMessage = "Brand with id 123 could not be found";
        mockClient.Setup(c => c.GetBrandByIdAsync(It.IsAny<BrandByIdRequest>(), null, null, default))
            .Throws(new RpcException(new Status(StatusCode.NotFound, "Not Found")));

        // Act
        var response = brandData.GetBrandById(brandId);

        // Assert
        response.Status.Should().Be(HttpStatusCode.NotFound);
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Object.Should().BeNull();
    }

    [TestMethod]
    public void GetBrandByName_WhenRpcExceptionIsThrown_ReturnsErrorResponse()
    {
        // Arrange
        var brandName = "SomeBrand";
        var expectedErrorMessage = "Brand with name SomeBrand could not be found";
        mockClient.Setup(c => c.GetBrandByNameAsync(It.IsAny<BrandByNameRequest>(), null, null, default))
            .Throws(new RpcException(new Status(StatusCode.NotFound, "Not Found")));

        // Act
        var response = brandData.GetBrandByName(brandName);

        // Assert
        response.Status.Should().Be(HttpStatusCode.NotFound);
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Object.Should().BeNull();
    }
}

// [TestClass]
// public class BrandDataTester
// {
//     private Mock<IHttpClientWrapper> _httpClientWrapperMock;
//
//     [TestInitialize]
//     public void Initialize()
//     {
//         _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
//     }
//
//     [TestMethod]
//     public async Task GetBrandByIdAsync_ReturnsBrandResponse()
//     {
//         // Arrange
//         var brandId = 1;
//         var brandResponse = new BrandResponse
//         {
//             Id = brandId,
//             Name = "Nike",
//             Country = "USA",
//             EnvironmentRating = 5,
//             PeopleRating = 4,
//             AnimalRating = 3,
//             RatingDescription = "Description 1",
//         };
//         
//         brandResponse.Categories.AddRange(new List<string> { "Category 1", "Category 2" });
//         brandResponse.Ranges.AddRange(new List<string> { "Range 1", "Range 2" });
//         var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
//         httpResponseMessage.Content = new StringContent(ToJsonString(brandResponse));
//
//         _httpClientWrapperMock.Setup(m => m.GetAsync(It.IsAny<string>()))
//             .ReturnsAsync(httpResponseMessage);
//
//         // Act
//         var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
//         var result = await brandData.GetBrandByIdAsync(brandId);
//
//         // Assert
//         result.Status.Should().Be(HttpStatusCode.OK);
//         result.Object.Should().BeEquivalentTo(brandResponse);
//     }
//
//
//     [TestMethod]
//     public async Task GetBrandByNameAsync_ReturnsBrandResponse()
//     {
//         // Arrange
//         var brandName = "Nike";
//         var brandResponse = new BrandResponse
//         {
//             Id = 1,
//             Name = brandName,
//             Country = "USA"
//             // Set other properties as needed
//         };
//
//         var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
//         httpResponseMessage.Content = new StringContent(ToJsonString(brandResponse));
//
//         _httpClientWrapperMock.Setup(m => m.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
//             .ReturnsAsync(httpResponseMessage);
//
//         // Act
//         var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
//         var result = await brandData.GetBrandByNameAsync(brandName);
//
//         // Assert
//         result.Status.Should().Be(HttpStatusCode.OK);
//         result.Object.Should().BeEquivalentTo(brandResponse);
//     }
//
//     [TestMethod]
//     public async Task GetBrandByNameAsync_ReturnsErrorResponse()
//     {
//         // Arrange
//         var brandName = "Nike";
//
//         var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
//         httpResponseMessage.ReasonPhrase = "Invalid request";
//
//         _httpClientWrapperMock.Setup(m => m.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
//             .ReturnsAsync(httpResponseMessage);
//
//         // Act
//         var brandData = new API.DataAccess.BrandData.BrandData(_httpClientWrapperMock.Object);
//         var result = await brandData.GetBrandByNameAsync(brandName);
//
//         // Assert
//         result.Status.Should().Be(HttpStatusCode.BadRequest);
//         result.ErrorMessage.Should().Be("Invalid request");
//     }
//
//     private string ToJsonString<T>(T obj) where T : class
//     {
//         return JsonConvert.SerializeObject(obj);
//     }
// }