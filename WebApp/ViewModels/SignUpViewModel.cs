using Business.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class SignUpViewModel
{
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Full name", Prompt = "Enter your full name")]
    [DataType(DataType.Text)]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$", ErrorMessage = "Invalid password")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Confirm password", Prompt = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Terms and conditions", Prompt = "I agree to the terms and conditions")]
    public bool TermsAndConditions { get; set; }

    public static implicit operator SignUpForm(SignUpViewModel model)
    {
        return new SignUpForm
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password,
            ConfirmPassword = model.ConfirmPassword,
            TermsAndConditions = model.TermsAndConditions
        };
    }
}
