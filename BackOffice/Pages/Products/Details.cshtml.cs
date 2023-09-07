using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

/// <summary>PageModel class for displaying details of a ProductEntity.</summary>
public class DetailsModel : PageModel
{
    /// <summary>Property to store the ProductEntity to display details.</summary>
    public ProductEntity ProductEntity { get; set; } = default!; 
        
    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the DetailsModel with the database context.</summary>
    /// <param name="context">The database context for ProductEntity.</param>
    public DetailsModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying details of a ProductEntity.</summary>
    /// <param name="id">The ID of the ProductEntity to display details for.</param>
    /// <returns>
    /// If the ID is not provided or the ProductEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the details page for the ProductEntity.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var productEntity = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (productEntity == null)
            return NotFound();

        ProductEntity = productEntity;
        return Page();
    }
}