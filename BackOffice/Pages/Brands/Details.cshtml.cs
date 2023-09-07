using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

public class DetailsModel : PageModel
{
    public BrandEntity BrandEntity { get; set; } = default!; 
    
    private readonly BrandAndProductDbContext _context;

    public DetailsModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

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