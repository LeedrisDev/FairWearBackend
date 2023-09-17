using System.ComponentModel.DataAnnotations;

namespace BackOffice.Models;

/// <summary>Class representing the data for the register form.</summary>
public class RegisterFormModel
{
    /// <summary>The user's email address.</summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    /// <summary>The user's username.</summary>
    [Required]
    [Display(Name = "Username")]
    [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
    public string UserName { get; set; } = null!;

    /// <summary>The user's first name.</summary>
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    /// <summary>The user's last name.</summary>
    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    /// <summary>The user's password.</summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    /// <summary>The user's password confirmation.</summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}