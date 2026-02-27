using System;
using System.ComponentModel.DataAnnotations;

namespace MathApp;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
