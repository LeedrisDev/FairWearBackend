using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.BrandBusiness;
using BrandAndProductDatabase.API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BrandAndProductDatabase.API.Controllers;

/// <summary>
/// Controller for managing brands.
/// </summary>
[ApiController]
[Route("api/brands")]
[Produces("application/json")]
public class BrandController : ControllerBase
{
    private readonly IBrandBusiness _brandBusiness;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrandController"/> class.
    /// </summary>
    /// <param name="brandBusiness">The brand business.</param>
    public BrandController(IBrandBusiness brandBusiness, IMapper mapper)
    {
        _brandBusiness = brandBusiness;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all the brands in the database.
    /// </summary>
    /// <returns>An HTTP response containing a collection of brands.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllBrandsAsync()
    {
        var brandList = await _brandBusiness.GetAllBrandsAsync();

        var brandResponse = new List<BrandResponse>();

        foreach (var brand in brandList.Object)
        {
            brandResponse.Add(_mapper.Map<BrandResponse>(brand));
        }

        return brandList.Status switch
        {
            HttpStatusCode.OK => Ok(brandResponse),
            _ => StatusCode((int)brandList.Status, brandList.ErrorMessage)
        };
    }
}