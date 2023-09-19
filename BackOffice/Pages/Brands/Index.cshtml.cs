using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

/// <summary>PageModel class for displaying a list of BrandEntities.</summary>
[Authorize]
public class IndexModel : PageModel
{
    /// <summary>Property to store a list of BrandEntities to display.</summary>
    public IList<BrandEntity> BrandEntity { get; set; } = default!;

    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the IndexModel with the database context.</summary>
    /// <param name="context">The database context for BrandEntity.</param>
    public IndexModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying a list of BrandEntities.</summary>
    /// <returns>Populates the BrandEntity property with a list of BrandEntities from the database.</returns>
    public async Task OnGetAsync()
    {
        BrandEntity = await _context
            .Brands
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();
    }
}
