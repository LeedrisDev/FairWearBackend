using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages;

/// <summary>PageModel class for displaying the privacy page.</summary>
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    /// <summary>Constructor to initialize the PrivacyModel with a logger.</summary>
    /// <param name="logger">The logger for the PrivacyModel.</param>
    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    /// <summary>HTTP GET request handler for the privacy page.</summary>
    public void OnGet()
    {
        // This method is used for handling HTTP GET requests to the privacy page.
    }
}
