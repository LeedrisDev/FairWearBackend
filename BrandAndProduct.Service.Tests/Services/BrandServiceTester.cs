using System.Net;
using AutoMapper;
using BrandAndProduct.Service.Business.BrandBusiness;
using BrandAndProduct.Service.Models;
using BrandAndProduct.Service.Models.Dto;
using BrandAndProduct.Service.Protos;
using BrandAndProduct.Service.Tests.TestHelpers;
using BrandAndProduct.Service.Utils;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using BrandService = BrandAndProduct.Service.Services.BrandService;

namespace BrandAndProduct.Service.Tests.Services;

[TestClass]
public class BrandServiceTester
{
    private Mock<IBrandBusiness> _brandBusinessMock;
    private BrandService _brandService;
    private CancellationTokenSource _cancellationTokenSource;
    private ServerCallContext _context;
    private Mock<ILogger<BrandService>> _logger;
    private IMapper _mapper;

    [TestInitialize]
    public void Initialize()
    {
        _brandBusinessMock = new Mock<IBrandBusiness>();
        _logger = new Mock<ILogger<BrandService>>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });
        _mapper = config.CreateMapper();
        _brandService = new BrandService(_brandBusinessMock.Object, _logger.Object, _mapper);
        _cancellationTokenSource = new CancellationTokenSource();

        _context = TestServerCallContext.Create(
            method: nameof(BrandService)
            , host: "localhost"
            , deadline: DateTime.Now.AddMinutes(30)
            , requestHeaders: new Metadata()
            , cancellationToken: _cancellationTokenSource.Token
            , peer: "10.0.0.25:5001"
            , authContext: null
            , contextPropagationToken: null
            , writeHeadersFunc: (metadata) => Task.CompletedTask
            , writeOptionsGetter: () => new WriteOptions()
            , writeOptionsSetter: (writeOptions) => { }
        );
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
            },
        };

        expected[0].Categories.AddRange(new List<string> { "Category 1" });
        expected[0].Ranges.AddRange(new List<string> { "Range 1" });

        expected[1].Categories.AddRange(new List<string> { "Category 2" });
        expected[1].Ranges.AddRange(new List<string> { "Range 2" });

        _brandBusinessMock.Setup(b => b.GetAllBrandsAsync(It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(brandList);

        var request = new BrandFilterList();
        //

        var responseStream = new TestServerStreamWriter<BrandResponse>(_context);

        // Act
        using var call = _brandService.GetAllBrandsAsync(request, responseStream, _context);
        call.IsCompletedSuccessfully.Should().BeTrue(); //Method should run until cancelled.
        _cancellationTokenSource.Cancel();

        await call;
        responseStream.Complete();

        var allBrands = new List<BrandResponse>();
        await foreach (var brand in responseStream.ReadAllAsync())
        {
            allBrands.Add(brand);
        }

        allBrands.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetAllBrandsAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<IEnumerable<BrandDto>>()
        {
            Status = HttpStatusCode.InternalServerError,
            ErrorMessage = "Internal server error"
        };

        var request = new BrandFilterList();

        _brandBusinessMock.Setup(b => b.GetAllBrandsAsync(It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(businessResult);

        var responseStream = new TestServerStreamWriter<BrandResponse>(_context);

        // Act
        using var call = _brandService.GetAllBrandsAsync(request, responseStream, _context);
        call.IsCompletedSuccessfully.Should().BeFalse(); //Method should FAIL
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

        var request = new BrandByIdRequest()
        {
            Id = 1
        };

        // Act
        var result = await _brandService.GetBrandByIdAsync(request, _context);

        // Assert
        result.Should().BeEquivalentTo(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange

        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.NotFound,
            ErrorMessage = "Brand not found"
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResponse);

        var request = new BrandByIdRequest()
        {
            Id = 1
        };

        // Act
        try
        {
            var result = await _brandService.GetBrandByIdAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be("Brand not found");
        }
    }

    [TestMethod]
    public async Task GetBrandByIdAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.InternalServerError,
            ErrorMessage = "Internal server error"
        };

        _brandBusinessMock.Setup(b => b.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(businessResult);

        var request = new BrandByIdRequest()
        {
            Id = 1
        };

        // Act
        try
        {
            var result = await _brandService.GetBrandByIdAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("Internal server error");
        }
    }

    [TestMethod]
    public async Task CreateBrandAsync_ReturnsCreatedResultWithBrand_WhenBrandBusinessCreatesBrand()
    {
        // Arrange
        var expected = new BrandResponse
        {
            Id = 1,
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };

        expected.Categories.AddRange(new List<string> { "Category 1" });
        expected.Ranges.AddRange(new List<string> { "Range 1" });

        var createdBrand = new BrandDto
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
        };

        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Object = createdBrand,
            Status = HttpStatusCode.OK
        };

        _brandBusinessMock.Setup(x => x.CreateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(businessResult);

        var brandRequest = new BrandRequest
        {
            Name = "Brand 1",
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };
        brandRequest.Categories.AddRange(new List<string> { "Category 1" });
        brandRequest.Ranges.AddRange(new List<string> { "Range 1" });

        // Act
        var result = await _brandService.CreateBrandAsync(brandRequest, _context);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    public async Task CreateBrandAsync_ReturnsBadRequestResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var brandRequest = new BrandRequest
        {
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };

        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.BadRequest
        };

        _brandBusinessMock.Setup(x => x.CreateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(businessResponse);

        // Act
        try
        {
            var result = await _brandService.CreateBrandAsync(brandRequest, _context);
        }
        catch (RpcException e)
        {
            // Assert

            e.Status.StatusCode.Should().Be(StatusCode.Internal);
        }
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsUpdatedBrand_WhenBrandIsValid()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };

        brand.Categories.AddRange(new List<string> { "Category 1" });
        brand.Ranges.AddRange(new List<string> { "Range 1" });


        var updatedBrand = new BrandResponse
        {
            Id = 1,
            Name = "Updated Brand",
            Country = "Updated Country",
            EnvironmentRating = 3,
            PeopleRating = 3,
            AnimalRating = 3,
            RatingDescription = "Updated Rating",
        };

        updatedBrand.Categories.AddRange(new List<string> { "Updated Category" });
        updatedBrand.Ranges.AddRange(new List<string> { "Updated Range" });

        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = _mapper.Map<BrandDto>(updatedBrand),
                Status = HttpStatusCode.OK
            });

        // Act
        var result = await _brandService.UpdateBrandAsync(brand, _context);

        // Assert
        result.Should().Be(updatedBrand);
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsNotFound_WhenBrandDoesNotExist()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };

        brand.Categories.AddRange(new List<string> { "Category 1" });
        brand.Ranges.AddRange(new List<string> { "Range 1" });


        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound,
                MessageObject = { Message = $"Brand with ID {brand.Id} not found." }
            });

        // Act
        try
        {
            var result = await _brandService.UpdateBrandAsync(brand, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be($"Brand with ID {brand.Id} not found.");
        }
    }

    [TestMethod]
    public async Task UpdateBrandAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        var brand = new BrandResponse
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
        };

        brand.Categories.AddRange(new List<string> { "Category 1" });
        brand.Ranges.AddRange(new List<string> { "Range 1" });

        _brandBusinessMock.Setup(x => x.UpdateBrandAsync(It.IsAny<BrandDto>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.InternalServerError,
            });

        // Act
        try
        {
            var result = await _brandService.UpdateBrandAsync(brand, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
        }
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsNoContentResult_WhenBrandIsDeleted()
    {
        // Arrange
        const int brandId = 1;

        var brand = new BrandDto()
        {
            Id = 1,
            Country = "Country 1",
            EnvironmentRating = 1,
            PeopleRating = 1,
            AnimalRating = 1,
            RatingDescription = "Rating 1",
            Categories = new List<string> { "Category 1" },
            Ranges = new List<string> { "Range 1" }
        };

        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Object = brand,
                Status = HttpStatusCode.OK
            });

        _brandBusinessMock.Setup(x => x.DeleteBrandAsync(It.IsAny<int>())).ReturnsAsync(
            new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.OK
            });

        var request = new BrandByIdRequest()
        {
            Id = brandId
        };

        // Act
        var result = _brandService.DeleteBrandAsync(request, _context);

        // Assert
        result.IsCompletedSuccessfully.Should().BeTrue();
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsNotFoundResult_WhenBrandDoesNotExist()
    {
        // Arrange
        const int brandId = 1;
        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            (new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.NotFound
            }));

        var request = new BrandByIdRequest()
        {
            Id = brandId
        };

        // Act
        try
        {
            var result = await _brandService.DeleteBrandAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }
    }

    [TestMethod]
    public async Task DeleteBrandAsync_ReturnsErrorStatusCode_WhenBrandBusinessFails()
    {
        // Arrange
        const int brandId = 1;
        _brandBusinessMock.Setup(x => x.GetBrandByIdAsync(It.IsAny<int>())).ReturnsAsync(
            (new ProcessingStatusResponse<BrandDto>()
            {
                Status = HttpStatusCode.InternalServerError
            }));

        var request = new BrandByIdRequest()
        {
            Id = brandId
        };

        // Act
        try
        {
            var result = await _brandService.DeleteBrandAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
        }
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_ExistingBrand_ReturnsOkResult()
    {
        // Arrange
        var brandName = "BrandName";
        var brandRequest = new BrandRequest { Name = brandName };

        var brandResponse = new BrandResponse
        {
            Id = 1,
            Name = brandName,
            Country = "Country",
            EnvironmentRating = 1,
            PeopleRating = 2,
            AnimalRating = 3,
            RatingDescription = "Rating"
        };

        brandResponse.Categories.AddRange(new List<string> { "Category" });
        brandResponse.Ranges.AddRange(new List<string> { "Range" });


        var businessResponse = new ProcessingStatusResponse<BrandDto>()
        {
            Object = new BrandDto()
            {
                Id = 1,
                Name = brandName,
                Country = "Country",
                EnvironmentRating = 1,
                PeopleRating = 2,
                AnimalRating = 3,
                RatingDescription = "Rating",
                Categories = new List<string>() { "Category" },
                Ranges = new List<string>() { "Range" }
            },
            Status = HttpStatusCode.OK
        };

        _brandBusinessMock.Setup(b => b.GetBrandByNameAsync(brandName))
            .ReturnsAsync(businessResponse);

        var request = new BrandByNameRequest()
        {
            Name = brandName
        };

        // Act
        var result = await _brandService.GetBrandByNameAsync(request, _context);

        // Assert
        result.Should().Be(brandResponse);
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_NonExistingBrand_ReturnsNotFoundResult()
    {
        // Arrange
        var brandName = "BrandName";
        var brandRequest = new BrandRequest { Name = brandName };

        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.NotFound,
            MessageObject = { Message = $"Brand with Name {brandName} not found." }
        };
        _brandBusinessMock.Setup(b => b.GetBrandByNameAsync(brandName))
            .ReturnsAsync(businessResult);

        var request = new BrandByNameRequest()
        {
            Name = brandName
        };

        // Act
        try
        {
            var result = await _brandService.GetBrandByNameAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.NotFound);
            e.Status.Detail.Should().Be($"Brand with Name {brandName} not found.");
        }
    }

    [TestMethod]
    public async Task GetBrandByNameAsync_Error_ReturnsStatusCodeResult()
    {
        // Arrange
        var brandName = "BrandName";
        var brandRequest = new BrandRequest { Name = brandName };

        var businessResult = new ProcessingStatusResponse<BrandDto>()
        {
            Status = HttpStatusCode.InternalServerError,
            MessageObject = { Message = "An error occurred." }
        };

        _brandBusinessMock.Setup(b => b.GetBrandByNameAsync(brandName))
            .ReturnsAsync(businessResult);

        var request = new BrandByNameRequest()
        {
            Name = brandName
        };

        // Act
        try
        {
            var result = await _brandService.GetBrandByNameAsync(request, _context);
        }
        catch (RpcException e)
        {
            // Assert
            e.Status.StatusCode.Should().Be(StatusCode.Internal);
            e.Status.Detail.Should().Be("An error occurred.");
        }
    }
}