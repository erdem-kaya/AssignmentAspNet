using System.ComponentModel.DataAnnotations;

namespace Business.Models.Identity;

public class SignUpForm
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "First name", Prompt = "Enter your first name")]
    [DataType(DataType.Text)]
    public static string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last name", Prompt = "Enter your last name")]
    [DataType(DataType.Text)]
    public static string LastName { get; set; } = null!;

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

}
