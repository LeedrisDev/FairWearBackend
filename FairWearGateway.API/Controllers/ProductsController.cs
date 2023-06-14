using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearGateway.API.Business.ProductBusiness;
using FairWearGateway.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace FairWearGateway.API.Controllers;

/// <summary>Controller that handles the requests related to the products.</summary>
[ApiController]
[Route("/api/")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductBusiness _productBusiness;

    /// <summary>Constructor of the ProductsController class.</summary>
    public ProductsController(IProductBusiness productBusiness)
    {
        _productBusiness = productBusiness;
    }

    /// <summary>Gets a product by its id.</summary>
    [HttpGet("product/{productId:int}")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetProductByIdAsync([Required] int productId)
    {
        var processingStatusResponse = await _productBusiness.GetProductByIdAsync(productId);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Gets a product by its barcode(UPC).</summary>
    [HttpGet("product/{upc}")]
    [ProducesResponseType(typeof(ProductInformationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetProductByUpcAsync([Required] string upc)
    {
        var processingStatusResponse = await _productBusiness.GetProductByUpcAsync(upc);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}