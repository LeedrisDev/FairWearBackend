using System.Net;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for displaying details of a BrandEntity.</summary>
[Authorize]
public class DetailsModel : PageModel
{
    /// <summary>Property to store the BrandEntity to display details.</summary>
    public BrandModel Brand { get; set; } = default!;
    
    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Initializes a new instance of the <see cref="DetailsModel"/> class.</summary>
    /// <param name="brandBusiness">The business service for managing brand entities.</param>
    public DetailsModel(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness ?? throw new ArgumentNullException(nameof(brandBusiness));
    }

    /// <summary>HTTP GET request handler for displaying details of a BrandEntity.</summary>
    /// <param name="id">The ID of the BrandEntity to display details for.</param>
    /// <returns>
    /// If the ID is not provided or the BrandEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the details page for the BrandEntity.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var response = await _brandBusiness.FindByIdAsync(id.Value);
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return NotFound();

        Brand = response.Entity;
        return Page();
    }
}
