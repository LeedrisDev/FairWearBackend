using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using FairWearProductDataRetriever.Service.Business.ProductBusiness;
using FairWearProductDataRetriever.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace FairWearProductDataRetriever.Service.Controllers;

/// <summary>Controller for retrieving product information.</summary>
[ApiController]
[Route("api/product")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductBusiness _productBusiness;

    /// <summary>Constructor</summary>
    /// <param name="productBusiness"> Product business.</param>
    public ProductController(IProductBusiness productBusiness)
    {
        _productBusiness = productBusiness;
    }

    /// <summary> Get product information from a barcode.</summary>
    /// <param name="barcode">Barcode of the product.</param>
    /// <returns> Product information.</returns>
    [HttpGet("{barcode}")]
    [ProducesResponseType(typeof(ProductModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetProduct([Required] string barcode)
    {
        if (!IsBarcodeValid(barcode))
            return BadRequest(new ErrorResponse { Message = "Barcode must be composed of only numbers." });

        var productInformation = await _productBusiness.GetProductInformation(barcode);

        return productInformation.Status switch
        {
            HttpStatusCode.OK => Ok(productInformation.Object),
            HttpStatusCode.NotFound => NotFound(productInformation.MessageObject),
            _ => StatusCode((int)productInformation.Status, productInformation.MessageObject)
        };
    }

    private static bool IsBarcodeValid(string barcode)
    {
        // return barcode.Length == 13;
        var regex = new Regex("^[0-9]+$");
        return regex.IsMatch(barcode);
    }
}