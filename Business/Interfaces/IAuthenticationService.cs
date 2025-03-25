using Business.Models.Identity;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(SignInForm form);
        Task<bool> SignOutAsync();
        Task<bool> SingUpAsync(SignUpForm form);
    }
}