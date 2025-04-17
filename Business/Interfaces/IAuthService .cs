using Business.Models.Identity;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(SignInForm form);
        Task<bool> SignOutAsync();
        Task<bool> SingUpAsync(SignUpForm form);
        Task AddUserProfileAsync(UsersProfileEntity usersProfile);
    }
}