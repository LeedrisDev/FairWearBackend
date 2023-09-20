using System.Net;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for creating a new BrandEntity.</summary>
[Authorize]
public class CreateModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public BrandModel Brand { get; set; } = default!;
    
    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Initializes a new instance of the <see cref="CreateModel"/> class.</summary>
    /// <param name="brandBusiness">The business service for managing brand entities.</param>
    public CreateModel(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    /// <summary>HTTP GET request handler for displaying the create form.</summary>
    /// <returns>The Razor Page for creating a new BrandEntity.</returns>
    public IActionResult OnGet()
    {
        Brand = new BrandModel();
        return Page();
    }

    /// <summary>HTTP POST request handler for form submission.</summary>
    /// <returns>
    /// If the model state is valid, adds the BrandEntity to the database and redirects to the Index page.
    /// If the model state is not valid, returns the current page with validation errors.
    /// </returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page(); // Return the current page with validation errors
        
        var response = await _brandBusiness.FindByNameAsync(Brand.Name);

        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            // If a brand with the same name exists, add a model error
            ModelState.AddModelError(string.Empty, "Brand with the same name already exists.");
            return Page(); // Return the current page with the alert
        }

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

        var savedResponse = await _brandBusiness.InsertAsync(Brand);

        // Redirect to the Details page after successful creation
        if (savedResponse.StatusCode == HttpStatusCode.Created)
            return RedirectToPage("./Details", new { id = savedResponse.Entity.Id });
        
        ModelState.AddModelError(string.Empty, "Failed to save the brand.");
        return Page();
    }
}
