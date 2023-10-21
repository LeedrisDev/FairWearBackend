using System.ComponentModel.DataAnnotations;
using System.Net;
using BrandAndProduct.Service.Protos;
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
    public IActionResult GetProductById([Required] int productId)
    {
        var processingStatusResponse = _productBusiness.GetProductById(productId);

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
    public IActionResult GetProductByUpc([Required] string upc)
    {
        var processingStatusResponse = _productBusiness.GetProductByUpc(upc);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            HttpStatusCode.NotFound => NotFound(processingStatusResponse.MessageObject),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }

    /// <summary>Gets all products.</summary>
    [HttpGet("product")]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllProductsAsync([FromQuery] Dictionary<string, string> filters)
    {
        var processingStatusResponse = await _productBusiness.GetAllProducts(filters);

        return processingStatusResponse.Status switch
        {
            HttpStatusCode.OK => Ok(processingStatusResponse.Object),
            _ => StatusCode((int)processingStatusResponse.Status, processingStatusResponse.MessageObject)
        };
    }
}