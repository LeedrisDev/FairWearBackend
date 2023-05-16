using System.Net;
using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace FairWearGateway.API.Controllers;

/// <summary>Controller that handles the requests for the Brand model.</summary>
[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class BrandsController : ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Constructor of the BrandsController class.</summary>
    public BrandsController(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }
    
    /// <summary>Gets all brands.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrandResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var processingStatusResponse = await _brandBusiness.GetAllBrandsAsync();
        
        return processingStatusResponse.Status != HttpStatusCode.OK ? StatusCode((int) processingStatusResponse.Status, processingStatusResponse.ErrorMessage) : Ok(processingStatusResponse.Object);
    }
}