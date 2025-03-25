using Business.Factories;
using Business.Models.UserProfile;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class UsersProfileService(UserManager<ApplicationUserEntity> userManager, UsersProfileRepository usersProfileRepository)
{
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly UsersProfileRepository _usersProfileRepository = usersProfileRepository;


    public async Task<bool> CreateUsersProfileAsync(UserRegistrationForm form)
    {
        try
        {
            var (appUser, userProfile) = UserProfileFactory.Create(form);
            // There is no password entry input when creating a user in the panel, so we give a fixed password. I don't know if there is a better solution for now.
            // Det finns inget input för lösenordsin när vi skappar en användare i panelen, så vi ger ett fast lösenord. Vet inte om det finns en bättre lösning för nu.
            var result = await _userManager.CreateAsync(appUser, "Exempel123");
            if (result.Succeeded)
            {
                await _usersProfileRepository.CreateAsync(userProfile);
                return true;
            }
            else
            {
                return false;
            }
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"User not created, {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersProfileAsyc()
    {
        try
        {
            var users = await _usersProfileRepository.GetAllAsync();
            
            var newUserList = new List<User>();
            
            foreach (var profile in users)
            {
                var appUser = await _userManager.FindByIdAsync(profile.Id);
                if (appUser != null)
                {
                    var user = UserProfileFactory.Create(appUser, profile);
                    newUserList.Add(user);
                }
            }
            return newUserList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting all users, {ex.Message}");
            return [];
        }
    }

    public async Task<User?> GetUserProfileByIdAsync(string id)
    {
        try
        {
           var appUser = await _userManager.FindByIdAsync(id);
           var userProfile = await _usersProfileRepository.GetItemAsync(x => x.Id == id);

            if (appUser != null && userProfile != null)
            {
                var user = UserProfileFactory.Create(appUser, userProfile);
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting user, {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateUserProfileAsync(string id, UserUpdateForm form)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var userProfile = await _usersProfileRepository.GetItemAsync(x => x.Id == id);
            if (appUser != null && userProfile != null)
            {
                UserProfileFactory.Update(appUser, userProfile, form);
                await _userManager.UpdateAsync(appUser);
                await _usersProfileRepository.UpdateAsync(x => x.Id == id, userProfile);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating user, {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserProfileAsync(string id)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var userProfile = await _usersProfileRepository.GetItemAsync(x => x.Id == id);
            if (appUser != null && userProfile != null)
            {
                await _userManager.DeleteAsync(appUser);
                await _usersProfileRepository.DeleteAsync(x => x.Id == id);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting user, {ex.Message}");
            return false;
        }
    }
}
