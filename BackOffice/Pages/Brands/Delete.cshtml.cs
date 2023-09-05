using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BackOffice.DataAccess;
using BackOffice.DataAccess.Entity;

namespace BackOffice.Pages.Brands
{
    public class DeleteModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public DeleteModel(BackOffice.DataAccess.BrandAndProductDbContext context)
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

            var brandentity = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

            if (brandentity == null)
            {
                return NotFound();
            }
            else 
            {
                BrandEntity = brandentity;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }
            var brandentity = await _context.Brands.FindAsync(id);

            if (brandentity != null)
            {
                BrandEntity = brandentity;
                _context.Brands.Remove(BrandEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
