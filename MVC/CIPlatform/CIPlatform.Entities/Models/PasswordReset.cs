using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIPlatform.Entities.Models;

public partial class PasswordReset
{
    public int PassResetId { get; set; }

    public string? Email { get; set; }

    public string? Token { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])).+$", ErrorMessage = "Password must be 8 characters long and must Contain 1-Symbol ,1-lowercase,1-Uppercase,1-digit")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]

    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; }
}
