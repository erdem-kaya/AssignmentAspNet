using Business.Factories;
using Business.Hubs;
using Business.Interfaces;
using Business.Models.Identity;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;


namespace Business.Services;

public class AuthService(UserManager<ApplicationUserEntity> userManager, SignInManager<ApplicationUserEntity> signInManager, UsersProfileRepository usersProfileRepository, INotificationService notificationService, RoleManager<IdentityRole> roleManager) : IAuthService
{
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;
    private readonly UsersProfileRepository _usersProfileRepository = usersProfileRepository;
    private readonly INotificationService _notificationService = notificationService;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    public async Task<bool> SingUpAsync(SignUpForm form)
    {
        try
        {
            var (appUser, userProfile) = IdentityFactory.Create(form);
            var result = await _userManager.CreateAsync(appUser, form.Password);
            if (result.Succeeded)
            {
                //Add a Role to the user if the user is created on the Team Members Page
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                var addRoleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (!addRoleResult.Succeeded)
                {
                    Debug.WriteLine($"Error adding role to user: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                    return false;
                }

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
                    var notificationEntity = new NotificationEntity
                    {
                        Message = $"{user.UsersProfile?.FirstName} {user.UsersProfile?.LastName} signed in",
                        NotificationTypeId = 1,
                    };
                    await _notificationService.AddNotificationAsync(notificationEntity, user.Id);

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

    public async Task AddUserProfileAsync(UsersProfileEntity usersProfile)
    {
        await _usersProfileRepository.CreateAsync(usersProfile);
    }
}

