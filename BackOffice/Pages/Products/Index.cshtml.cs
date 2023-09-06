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
    public class IndexModel : PageModel
    {
        private readonly BackOffice.DataAccess.BrandAndProductDbContext _context;

        public IndexModel(BackOffice.DataAccess.BrandAndProductDbContext context)
        {
            _context = context;
        }

        public IList<ProductEntity> ProductEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                ProductEntity = await _context.Products
                .Include(p => p.BrandEntity).ToListAsync();
            }
        }
    }
}
