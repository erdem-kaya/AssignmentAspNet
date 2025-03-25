using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{

    private readonly IAuthService _authService = authService;

    public IActionResult SignInPage()
    {
        

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignInPage(SignInViewModel model)
    {

        if (ModelState.IsValid)
        {
            var result = await _authService.SignInAsync(model);
            if (result)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }
        return View();
    }

    public IActionResult SignUpPage()
    {
        var model = new SignUpViewModel
        {
            Title = "Create Account"
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SignUpPage(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await _authService.SingUpAsync(model);
            if (result)
            {
                return RedirectToAction("SignInPage", "Auth");
            }
            return View(model);
        }
        return View();
    }

    public async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return LocalRedirect("~/");
    }
}
