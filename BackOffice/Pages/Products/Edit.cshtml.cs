using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

public class EditModel : PageModel
{
    [BindProperty] public ProductEntity ProductEntity { get; set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    public EditModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var productEntity =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (productEntity == null)
            return NotFound();
            
        ProductEntity = productEntity;
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Attach(ProductEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductEntityExists(ProductEntity.Id))
                return NotFound();
                
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ProductEntityExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}