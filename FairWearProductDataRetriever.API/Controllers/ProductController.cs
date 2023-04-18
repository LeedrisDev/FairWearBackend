using System.ComponentModel.DataAnnotations;
using System.Net;
using FairWearProductDataRetriever.API.Business.ProductBusiness;
using FairWearProductDataRetriever.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FairWearProductDataRetriever.API.Controllers;

[ApiController]
// [ApiVersion("1.0")]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController: ControllerBase
{
    private readonly IProductBusiness _productBusiness;

    public ProductController(IProductBusiness productBusiness)
    {
        _productBusiness = productBusiness;
    }
    
    /// <summary>
    /// Endpoint for retrieving product information.
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    [HttpGet("{barcode}")]
    [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProduct([Required] string barcode)
    {
        var productInformation = await _productBusiness.GetProductInformation(barcode);

        return productInformation.Status switch
        {
            HttpStatusCode.OK => Ok(productInformation.Object),
            HttpStatusCode.NotFound => NotFound(productInformation.ErrorMessage),
            _ => StatusCode((int)productInformation.Status, productInformation.ErrorMessage)
        };
    }
}
