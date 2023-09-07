using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

public class DetailsModel : PageModel
{
    public ProductEntity ProductEntity { get; set; } = default!; 
        
    private readonly BrandAndProductDbContext _context;

    public DetailsModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

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