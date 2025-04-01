using Business.Models.UserProfile;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.UserProfile;

public class UserUpdateFormViewModel
{
    public string Id { get; set; } = null!;

    [Display(Name = "Profile picture")]
    [DataType(DataType.ImageUrl)]
    public string? ProfilePicture { get; set; }


    [Display(Name = "First name", Prompt = "Enter your first name")]
    [DataType(DataType.Text)]
    public string? FirstName { get; set; }


    [Display(Name = "Last name", Prompt = "Enter your last name")]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }


    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [RegularExpression(@"^((\+46|0046|07)[0-9]{8})$", ErrorMessage = "Invalid Swedish phone number format")]
    [Display(Name = "Phone number", Prompt = "Enter your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job title", Prompt = "Enter your job title")]
    [DataType(DataType.Text)]
    public string? JobTitle { get; set; }

    [Display(Name = "Address", Prompt = "Enter your address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }


    [Display(Name = "City", Prompt = "Enter your city")]
    [DataType(DataType.Text)]
    public string? City { get; set; }

    [Range(1, 31, ErrorMessage = "Invalid day")]
    [Display(Name = "Day")]
    public int Day { get; set; }


    [Range(1, 12, ErrorMessage = "Invalid month")]
    [Display(Name = "Month")]
    public int Month { get; set; }


    [Range(1900, 2100, ErrorMessage = "Invalid year")]
    [Display(Name = "Year")]
    public int Year { get; set; }

    //ChatGpt hjälpte mig med denna kod
    [ScaffoldColumn(false)]
    public DateTime? Birthday
    {
        get
        {
            try
            {
                return new DateTime(Year, Month, Day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }

    public static implicit operator UserUpdateForm(UserUpdateFormViewModel model)
    {
        return new UserUpdateForm
        {
            Id = model.Id,
            ProfilePicture = model.ProfilePicture,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            JobTitle = model.JobTitle,
            Address = model.Address,
            City = model.City,
            Birthday = model.Birthday
        };
    }
}
