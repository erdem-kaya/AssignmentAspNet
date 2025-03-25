using Business.Factories;
using Business.Models.Identity;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;


namespace Business.Services;

public class AuthenticationService(UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager)
{
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;

    public async Task<bool> SingUpAsync(SignUpForm form)
    {
        try
        {
            var (appUser, userProfile) = IdentityFactory.Create(form);
            var result = await _userManager.CreateAsync(appUser, form.Password);
            return result.Succeeded;

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
