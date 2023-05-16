using System.Net;
using FairWearGateway.API.DataAccess.BrandData;
using Microsoft.AspNetCore.Mvc;

namespace FairWearGateway.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class BrandsController : ControllerBase
{
    private readonly IBrandData _brandData;

    public BrandsController(IBrandData brandData)
    {
        _brandData = brandData;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var processingStatusResponse = await _brandData.GetAllBrandsAsync();
        
        return processingStatusResponse.Status != HttpStatusCode.OK ? StatusCode((int) processingStatusResponse.Status, processingStatusResponse.ErrorMessage) : Ok(processingStatusResponse.Object);
    }
}