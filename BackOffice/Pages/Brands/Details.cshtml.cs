using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for displaying details of a BrandEntity.</summary>
public class DetailsModel : PageModel
{
    /// <summary>Property to store the BrandEntity to display details.</summary>
    public BrandEntity BrandEntity { get; set; } = default!;

    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the DetailsModel with the database context.</summary>
    /// <param name="context">The database context for BrandEntity.</param>
    public DetailsModel(BrandAndProductDbContext context)
    {
        _context = context;
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

        var brandEntity = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
        if (brandEntity == null)
            return NotFound();

        BrandEntity = brandEntity;
        return Page();
    }
}
