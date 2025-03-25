using Business.Models.Identity;
using Business.Models.UserProfile;
using Data.Entities;

namespace Business.Factories;

public static class IdentityFactory
{
    public static (ApplicationUserEntity appUser, UsersProfileEntity userProfile) Create(SignUpForm form)
    {
        var (firstName, lastName) = SplitFullName(form.FullName);

        var appUser = new ApplicationUserEntity
        {
            Email = form.Email,
            UserName = form.Email,
        };

        var userProfile = new UsersProfileEntity
        {
            Id = appUser.Id,
            FirstName = firstName,
            LastName = lastName,
            JobTitle = "Unknown",
            Address = "Unknown",
            City = "Unknown",
            Birthday = DateTime.UtcNow
        };

        return (appUser, userProfile);
    }


    //ChatGpt hjäper till att dela upp namnet i för och efternamn
    private static (string FirstName, string LastName) SplitFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return ("Unknown", "Unknown");

        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 1 ? (parts[0], string.Join(" ", parts.Skip(1))) : (parts[0], "Unknown");
    }
}
