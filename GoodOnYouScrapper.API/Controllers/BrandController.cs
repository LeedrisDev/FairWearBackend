using System.ComponentModel.DataAnnotations;
using System.Net;
using GoodOnYouScrapper.API.Business.BrandBusiness;
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
    /// <param name="brandRequest">Brand object containing the brand name</param>
    /// <returns> Brand information </returns>
    [HttpGet]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBrand([Required][FromBody] BrandRequest brandRequest)
    {
        var brandInformation = await _brandBusiness.GetBrandInformation(brandRequest.Name);

        return brandInformation.Status switch
        {
            HttpStatusCode.OK => Ok(brandInformation.Object),
            HttpStatusCode.NotFound => NotFound(brandInformation.ErrorMessage),
            _ => StatusCode((int)brandInformation.Status, brandInformation.ErrorMessage)
        };
    }
}