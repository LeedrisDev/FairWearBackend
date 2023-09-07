using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

public class CreateModel : PageModel
{
    private readonly BrandAndProductDbContext _context;

    public CreateModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public BrandEntity BrandEntity { get; set; } = default!;
        

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Brands == null || BrandEntity == null)
        {
            return Page();
        }

        _context.Brands.Add(BrandEntity);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}