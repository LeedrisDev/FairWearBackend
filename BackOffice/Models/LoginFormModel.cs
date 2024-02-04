using System.ComponentModel.DataAnnotations;

namespace BackOffice.Models;

/// <summary>Class representing the data for the login form.</summary>
public class LoginFormModel
{
    /// <summary>The user's username.</summary>
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = null!;

    /// <summary>The user's password.</summary>
    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    /// <summary>Whether or not the user should be remembered.</summary>
    [Display(Name = "Remember me ?")]
    public bool RememberMe { get; set; }
}