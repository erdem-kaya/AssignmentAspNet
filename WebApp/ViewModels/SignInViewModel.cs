﻿using Business.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignInViewModel
{
    public string? Title { get; set; }
    public string? ErrorMessages { get; set; }

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; }

    public static implicit operator SignInForm(SignInViewModel model)
    {
        return new SignInForm
        {
            Email = model.Email,
            Password = model.Password,
            IsPersistent = model.IsPersistent
        };
    }
}
