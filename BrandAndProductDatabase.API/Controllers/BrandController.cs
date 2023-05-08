using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.BrandBusiness;
using BrandAndProductDatabase.API.Models.Dto;
using BrandAndProductDatabase.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BrandAndProductDatabase.API.Controllers;

/// <summary>Controller for managing brands.</summary>
[ApiController]
[Route("api/brands")]
[Produces("application/json")]
public class BrandController : ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="BrandController"/> class.</summary>
    /// <param name="brandBusiness">The brand business.</param>
    /// <param name="mapper">The mapper.</param>
    public BrandController(IBrandBusiness brandBusiness, IMapper mapper)
    {
        _brandBusiness = brandBusiness;
        _mapper = mapper;
    }

    /// <summary>Gets all the brands in the database.</summary>
    /// <returns>An HTTP response containing a collection of brands.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var brandList = await _brandBusiness.GetAllBrandsAsync();

        return brandList.Status switch
        {
            HttpStatusCode.OK => Ok(brandList.Object.Select(brand => _mapper.Map<BrandResponse>(brand)).ToList()),
            _ => StatusCode((int)brandList.Status, brandList.ErrorMessage)
        };
    }

    /// <summary>
    /// Gets a single brand by its ID.
    /// </summary>
    /// <param name="id">The ID of the brand to get.</param>
    /// <returns>An HTTP response containing the brand.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBrandByIdAsync(int id)
    {
        var brand = await _brandBusiness.GetBrandByIdAsync(id);

        return brand.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<BrandResponse>(brand.Object)),
            HttpStatusCode.NotFound => NotFound(),
            _ => StatusCode((int)brand.Status, brand.ErrorMessage)
        };
    }

    /// <summary>Creates a new brand in the database.</summary>
    /// <param name="brand">The brand containing the brand information.</param>
    /// <returns>An HTTP response containing the newly created brand.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateBrandAsync([FromBody] BrandResponse brand)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdBrand = await _brandBusiness.CreateBrandAsync(_mapper.Map<BrandDto>(brand));

        return createdBrand.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<BrandResponse>(createdBrand.Object)),
            _ => StatusCode((int)createdBrand.Status, createdBrand.ErrorMessage)
        };
    }

    /// <summary>Updates a brand in the database.</summary>
    /// <param name="brand">The updated brand data.</param>
    [HttpPatch]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateBrandAsync([FromBody] BrandResponse brand)
    {
        var updatedBrand = await _brandBusiness.UpdateBrandAsync(_mapper.Map<BrandDto>(brand));

        return updatedBrand.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<BrandResponse>(updatedBrand.Object)),
            HttpStatusCode.NotFound => NotFound(updatedBrand.ErrorMessage),
            _ => StatusCode((int)updatedBrand.Status, updatedBrand.ErrorMessage)
        };
    }

    /// <summary>Deletes a brand with the given id from the database.</summary>
    /// <param name="id">The id of the brand to delete.</param>
    /// <returns>
    /// Returns a NoContentResult if the brand was deleted successfully, otherwise returns a NotFoundResult.
    /// </returns>
    /// <response code="200">The brand was deleted successfully.</response>
    /// <response code="404">The brand with the given id was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteBrandAsync(int id)
    {
        var brand = await _brandBusiness.GetBrandByIdAsync(id);

        if (brand.Status == HttpStatusCode.NotFound)
            return NotFound();

        var deleteBrand = await _brandBusiness.DeleteBrandAsync(id);
        return StatusCode((int)deleteBrand.Status, deleteBrand.ErrorMessage);
    }
}