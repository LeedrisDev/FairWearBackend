using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for deleting a BrandEntity.</summary>
public class DeleteModel : PageModel
{
    /// <summary>Property to bind the BrandEntity for deletion.</summary>
    [BindProperty] public BrandEntity BrandEntity { get; set; } = default!;

    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the DeleteModel with the database context.</summary>
    /// <param name="context">The database context for BrandEntity.</param>
    public DeleteModel(BrandAndProductDbContext context)
    {
        _context = context;
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

        var brandEntity = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

        if (brandEntity == null)
            return NotFound();

        BrandEntity = brandEntity;
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

        var brandEntity = await _context.Brands.FindAsync(id);
        if (brandEntity == null)
            return RedirectToPage("./Index");

        BrandEntity = brandEntity;

        // Remove the BrandEntity from the database
        _context.Brands.Remove(BrandEntity);
        
        // Save changes to the database asynchronously
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
