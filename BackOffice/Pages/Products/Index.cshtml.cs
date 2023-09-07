using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

public class IndexModel : PageModel
{
    public IList<ProductEntity> ProductEntity { get;set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    public IndexModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        ProductEntity = await _context.Products
            .Include(p => p.BrandEntity)
            .ToListAsync();
    }
}