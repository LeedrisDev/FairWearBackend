using System.ComponentModel.DataAnnotations;
using System.Net;
using GoodOnYouScrapper.API.Business.BrandBusiness;
using GoodOnYouScrapper.API.Models;
using GoodOnYouScrapper.API.Models.Request;
using GoodOnYouScrapper.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace GoodOnYouScrapper.API.Controllers;

/// <summary>Brand controller</summary>
[ApiController]
// [ApiVersion("1.0")]
[Route("api/[controller]")]
[Produces("application/json")]
public class BrandController: ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Brand controller constructor</summary>
    /// <param name="brandBusiness"></param>
    public BrandController(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    /// <summary>Get brand information</summary>
    /// <param name="brandName"> Brand name to get information from</param>
    /// <returns> Brand information </returns>
    [HttpGet("{brandName}")]
    [ProducesResponseType(typeof(BrandModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBrand([Required] string brandName)
    {
        var brandInformation = await _brandBusiness.GetBrandInformation(brandName);

        return brandInformation.Status switch
        {
            HttpStatusCode.OK => Ok(brandInformation.Object),
            HttpStatusCode.NotFound => NotFound(brandInformation.ErrorMessage),
            _ => StatusCode((int)brandInformation.Status, brandInformation.ErrorMessage)
        };
    }
}