using Business.Models.UserProfile;

namespace WebApp.ViewModels.UserProfile;

public class UserProfileViewModel
{
    public string? Title { get; set; }

    public string? ErrorMessages { get; set; }

    public UserRegistrationFormViewModel RegistrationForm { get; set; } = new();

    public UserUpdateFormViewModel UpdateForm { get; set; } = new();

    public IEnumerable<User> UserProfileList { get; set; } = [];
}
