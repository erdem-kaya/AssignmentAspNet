using System.ComponentModel.DataAnnotations;

namespace Business.Models.Identity;

public class ChangePasswordForm
{
    [Required]
    [Display(Name = "Current password", Prompt = "Enter your current password")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$", ErrorMessage = "Invalid password")]
    [Display(Name = "New password", Prompt = "Enter your new password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm new password", Prompt = "Confirm your new password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; } = null!;

}
