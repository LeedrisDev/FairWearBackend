using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

/// <summary>PageModel class for displaying a list of ProductEntities.</summary>
public class IndexModel : PageModel
{
    /// <summary>Property to store a list of BrandEntities to display.</summary>
    public IList<ProductEntity> ProductEntity { get;set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the IndexModel with the database context.</summary>
    /// <param name="context">The database context for ProductEntity.</param>
    public IndexModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying a list of BrandEntities.</summary>
    /// <returns>Populates the ProductEntity property with a list of BrandEntities from the database.</returns>
    public async Task OnGetAsync()
    {
        ProductEntity = await _context.Products
            .Include(p => p.BrandEntity)
            .ToListAsync();
    }
}