using AutoMapper;
using BrandAndProductDatabase.API.DataAccess.IRepositories;
using BrandAndProductDatabase.API.Models;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Utils;
using FluentAssertions;
using Moq;

namespace BrandAndProductDatabase.API.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private API.Business.BrandBusiness.BrandBusiness _brandBusiness;
    private Mock<IBrandRepository> _brandRepositoryMock;
    private IMapper _mapper;

    [TestInitialize]
    public void Initialize()
    {
        _brandRepositoryMock = new Mock<IBrandRepository>();

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });

        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public async Task GetAllBrandsAsync_ReturnsAllBrands()
    {
        // Arrange
        var brandsInDb = new List<BrandDto>
        {
            new BrandDto
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
            new BrandDto
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
            }
        };

        _brandRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(
            new ProcessingStatusResponse<IEnumerable<BrandDto>>()
            {
                Object = brandsInDb
            });

        var brandBusiness = new API.Business.BrandBusiness.BrandBusiness(_brandRepositoryMock.Object, _mapper);

        // Act
        var result = await brandBusiness.GetAllBrandsAsync();

        // // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(brandsInDb);
    }
}