using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService, SignInManager<ApplicationUserEntity> signInManager, UserManager<ApplicationUserEntity> userManager) : Controller
{

    private readonly IAuthService _authService = authService;
    private readonly SignInManager<ApplicationUserEntity> _signInManager = signInManager;
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;

    public IActionResult SignInPage()
    {
        var model = new SignInViewModel
        {
            Title = "Login",
            ErrorMessages = ""
        };
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SignInPage(SignInViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });
        }

        var result = await _authService.SignInAsync(model);
        if (result)
            return RedirectToAction("Index", "Admin");

        //Chatgpt hjälpe mig
        return BadRequest(new
        {
            success = false,
            globalError = "Incorrect email or password"
        });
    }


    [Route("register")]
    public IActionResult SignUpPage()
    {
        var model = new SignUpViewModel
        {
            Title = "Create Account",
            ErrorMessages = ""

        };

        return View(model);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> SignUpPage(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage)
                .ToArray());

            return BadRequest(new { sucess = false, errors });
        }
        var result = await _authService.SingUpAsync(model);
        if (result)
        {
            return RedirectToAction("SignInPage", "Auth");
        }
        return BadRequest(new
        {
            success = false,
            globalError = "This e-mail address is available"
        });
    }

    public async Task<IActionResult> SignOutUser()
    {
        await _authService.SignOutAsync();
        return LocalRedirect("~/");
    }



    [HttpPost]
    public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignInPage");
        }

        Console.WriteLine("Provider: " + provider);
        Console.WriteLine("ReturnUrl: " + returnUrl);

        var redirectUrl = Url.Action("ExternalSignInCallback", "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }


    public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");
        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external: {remoteError}");
            return View("SignInPage");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ModelState.AddModelError("", "Error loading external login information");
            return RedirectToAction("SignInPage");
        }

        var signinResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signinResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = string.Empty;
            string lastName = string.Empty;

            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }


            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var user = new ApplicationUserEntity
            {
                UserName = username,
                Email = email,
            };

            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                var userProfile = new UsersProfileEntity
                {
                    Id = user.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    JobTitle = "JobTitle",
                    Address = "Address",
                    City = "City",
                    Birthday = DateTime.Now,
                    ProfilePicture = "/Images/avatar-default.svg"
                };

                await _authService.AddUserProfileAsync(userProfile);

                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("SignInPage");
        }
    }
}
