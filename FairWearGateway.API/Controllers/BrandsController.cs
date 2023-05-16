using System.Net;
using FairWearGateway.API.DataAccess.BrandData;
using Microsoft.AspNetCore.Mvc;

namespace FairWearGateway.API.Controllers;

/// <summary>Controller that handles the requests for the Brand model.</summary>
[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class BrandsController : ControllerBase
{
    private readonly IBrandData _brandData;

    /// <summary>Constructor of the BrandsController class.</summary>
    public BrandsController(IBrandData brandData)
    {
        _brandData = brandData;
    }
    
    /// <summary>Gets all brands.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var processingStatusResponse = await _brandData.GetAllBrandsAsync();
        
        return processingStatusResponse.Status != HttpStatusCode.OK ? StatusCode((int) processingStatusResponse.Status, processingStatusResponse.ErrorMessage) : Ok(processingStatusResponse.Object);
    }
}