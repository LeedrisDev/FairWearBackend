using BackOffice.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackOffice.Areas.Identity.Pages.Account;

/// <summary>Razor page model for the logout page.</summary>
public class LogoutModel : PageModel
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutModel"/> class.
    /// </summary>
    /// <param name="signInManager">The sign-in manager for managing user sign-out operations.</param>
    /// <param name="logger">The logger for logging user logout-related activities.</param>
    public LogoutModel(SignInManager<UserEntity> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    /// Handles the HTTP POST request for user logout.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after the user logs out.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the logout operation.</returns>
    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
            return LocalRedirect(returnUrl);

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return RedirectToPage();
    }
}