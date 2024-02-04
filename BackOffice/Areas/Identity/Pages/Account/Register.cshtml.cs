using System.Text;
using System.Text.Encodings.Web;
using BackOffice.DataAccess.Entities;
using BackOffice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BackOffice.Areas.Identity.Pages.Account;

/// <summary>Razor page for registering a new user.</summary>
public class RegisterModel : PageModel
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IUserStore<UserEntity> _userStore;
    private readonly IUserEmailStore<UserEntity> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;

    /// <summary>Initializes a new instance of the <see cref="RegisterModel"/> class.</summary>
    /// <param name="userManager">The user manager for managing user-related operations.</param>
    /// <param name="userStore">The user store for accessing and managing user data.</param>
    /// <param name="signInManager">The sign-in manager for managing user sign-in operations.</param>
    /// <param name="logger">The logger for logging registration-related activities.</param>
    /// <param name="emailSender">The email sender for sending registration-related emails.</param>
    public RegisterModel(
        UserManager<UserEntity> userManager,
        IUserStore<UserEntity> userStore,
        SignInManager<UserEntity> signInManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public RegisterFormModel Input { get; set; } = null!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> ExternalLogins { get; set; } = null!;

    /// <summary>
    /// Handles the HTTP GET request for the page and initializes necessary data.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after the operation is complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    /// <summary>
    /// Handles the HTTP POST request for user registration.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after the registration is complete.</param>
    /// <returns>An IActionResult representing the result of the registration.</returns>
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
        if (!ModelState.IsValid) 
            return Page();
        
        var user = CreateUser();
        
        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;

        await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account with password.");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code, returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private UserEntity CreateUser()
    {
        try
        {
            return Activator.CreateInstance<UserEntity>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(UserEntity)}'. " +
                                                $"Ensure that '{nameof(UserEntity)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<UserEntity> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<UserEntity>)_userStore;
    }
}