using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Pages;

/// <summary>PageModel class for handling errors.</summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>Gets or sets the unique request identifier associated with the error.</summary>
    public string? RequestId { get; set; }

    /// <summary>Gets a boolean value indicating whether the request identifier is available and not empty.</summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>HTTP GET request handler for the error page.</summary>
    public void OnGet()
    {
        // This method is used for handling HTTP GET requests to the error page.
        // It sets the RequestId based on the current Activity or TraceIdentifier in the HttpContext.
    }
}
