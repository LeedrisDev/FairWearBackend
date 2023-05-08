using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.ProductBusiness;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BrandAndProductDatabase.API.Controllers;

/// <summary>Controller for managing products.</summary>
[ApiController]
[Route("api/products")]
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
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var productList = await _productBusiness.GetAllProductsAsync();

        var productResponse = productList.Object.Select(product => _mapper.Map<ProductResponse>(product)).ToList();

        return productList.Status switch
        {
            HttpStatusCode.OK => Ok(productResponse),
            _ => StatusCode((int)productList.Status, productList.ErrorMessage)
        };
    }

    /// <summary>Gets a single Product by its ID.</summary>
    /// <param name="id">The ID of the Product to get.</param>
    /// <returns>An HTTP response containing the Product.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetProductByIdAsync(int id)
    {
        var product = await _productBusiness.GetProductByIdAsync(id);

        return product.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(product.Object)),
            HttpStatusCode.NotFound => NotFound(),
            _ => StatusCode((int)product.Status, product.ErrorMessage)
        };
    }

    /// <summary>Creates a new Product in the database.</summary>
    /// <param name="product">The Product containing the Product information.</param>
    /// <returns>An HTTP response containing the newly created Product.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductResponse product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = await _productBusiness.CreateProductAsync(_mapper.Map<ProductDto>(product));

        return createdProduct.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(createdProduct.Object)),
            _ => StatusCode((int)createdProduct.Status, createdProduct.ErrorMessage)
        };
    }

    /// <summary>Updates a Product in the database.</summary>
    /// <param name="product">The updated Product data.</param>
    [HttpPatch]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateProductAsync([FromBody] ProductResponse product)
    {
        var updatedProduct = await _productBusiness.UpdateProductAsync(_mapper.Map<ProductDto>(product));

        return updatedProduct.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<ProductResponse>(updatedProduct.Object)),
            HttpStatusCode.NotFound => NotFound(updatedProduct.ErrorMessage),
            _ => StatusCode((int)updatedProduct.Status, updatedProduct.ErrorMessage)
        };
    }

    /// <summary>Deletes a Product with the given id from the database.</summary>
    /// <param name="id">The id of the Product to delete.</param>
    /// <returns>
    /// Returns a NoContentResult if the Product was deleted successfully, otherwise returns a NotFoundResult.
    /// </returns>
    /// <response code="200">The Product was deleted successfully.</response>
    /// <response code="404">The Product with the given id was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var product = await _productBusiness.GetProductByIdAsync(id);

        if (product.Status == HttpStatusCode.NotFound)
            return NotFound();

        var deleteProduct = await _productBusiness.DeleteProductAsync(id);
        return StatusCode((int)deleteProduct.Status, deleteProduct.ErrorMessage);
    }
}