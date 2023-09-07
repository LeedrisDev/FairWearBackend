using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

public class IndexModel : PageModel
{
    private readonly BrandAndProductDbContext _context;

    public IndexModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public IList<BrandEntity> BrandEntity { get;set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Brands != null)
        {
            BrandEntity = await _context.Brands.ToListAsync();
        }
    }
}