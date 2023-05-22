using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.ProductBusiness;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BrandAndProductDatabase.API.Controllers;

/// <summary>Controller for managing products.</summary>
[ApiController]
[Route("api/")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProductBusiness _productBusiness;

    /// <summary>Initializes a new instance of the <see cref="ProductController"/> class.</summary>
    /// <param name="productBusiness">The Product business.</param>
    /// <param name="mapper">The mapper.</param>
    public ProductController(IProductBusiness productBusiness, IMapper mapper)
    {
        _productBusiness = productBusiness;
        _mapper = mapper;
    }

    /// <summary>Gets all the Products in the database.</summary>
    /// <returns>An HTTP response containing a collection of Products.</returns>
    [HttpGet("/products")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var productList = await _productBusiness.GetAllProductsAsync();

        return productList.Status switch
        {
            HttpStatusCode.OK => Ok(
                productList.Object.Select(product => _mapper.Map<ProductResponse>(product)).ToList()),
            _ => StatusCode((int)productList.Status, productList.MessageObject)
        };
    }

    /// <summary>Gets all the Products in the database.</summary>
    /// <returns>An HTTP response containing a collection of Products.</returns>
    [HttpGet("/products")]
    [ProducesResponseType(typeof(ProductScanResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUpcAsync([FromQuery] string Upc)
    {
        var productScores = new ProductScoreResponse
        {
            moral = 2,
            animal = 2,
            environemental = 3
        };

        var productComposition = new List<ProductCompositionResponse>
        {
            new()
            {
                percentage = 77,
                component = "viscose"
            },
            new()
            {
                percentage = 23,
                component = "polyamide"
            }
        };
        var product = new ProductScanResponse()
        {
            name = "White shirt",
            country = "Bangladesh",
            image = "image de ta maman",
            globalScore = (productScores.animal + productScores.environemental + productScores.moral) / 3,
            composition = productComposition.ToArray(),
            alternatives = new string[0],
            brand = "Bershka"
        };

        return Ok(product);
    }

    /// <summary>Creates a new Product in the database.</summary>
    /// <param name="product">The Product containing the Product information.</param>
    /// <returns>An HTTP response containing the newly created Product.</returns>
    [HttpPost("/product")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateProductAsync([Required] [FromBody] ProductResponse product)
    {
        var createdProduct = await _productBusiness.CreateProductAsync(_mapper.Map<ProductDto>(product));

        return createdProduct.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(createdProduct.Object)),
            _ => StatusCode((int)createdProduct.Status, createdProduct.ErrorMessage)
        };
    }

    /// <summary>Updates a Product in the database.</summary>
    /// <param name="product">The updated Product data.</param>
    [HttpPatch("/product")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateProductAsync([Required] [FromBody] ProductResponse product)
    {
        var updatedProduct = await _productBusiness.UpdateProductAsync(_mapper.Map<ProductDto>(product));

        return updatedProduct.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(updatedProduct.Object)),
            HttpStatusCode.NotFound => NotFound(updatedProduct.MessageObject),
            _ => StatusCode((int)updatedProduct.Status, updatedProduct.MessageObject)
        };
    }

    /// <summary>Deletes a Product with the given id from the database.</summary>
    /// <param name="id">The id of the Product to delete.</param>
    /// <returns>
    /// Returns a NoContentResult if the Product was deleted successfully, otherwise returns a NotFoundResult.
    /// </returns>
    /// <response code="200">The Product was deleted successfully.</response>
    /// <response code="404">The Product with the given id was not found.</response>
    [HttpDelete("/product/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteProductAsync([Required] int id)
    {
        var product = await _productBusiness.GetProductByIdAsync(id);

        if (product.Status == HttpStatusCode.NotFound)
            return NotFound();

        var deleteProduct = await _productBusiness.DeleteProductAsync(id);
        return StatusCode((int)deleteProduct.Status, deleteProduct.MessageObject);
    }

    /// <summary>Gets a single Product by its ID.</summary>
    /// <param name="id">The ID of the Product to get.</param>
    /// <returns>An HTTP response containing the Product.</returns>
    [HttpGet("/product/{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetProductByIdAsync([Required] int id)
    {
        var product = await _productBusiness.GetProductByIdAsync(id);

        return product.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(product.Object)),
            HttpStatusCode.NotFound => NotFound(product.MessageObject),
            _ => StatusCode((int)product.Status, product.MessageObject)
        };
    }

    /// <summary>Gets a single Product by its barcode.</summary>
    /// <param name="barcode">The barcode of the Product to get.</param>
    /// <returns>An HTTP response containing the Product.</returns>
    [HttpGet("/product/barcode/{barcode}")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetProductByBarcodeAsync([Required] string barcode)
    {
        var product = await _productBusiness.GetProductByBarcodeAsync(barcode);

        return product.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(product.Object)),
            HttpStatusCode.NotFound => NotFound(product.MessageObject),
            _ => StatusCode((int)product.Status, product.MessageObject)
        };
    }
}