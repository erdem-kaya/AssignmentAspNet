using Business.Models.UserProfile;
using Data.Entities;

namespace Business.Factories;

public class UserProfileFactory
{
    public static (ApplicationUserEntity appUser, UsersProfileEntity userProfile) Create(UserRegistrationForm form)
    {
        var appUser = new ApplicationUserEntity
        {
            Email = form.Email,
            UserName = form.Email,
            PhoneNumber = form.PhoneNumber,
        };

        var userProfile = new UsersProfileEntity
        {
            Id = appUser.Id,
            FirstName = form.FirstName,
            LastName = form.LastName,
            JobTitle = form.JobTitle ?? "NULL",
            Address = form.Address ?? "NULL",
            City = form.City ?? "NULL",
            Birthday = form.Birthday,
            ProfilePicture = form.ProfilePicture ?? "NULL",

        };

        return (appUser, userProfile);
    }

    public static User Create(ApplicationUserEntity appUser, UsersProfileEntity usersProfile)
    {
        return new User
        {
            Id = appUser.Id,
            Email = appUser.Email ?? null!,
            PhoneNumber = appUser.PhoneNumber,
            FirstName = usersProfile.FirstName,
            LastName = usersProfile.LastName,
            JobTitle = usersProfile.JobTitle,
            Address = usersProfile.Address,
            City = usersProfile.City,
            Birthday = usersProfile.Birthday,
            ProfilePicture = usersProfile.ProfilePicture,
        };
    }

    public static void Update(ApplicationUserEntity appUser, UsersProfileEntity userProfile, UserUpdateForm form)
    {
        if (!string.IsNullOrWhiteSpace(form.Email))
        {
            appUser.Email = form.Email;
            appUser.UserName = form.Email;
        }

        if (!string.IsNullOrWhiteSpace(form.PhoneNumber))
        {
            appUser.PhoneNumber = form.PhoneNumber;
        }

        if (!string.IsNullOrWhiteSpace(form.FirstName))
        {
            userProfile.FirstName = form.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(form.LastName))
        {
            userProfile.LastName = form.LastName;
        }

        if (!string.IsNullOrWhiteSpace(form.JobTitle))
        {
            userProfile.JobTitle = form.JobTitle;
        }

        if (!string.IsNullOrWhiteSpace(form.Address))
        {
            userProfile.Address = form.Address;
        }

        if (!string.IsNullOrWhiteSpace(form.City))
        {
            userProfile.City = form.City;
        }

        if (form.Birthday.HasValue)
        {
            userProfile.Birthday = form.Birthday.Value;
        }

        if (!string.IsNullOrWhiteSpace(form.ProfilePicture))
        {
            userProfile.ProfilePicture = form.ProfilePicture;
        }
    }
}
