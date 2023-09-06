using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;

namespace BackOffice.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public DeleteModel(BackOffice.DataAccess.BrandAndProductDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public ProductEntity ProductEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productentity = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (productentity == null)
            {
                return NotFound();
            }
            else 
            {
                ProductEntity = productentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var productentity = await _context.Products.FindAsync(id);

            if (productentity != null)
            {
                ProductEntity = productentity;
                _context.Products.Remove(ProductEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
