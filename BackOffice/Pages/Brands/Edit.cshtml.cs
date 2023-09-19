using BackOffice.DataAccess;
using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Pages.Brands;


/// <summary>PageModel class for editing a BrandEntity.</summary>
[Authorize]
public class EditModel : PageModel
{
    /// <summary>Property to bind data from the form to this model.</summary>
    [BindProperty] public BrandEntity BrandEntity { get; set; } = default!;

    private readonly BrandAndProductDbContext _context;

    /// <summary>Constructor to initialize the EditModel with the database context.</summary>
    /// <param name="context">The database context for BrandEntity.</param>
    public EditModel(BrandAndProductDbContext context)
    {
        _context = context;
    }

    /// <summary>HTTP GET request handler for displaying the edit form.</summary>
    /// <param name="id">The ID of the BrandEntity to edit.</param>
    /// <returns>
    /// If the ID is not provided or the BrandEntity is not found, returns a "Not Found" result;
    /// otherwise, displays the edit form for the BrandEntity.
    /// </returns>
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var brandEntity = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
        if (brandEntity == null)
            return NotFound();

        BrandEntity = brandEntity;
        return Page();
    }
    
    // TODO: To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.

    /// <summary>HTTP POST request handler for updating a BrandEntity.</summary>
    /// <returns>
    /// If the model state is not valid, returns the current page with validation errors.
    /// If the BrandEntity does not exist, returns a "Not Found" result.
    /// If the update is successful, redirects to the Index page.
    /// </returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Attach the BrandEntity to track changes
        _context.Attach(BrandEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BrandEntityExists(BrandEntity.Id))
                return NotFound();

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool BrandEntityExists(long id)
    {
        return _context.Brands.Any(e => e.Id == id);
    }
}
