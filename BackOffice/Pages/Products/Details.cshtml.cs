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
    public class DetailsModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public DetailsModel(BackOffice.DataAccess.BrandAndProductDbContext context)
        {
            _context = context;
        }

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
    }
}
