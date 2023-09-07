using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackOffice.Pages.Products;

/// <summary>PageModel class for creating a new ProductEntity.</summary>
public class CreateModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public ProductEntity ProductEntity { get; set; } = default!;
        
    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the CreateModel with the database context.</summary>
    /// <param name="context">The database context for ProductEntity.</param>
    public CreateModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying the create form.</summary>
    /// <returns>The Razor Page for creating a new ProductEntity.</returns>
    public IActionResult OnGet()
    {
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
        return Page();
    }

    // TODO: To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    /// <summary>HTTP POST request handler for form submission.</summary>
    /// <returns>
    /// If the model state is valid, adds the BrandEntity to the database and redirects to the Index page.
    /// If the model state is not valid, returns the current page with validation errors.
    /// </returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page(); // Return the current page with validation errors

        // Add the ProductEntity to the database
        _context.Products.Add(ProductEntity);
        
        // Save changes to the database asynchronously
        await _context.SaveChangesAsync();

        // Redirect to the Index page after successful creation
        return RedirectToPage("./Index");
    }
}