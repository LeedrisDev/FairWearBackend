using System.ComponentModel.DataAnnotations;
using GoodOnYouScrapperAPI.Business.BrandBusiness;
using Microsoft.AspNetCore.Mvc;

namespace GoodOnYouScrapperAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class BrandController: ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;

    public BrandController(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    [HttpGet("{brandName}")]
    public IActionResult GetBrand([Required] string brandName)
    {
        // TODO

        return Ok();
    }
}