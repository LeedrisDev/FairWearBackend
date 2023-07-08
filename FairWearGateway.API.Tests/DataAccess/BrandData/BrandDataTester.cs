﻿using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FluentAssertions;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Moq;

namespace FairWearGateway.API.Tests.DataAccess.BrandData;

[TestClass]
public class BrandDataTests
{
    private API.DataAccess.BrandData.BrandData _brandData;
    private Mock<BrandService.BrandServiceClient> _mockBrandServiceClient;
    private Mock<GrpcClientFactory> _mockGrpcClientFactory;

    [TestInitialize]
    public void Setup()
    {
        _mockGrpcClientFactory = new Mock<GrpcClientFactory>();
        _mockBrandServiceClient = new Mock<BrandService.BrandServiceClient>();
        _mockGrpcClientFactory
            .Setup(f => f.CreateClient<BrandService.BrandServiceClient>("BrandService"))
            .Returns(_mockBrandServiceClient.Object);
        _brandData = new API.DataAccess.BrandData.BrandData(_mockGrpcClientFactory.Object);
    }

    [TestMethod]
    public async Task GetBrandById_Should_Return_Correct_Brand_When_Id_Exists()
    {
        // Arrange
        int testBrandId = 1;
        var testBrandResponse = new BrandResponse
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };
        testBrandResponse.Categories.AddRange(new List<string> { "Category 1" });
        testBrandResponse.Ranges.AddRange(new List<string> { "Range 1" });


        _mockBrandServiceClient
            .Setup(c => c.GetBrandByIdAsync(It.IsAny<BrandByIdRequest>(), null, null, new CancellationToken()))
            .Returns(testBrandResponse);

        // Act
        var result = _brandData.GetBrandById(testBrandId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(testBrandResponse);
    }

    [TestMethod]
    public async Task GetBrandById_Should_Return_NotFound_When_Id_Does_Not_Exists()
    {
        // Arrange
        int testBrandId = 1;

        _mockBrandServiceClient
            .Setup(c => c.GetBrandByIdAsync(It.IsAny<BrandByIdRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.NotFound,
                $"Brand with id {testBrandId} could not be found")));

        // Act
        var result = _brandData.GetBrandById(testBrandId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Brand with id {testBrandId} could not be found");
    }

    [TestMethod]
    public async Task GetBrandById_Should_Return_InternalServerError_On_Exception()
    {
        // Arrange
        int testBrandId = 1;

        _mockBrandServiceClient
            .Setup(c => c.GetBrandByIdAsync(It.IsAny<BrandByIdRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal,
                "Internal Server Error")));

        // Act
        var result = _brandData.GetBrandById(testBrandId);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("Internal Server Error");
    }

    [TestMethod]
    public async Task GetBrandByName_Should_Return_Correct_Brand_When_Name_Exists()
    {
        // Arrange
        string testBrandName = "Nike";
        var testBrandResponse = new BrandResponse
        {
            Id = 1,
            Name = "Nike",
            Country = "USA",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };
        testBrandResponse.Categories.AddRange(new List<string> { "Category 1" });
        testBrandResponse.Ranges.AddRange(new List<string> { "Range 1" });


        _mockBrandServiceClient
            .Setup(c => c.GetBrandByNameAsync(It.IsAny<BrandByNameRequest>(), null, null, new CancellationToken()))
            .Returns(testBrandResponse);

        // Act
        var result = _brandData.GetBrandByName(testBrandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.OK);
        result.Object.Should().BeEquivalentTo(testBrandResponse);
    }

    [TestMethod]
    public async Task GetBrandByName_Should_Return_NotFound_When_Name_Does_Not_Exists()
    {
        // Arrange
        string testBrandName = "UNKNOWN";

        _mockBrandServiceClient
            .Setup(c => c.GetBrandByNameAsync(It.IsAny<BrandByNameRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.NotFound,
                $"Brand with id {testBrandName} could not be found")));

        // Act
        var result = _brandData.GetBrandByName(testBrandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.NotFound);
        result.ErrorMessage.Should().Be($"Brand with name {testBrandName} could not be found");
    }

    [TestMethod]
    public async Task GetBrandByName_Should_Return_InternalServerError_On_Exception()
    {
        // Arrange
        string testBrandName = "UNKNOWN";

        _mockBrandServiceClient
            .Setup(c => c.GetBrandByNameAsync(It.IsAny<BrandByNameRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal,
                "Internal Server Error")));

        // Act
        var result = _brandData.GetBrandByName(testBrandName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorMessage.Should().Be("Internal Server Error");
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