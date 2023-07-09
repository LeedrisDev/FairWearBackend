using System.ComponentModel.DataAnnotations;
using System.Net;
using BrandAndProductDatabase.Service.Protos;
using FairWearGateway.API.Business.BrandBusiness;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace FairWearGateway.API.Controllers;

/// <summary>Controller that handles the requests for the Brand model.</summary>
[ApiController]
[Route("api/")]
[Produces("application/json")]
public class BrandsController : ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Constructor of the BrandsController class.</summary>
    public BrandsController(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    /// <summary>Gets a brand by its id.</summary>
    [HttpGet("brand/{brandId:int}")]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetBrandById([Required] int brandId)
    {
        var processingStatusResponse = _brandBusiness.GetBrandById(brandId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Gets a brand by its name.</summary>
    [HttpPost("brand/name")]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public IActionResult GetBrandByName([Required] BrandByNameRequest brandRequest)
    {
        var processingStatusResponse = _brandBusiness.GetBrandByName(brandRequest.Name);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Gets all brands.</summary>
    [HttpGet("brand")]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllBrandsAsync([FromQuery] Dictionary<string, string> filters)
    {
        var processingStatusResponse = await _brandBusiness.GetAllBrands(filters);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}