using Business.Models.UserProfile;

namespace Business.Interfaces
{
    public interface IUsersProfileService
    {
        Task<bool> CreateUsersProfileAsync(UserRegistrationForm form);
        Task<bool> DeleteUserProfileAsync(string id);
        Task<IEnumerable<User>> GetAllUsersProfileAsyc();
        Task<User?> GetUserProfileByIdAsync(string id);
        Task<bool> UpdateUserProfileAsync(string id, UserUpdateForm form);
    }
}