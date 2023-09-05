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
    public class DetailsModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public DetailsModel(BackOffice.DataAccess.BrandAndProductDbContext context)
        {
            _context = context;
        }

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
    }
}
