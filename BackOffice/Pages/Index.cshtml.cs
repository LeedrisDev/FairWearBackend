using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages;

/// <summary>PageModel class for the index page.</summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Constructor to initialize the IndexModel with a logger.</summary>
    /// <param name="logger">The logger for the IndexModel.</param>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    /// <summary>HTTP GET request handler for the index page.</summary>
    public void OnGet()
    {
        // This method is used for handling HTTP GET requests to the index page.
    }
}
