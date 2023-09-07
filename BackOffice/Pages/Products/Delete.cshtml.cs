using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

public class DeleteModel : PageModel
{
    [BindProperty] public ProductEntity ProductEntity { get; set; } = default!;
        
    private readonly BrandAndProductDbContext _context;

    public DeleteModel(BrandAndProductDbContext context)
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();
            
        var productEntity = await _context.Products.FindAsync(id);

        if (productEntity == null) 
            return RedirectToPage("./Index");
            
        ProductEntity = productEntity;
        _context.Products.Remove(ProductEntity);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}