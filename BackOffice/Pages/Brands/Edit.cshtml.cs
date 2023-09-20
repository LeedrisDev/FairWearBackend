using System.Net;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;


/// <summary>PageModel class for editing a BrandEntity.</summary>
[Authorize]
public class EditModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public BrandModel Brand { get; set; } = default!;

    private readonly IBrandBusiness _brandBusiness;
    
    /// <summary>Initializes a new instance of the <see cref="EditModel"/> class.</summary>
    /// <param name="brandBusiness">The business service for managing brand entities.</param>
    public EditModel(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }


    /// <summary>HTTP GET request handler for displaying the edit form.</summary>
    /// <param name="id">The ID of the BrandEntity to edit.</param>
    /// <returns>
    /// If the ID is not provided or the BrandEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the edit form for the BrandEntity.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null)
            return NotFound();
        
        var response = await _brandBusiness.FindByIdAsync(id.Value);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return NotFound();
        
        Brand = response.Entity;
        return Page();
    }
    
    // TODO: To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.

    /// <summary>HTTP POST request handler for updating a BrandEntity.</summary>
    /// <returns>
    /// If the model state is not valid, returns the current page with validation errors.
    /// If the BrandEntity does not exist, returns a "Not Found" result.
    /// If the update is successful, redirects to the Index page.
    /// </returns>
    public async Task<IActionResult> OnPostAsync(long? id)
    {
        if (!ModelState.IsValid)
            return Page();
        
        if (Brand.Categories.Count == 0)
        {
            ModelState.AddModelError($"{nameof(Brand)}.{nameof(Brand.Categories)}", "Please add at least one category.");
            return Page();
        }

        if (Brand.Ranges.Count == 0)
        {
            ModelState.AddModelError($"{nameof(Brand)}.{nameof(Brand.Ranges)}", "Please add at least one range.");
            return Page();
        }

        if (id == null)
            return NotFound();
        
        Brand.Id = id.Value;
        var response = await _brandBusiness.UpdateAsync(Brand);

        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => NotFound(),
            HttpStatusCode.InternalServerError => StatusCode(500),
            _ => RedirectToPage("./Details", new {id = Brand.Id})
        };
    }
}
