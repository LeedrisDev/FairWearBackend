using System.Net;
using FluentAssertions;
using GoodOnYouScrapper.Service.Business.BrandBusiness;
using GoodOnYouScrapper.Service.Models;
using GoodOnYouScrapper.Service.Protos;
using Grpc.Core;
using Grpc.Core.Testing;
using Moq;
using BrandScrapperService = GoodOnYouScrapper.Service.Services.BrandScrapperService;

namespace GoodOnYouScrapper.Service.Tests.Services;

[TestClass]
public class BrandScrapperServiceTester
{
    private Mock<IBrandBusiness> _brandBusinessMock = null!;
    private BrandScrapperService _brandScrapperService = null!;
    private ServerCallContext _context = null!;


    [TestInitialize]
    public void Initialize()
    {
        _brandBusinessMock = new Mock<IBrandBusiness>();
        _brandScrapperService = new BrandScrapperService(_brandBusinessMock.Object);
        _context = TestServerCallContext.Create(
            method: nameof(BrandScrapperService)
            , host: "localhost"
            , deadline: DateTime.Now.AddMinutes(30)
            , requestHeaders: new Metadata()
            , cancellationToken: CancellationToken.None
            , peer: "10.0.0.25:5001"
            , authContext: null
            , contextPropagationToken: null
            , writeHeadersFunc: (metadata) => Task.CompletedTask
            , writeOptionsGetter: () => new WriteOptions()
            , writeOptionsSetter: (writeOptions) => { }
        );
    }

    [TestMethod]
    public async Task GetBrand_Returns_Ok_With_Brand_Information()
    {
        // Arrange
        var brand = new BrandScrapperRequest { Name = "NorthFace" };
        var brandInformation = new BrandScrapperResponse
        {
            Name = brand.Name,
            Country = "United States",
            EnvironmentRating = 5,
            PeopleRating = 5,
            AnimalRating = 5,
            RatingDescription = "Description",
        };
        brandInformation.Categories.AddRange(new List<string>()
        {
            "Shoes", "Shirt"
        });

        brandInformation.Ranges.AddRange(new List<string>()
        {
            "Men", "Women"
        });

        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(
            new ProcessingStatusResponse<BrandScrapperResponse>
            {
                Status = HttpStatusCode.OK,
                Object = brandInformation
            });

        // Act

        var response = await _brandScrapperService.GetBrand(brand, _context);

        // Assert

        response.Should().Be(brandInformation);
    }

    [TestMethod]
    public async Task GetBrand_Returns_NotFound_When_Brand_Is_Not_Found()
    {
        // Arrange
        var brand = new BrandScrapperRequest { Name = "NorthFace" };

        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(
            new ProcessingStatusResponse<BrandScrapperResponse>
            {
                Status = HttpStatusCode.NotFound,
                ErrorMessage = "Brand not found"
            });

        // Act
        try
        {
            await _brandScrapperService.GetBrand(brand, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be("Brand not found");
        }
    }

    [TestMethod]
    public async Task GetBrand_Returns_InternalServerError_When_An_Error_Occurs()
    {
        // Arrange
        var brand = new BrandScrapperRequest() { Name = "NorthFace" };

        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(
            new ProcessingStatusResponse<BrandScrapperResponse>
            {
                Status = HttpStatusCode.InternalServerError,
                ErrorMessage = "An error occurred while retrieving brand formation."
            });

        // Act
        try
        {
            await _brandScrapperService.GetBrand(brand, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred while retrieving brand formation.");
        }
    }
}