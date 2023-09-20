using System.Net;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages.Brands;

/// <summary>Represents the model for Brands index page.</summary>
[Authorize]
public class IndexModel : PageModel
{
    /// <summary>Gets or sets the list of brand entities to display.</summary>
    public IList<BrandModel> Brands { get; set; } = default!;

    private readonly IBrandBusiness _brandBusiness;

    /// <summary>Initializes a new instance of the <see cref="IndexModel"/> class.</summary>
    /// <param name="brandBusiness">The business service for managing brand entities.</param>
    public IndexModel(IBrandBusiness brandBusiness)
    {
        _brandBusiness = brandBusiness;
    }

    /// <summary>Handles the HTTP GET request for the index page.</summary>
    public async Task OnGetAsync()
    {
        var response = await _brandBusiness.GetAsync();
        
        if (response.StatusCode == HttpStatusCode.OK)
            Brands = response.Entity.ToList();
        else
            ModelState.AddModelError(string.Empty, response.Message);
    }
}
