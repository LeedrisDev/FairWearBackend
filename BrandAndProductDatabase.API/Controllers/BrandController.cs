﻿using System.Net;
using AutoMapper;
using BrandAndProductDatabase.API.Business.BrandBusiness;
using BrandAndProductDatabase.API.Models.Dto;
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

    /// <summary>
    /// Creates a new brand in the database.
    /// </summary>
    /// <param name="brand">The brand containing the brand information.</param>
    /// <returns>An HTTP response containing the newly created brand.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BrandResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateBrandAsync([FromBody] BrandResponse brand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdBrand = await _brandBusiness.CreateBrandAsync(_mapper.Map<BrandDto>(brand));

        return createdBrand.Status switch
        {
            HttpStatusCode.OK => Ok(_mapper.Map<BrandResponse>(createdBrand.Object)),
            _ => StatusCode((int)createdBrand.Status, createdBrand.ErrorMessage)
        };
    }
}