using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;

namespace BackOffice.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public EditModel(BackOffice.DataAccess.BrandAndProductDbContext context)
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

            var productentity =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (productentity == null)
            {
                return NotFound();
            }
            ProductEntity = productentity;
           ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
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

            _context.Attach(ProductEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(ProductEntity.Id))
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

        private bool ProductEntityExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
