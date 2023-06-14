using System.Net;
using FluentAssertions;
using GoodOnYouScrapper.API.Business.BrandBusiness;
using GoodOnYouScrapper.API.Controllers;
using GoodOnYouScrapper.API.Models;
using GoodOnYouScrapper.API.Models.Request;
using GoodOnYouScrapper.API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GoodOnYouScrapper.API.Tests.Controllers;

[TestClass]
public class BrandControllerTester
{
    private readonly Mock<IBrandBusiness> _brandBusinessMock;
    private readonly BrandController _brandController;

    public BrandControllerTester()
    {
        _brandBusinessMock = new Mock<IBrandBusiness>();
        _brandController = new BrandController(_brandBusinessMock.Object);
    }

    [TestMethod]
    public async Task GetBrand_Returns_Ok_With_Brand_Information()
    {
        // Arrange
        var brand = new BrandRequest { Name = "NorthFace"};
        var brandInformation = new BrandResponse
        {
            Name = brand.Name,
            Country = "United States",
            EnvironmentRating = 5,
            PeopleRating = 5,
            AnimalRating = 5,
            RatingDescription = "Description",
            Categories = new List<string>()
            {
                "Shoes", "Shirt"
            },
            
            Ranges = new List<string>()
            {
                "Men", "Women"
            }
        };

        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(new ProcessingStatusResponse<BrandResponse>
        {
            Status = HttpStatusCode.OK,
            Object = brandInformation
        });

        
        var response = await _brandController.GetBrand(brand);
        
        response.Should().BeOfType<OkObjectResult>();
        
        var result = response as OkObjectResult;
        result?.Value.Should().BeSameAs(brandInformation);
        
    }
    
    [TestMethod]
    public async Task GetBrand_Returns_NotFound_When_Brand_Is_Not_Found()
    {
        var brand = new BrandRequest { Name = "NorthFace"};
    
        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(new ProcessingStatusResponse<BrandResponse>
        {
            Status = HttpStatusCode.NotFound,
            ErrorMessage = "Brand not found"
        });
    
        var response = await _brandController.GetBrand(brand);
    
        response.Should().BeOfType<NotFoundObjectResult>();
        
        var result = response as NotFoundObjectResult;
        result.Should().NotBeNull();
        result?.Value.Should().Be("Brand not found");
    }
    
    [TestMethod]
    public async Task GetBrand_Returns_InternalServerError_When_An_Error_Occurs()
    {
        var brand = new BrandRequest { Name = "NorthFace"};
    
        _brandBusinessMock.Setup(x => x.GetBrandInformation(brand.Name)).ReturnsAsync(new ProcessingStatusResponse<BrandResponse>
        {
            Status = HttpStatusCode.InternalServerError,
            ErrorMessage = "An error occurred while retrieving brand formation."
        });
    
        var response = await _brandController.GetBrand(brand);
    
        var result = response as ObjectResult;
    
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}