using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Products;

/// <summary>PageModel class for editing a ProductEntity.</summary>
public class EditModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public ProductEntity ProductEntity { get; set; } = default!;
    
    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the EditModel with the database context.</summary>
    /// <param name="context">The database context for ProductEntity.</param>
    public EditModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying the edit form.</summary>
    /// <param name="id">The ID of the ProductEntity to edit.</param>
    /// <returns>
    /// If the ID is not provided or the ProductEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the edit form for the ProductEntity.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var productEntity =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (productEntity == null)
            return NotFound();
            
        ProductEntity = productEntity;
        ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Id");
        return Page();
    }

    // TODO: To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.

    /// <summary>HTTP POST request handler for updating a ProductEntity.</summary>
    /// <returns>
    /// If the model state is not valid, returns the current page with validation errors.
    /// If the ProductEntity does not exist, returns a "Not Found" result.
    /// If the update is successful, redirects to the Index page.
    /// </returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // Attach the ProductEntity to track changes
        _context.Attach(ProductEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductEntityExists(ProductEntity.Id))
                return NotFound();
                
            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool ProductEntityExists(long id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}