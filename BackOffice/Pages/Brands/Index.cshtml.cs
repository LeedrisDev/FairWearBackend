using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

public class IndexModel : PageModel
{
    private readonly BrandAndProductDbContext _context;
    
    public IList<BrandEntity> BrandEntity { get;set; } = default!;
    
    public IndexModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        BrandEntity = await _context.Brands.ToListAsync();
    }
}