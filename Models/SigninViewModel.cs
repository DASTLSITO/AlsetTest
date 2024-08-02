using System.ComponentModel.DataAnnotations;

namespace AlsetTest.Models;

public class SigninViewModel
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords doesn't match.")]
    public string ConfirmPassword { get; set; }
}