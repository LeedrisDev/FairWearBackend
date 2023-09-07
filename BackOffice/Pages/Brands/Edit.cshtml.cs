using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;

public class EditModel : PageModel
{
    private readonly BrandAndProductDbContext _context;

    public EditModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrandEntity BrandEntity { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Brands == null)
        {
            return NotFound();
        }

        var brandentity =  await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
        if (brandentity == null)
        {
            return NotFound();
        }
        BrandEntity = brandentity;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(BrandEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BrandEntityExists(BrandEntity.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool BrandEntityExists(int id)
    {
        return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}