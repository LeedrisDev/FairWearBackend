using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

public class CreateModel : PageModel
{
    [BindProperty] public BrandEntity BrandEntity { get; set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    public CreateModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Brands.Add(BrandEntity);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}