using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FluentAssertions;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Moq;

namespace FairWearGateway.API.Tests.DataAccess.BrandData;

[TestClass]
public class BrandDataTester
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
        _brandData = new API.DataAccess.BrandData.BrandData();
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

    [TestMethod]
    public async Task GetAllBrands_ReturnsCorrectBrands()
    {
        // Arrange

        var expectedBrandResponses = new List<BrandResponse>
        {
            new BrandResponse
            {
                /* set properties as needed */
            },
            new BrandResponse
            {
                /* set properties as needed */
            }
        };

        var asyncResponseCallMock = new Mock<IAsyncStreamReader<BrandResponse>>();
        var moveNextResponses = new Queue<bool>();
        foreach (var _ in expectedBrandResponses)
        {
            moveNextResponses.Enqueue(true);
        }

        moveNextResponses.Enqueue(false); // End of enumeration

        asyncResponseCallMock.Setup(a => a.MoveNext(CancellationToken.None))
            .ReturnsAsync(moveNextResponses.Dequeue);

        int index = 0;
        asyncResponseCallMock.Setup(a => a.Current)
            .Returns(() => expectedBrandResponses[index++]);

        var asyncResponse =
            new AsyncServerStreamingCall<BrandResponse>(asyncResponseCallMock.Object, null, null, null, null);

        _mockBrandServiceClient.Setup(b => b.GetAllBrandsAsync(It.IsAny<BrandFilterList>(), null, default, default))
            .Returns(asyncResponse);


        // Act
        var actualBrandResponse = await _brandData.GetAllBrands(new Dictionary<string, string>());

        // Assert
        actualBrandResponse.Status.Should().Be(HttpStatusCode.OK);
        actualBrandResponse.Object.Should().BeEquivalentTo(expectedBrandResponses);
    }

    [TestMethod]
    public async Task GetAllBrands_ReturnsInternalServerError_WhenRpcFails()
    {
        // Arrange
        var rpcException = new RpcException(new Status(StatusCode.Internal, "Internal Server error"));

        _mockBrandServiceClient
            .Setup(b => b.GetAllBrandsAsync(It.IsAny<BrandFilterList>(), null, default, default))
            .Throws(rpcException);

        // Act
        var actualResponse = await _brandData.GetAllBrands(new Dictionary<string, string>());

        // Assert
        actualResponse.Status.Should().Be(HttpStatusCode.InternalServerError);
    }
}