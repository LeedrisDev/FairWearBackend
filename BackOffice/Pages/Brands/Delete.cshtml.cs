using System.Net;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for deleting a BrandEntity.</summary>
[Authorize]
public class DeleteModel : PageModel
{
    /// <summary>Property to bind the BrandEntity for deletion.</summary>
    [BindProperty] public BrandModel Brand { get; set; } = default!;

    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Initializes a new instance of the <see cref="DeleteModel"/> class.</summary>
    /// <param name="brandBusiness">The business service for managing brand entities.</param>
    public DeleteModel(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    /// <summary>HTTP GET request handler for displaying the delete confirmation page.</summary>
    /// <param name="id">The ID of the BrandEntity to delete.</param>
    /// <returns>
    /// If the ID is not provided or the BrandEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the delete confirmation page.
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

    /// <summary>HTTP POST request handler for deleting a BrandEntity.</summary>
    /// <param name="id">The ID of the BrandEntity to delete.</param>
    /// <returns>
    /// If the ID is not provided or the BrandEntity is not found, redirects to the Index page;
    /// otherwise, deletes the BrandEntity and redirects to the Index page.
    /// </returns>
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var response = await _brandBusiness.FindByIdAsync(id.Value);
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return NotFound();

        Brand = response.Entity;

        // Remove the brand from the database
        await _brandBusiness.DeleteAsync(Brand.Id);
        return RedirectToPage("./Index");
    }
}
