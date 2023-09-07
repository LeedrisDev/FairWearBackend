using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackOffice.Pages.Products;

public class CreateModel : PageModel
{
    [BindProperty] public ProductEntity ProductEntity { get; set; } = default!;
        
    private readonly BrandAndProductDbContext _context;

    public CreateModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
        return Page();
    }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Products.Add(ProductEntity);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}