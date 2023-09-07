using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

public class DeleteModel : PageModel
{
    [BindProperty] public BrandEntity BrandEntity { get; set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    public DeleteModel(BrandAndProductDbContext context)
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();
        
        var brandEntity = await _context.Brands.FindAsync(id);
        if (brandEntity == null)
            return RedirectToPage("./Index");
        
        BrandEntity = brandEntity;
        _context.Brands.Remove(BrandEntity);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}