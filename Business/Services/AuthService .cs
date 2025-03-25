using Business.Factories;
using Business.Interfaces;
using Business.Models.Identity;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;


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
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
            return result.Succeeded;
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
}
