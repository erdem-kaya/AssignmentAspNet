using Business.Models.UserProfile;

namespace WebApp.ViewModels.UserProfile;

public class UserProfileViewModel
{
    public UserRegistrationFormViewModel RegistrationForm { get; set; } = new();
    public UserUpdateFormViewModel UpdateForm { get; set; } = new();
    public UserDeleteViewModel Delete { get; set; } = new();
    public IEnumerable<User> UserProfileList { get; set; } = [];
}
