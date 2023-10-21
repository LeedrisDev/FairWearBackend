using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using GoodOnYouScrapper.Service.Protos;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Moq;

namespace BrandAndProduct.Service.Tests.DataAccess.BrandData;

[TestClass]
public class BrandDataTester
{
    private BrandAndProduct.Service.DataAccess.BrandData.BrandData _brandData;
    private IMapper _mapper;
    private Mock<BrandScrapperService.BrandScrapperServiceClient> _mockBrandServiceClient;
    private Mock<GrpcClientFactory> _mockGrpcClientFactory;


    [TestInitialize]
    public void Initialize()
    {
        _mockGrpcClientFactory = new Mock<GrpcClientFactory>();
        _mockBrandServiceClient = new Mock<BrandScrapperService.BrandScrapperServiceClient>();
        _mockGrpcClientFactory
            .Setup(f => f.CreateClient<BrandScrapperService.BrandScrapperServiceClient>("BrandService"))
            .Returns(_mockBrandServiceClient.Object);

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _brandData = new BrandAndProduct.Service.DataAccess.BrandData.BrandData(_mockGrpcClientFactory.Object, _mapper);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ReturnsBrandDto()
    {
        // Arrange
        var expectedName = "ExampleBrand";
        var expectedBrandDto = new BrandDto
        {
            Id = 0,
            Name = expectedName,
            Country = "USA",
            EnvironmentRating = 5,
            PeopleRating = 4,
            AnimalRating = 3,
            RatingDescription = "Description 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        var brandResponse = new BrandScrapperResponse
        {
            Name = expectedName,
            Country = "USA",
            EnvironmentRating = 5,
            PeopleRating = 4,
            AnimalRating = 3,
            RatingDescription = "Description 1",
        };

        brandResponse.Categories.AddRange(new List<string> { "Category 1" });
        brandResponse.Ranges.AddRange(new List<string> { "Range 1" });

        _mockBrandServiceClient
            .Setup(c => c.GetBrand(It.IsAny<BrandScrapperRequest>(), null, null, new CancellationToken()))
            .Returns(brandResponse);

        // Act
        var result = _brandData.GetBrandByName(expectedName);

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

        _mockBrandServiceClient
            .Setup(c => c.GetBrand(It.IsAny<BrandScrapperRequest>(), null, null, new CancellationToken()))
            .Throws(new RpcException(new Status(StatusCode.Internal, "Internal Server Error")));

        // Act
        var result = _brandData.GetBrandByName(expectedName);

        // Assert
        result.Status.Should().Be(HttpStatusCode.InternalServerError);
    }
}