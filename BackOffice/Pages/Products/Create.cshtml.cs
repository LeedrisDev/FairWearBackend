using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;

namespace BackOffice.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public CreateModel(BackOffice.DataAccess.BrandAndProductDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ProductEntity ProductEntity { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Products == null || ProductEntity == null)
            {
                return Page();
            }

            _context.Products.Add(ProductEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
