using Business.Factories;
using Business.Interfaces;
using Business.Models.Identity;
using Business.Models.UserProfile;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;


namespace Business.Services;

public class AuthService(UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager, UsersProfileRepository usersProfileRepository) : IAuthService
{
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;
    private readonly UsersProfileRepository _usersProfileRepository = usersProfileRepository;


    public async Task<bool> SingUpAsync(SignUpForm form)
    {
        try
        {
            var (appUser, userProfile) = IdentityFactory.Create(form);
            var result = await _userManager.CreateAsync(appUser, form.Password);
            if (result.Succeeded)
            {
                userProfile.Id = appUser.Id;
                // Create user profile if user was created
                // Vi behöver FirstName och LastName för att skapa en UserProfile i databasen
                await _usersProfileRepository.CreateAsync(userProfile);
                return true;
            }
            return false;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"User not created, {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SignInAsync(SignInForm form)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.IsPersistent, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(form.Email);
                if (user != null) 
                { 
                    var userProfile = await _usersProfileRepository.GetItemAsync(u => u.Id == user.Id);
                    if (userProfile != null)
                    {
                        await AddClaimByEmailAsync(user, "DisplayName", $"{userProfile.FirstName} {userProfile.LastName}");
                        
                        var profileImage = userProfile.ProfilePicture;
                        if (profileImage != null)
                        {
                            await AddClaimByEmailAsync(user, "ProfileImage", profileImage);
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Debug.WriteLine($"User not signed in, {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SignOutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"User not signed out, {ex.Message}");
            return false;
        }

    }

    public async Task AddClaimByEmailAsync(ApplicationUserEntity user, string typeName, string value)
    {
        // Claim check
        if (user != null)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            if (!claims.Any(x => x.Type == typeName))
            {
                await _userManager.AddClaimAsync(user, new Claim(typeName, value));
            }
        }
    }
}
