using System.ComponentModel.DataAnnotations;
using System.Net;
using BrandAndProductDatabase.API.Models.Response;
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
    
    /// <summary>Gets all products.</summary>
    [HttpGet("products")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var processingStatusResponse = await _productBusiness.GetAllProductsAsync();
        
        return processingStatusResponse.Status != HttpStatusCode.OK ? StatusCode((int) processingStatusResponse.Status, processingStatusResponse.MessageObject) : Ok(processingStatusResponse.Object);
    }
    
    /// <summary>Gets a product by its id.</summary>
    [HttpGet("product/{productId:int}")]
    [ProducesResponseType(typeof(ProductResponse), (int) HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
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
}