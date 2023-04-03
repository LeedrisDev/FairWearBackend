using System.ComponentModel.DataAnnotations;
using System.Net;
using GoodOnYouScrapperAPI.Business.BrandBusiness;
using GoodOnYouScrapperAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodOnYouScrapperAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class BrandController: ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;

    public BrandController(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

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