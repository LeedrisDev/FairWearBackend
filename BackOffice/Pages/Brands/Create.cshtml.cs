using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for creating a new BrandEntity.</summary>
[Authorize]
public class CreateModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public BrandEntity BrandEntity { get; set; } = default!;

    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the CreateModel with the database context.</summary>
    /// <param name="context">The database context for BrandEntity.</param>
    public CreateModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying the create form.</summary>
    /// <returns>The Razor Page for creating a new BrandEntity.</returns>
    public IActionResult OnGet()
    {
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
        
        // Check if a brand with the same name already exists in the database
        if (_context.Brands.Any(b => b.Name.ToLower() == BrandEntity.Name.ToLower()))
        {
            // If a brand with the same name exists, add a model error
            ModelState.AddModelError(string.Empty, "Brand with the same name already exists.");
            return Page(); // Return the current page with the alert
        }

        var categories = JsonConvert.DeserializeObject<List<string>>(BrandEntity.Categories.First());
        var ranges = JsonConvert.DeserializeObject<List<string>>(BrandEntity.Ranges.First());

        if (categories == null || ranges == null)
        {
            ModelState.AddModelError(string.Empty, "Categories and ranges cannot be empty.");
            return Page();
        }

        BrandEntity.Categories = categories;
        BrandEntity.Ranges = ranges;

        // Add the BrandEntity to the database
        _context.Brands.Add(BrandEntity);

        // Save changes to the database asynchronously
        await _context.SaveChangesAsync();

        // Redirect to the Index page after successful creation
        return RedirectToPage("./Index");
    }
}
