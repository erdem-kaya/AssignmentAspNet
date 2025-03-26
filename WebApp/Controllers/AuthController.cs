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

        //Chatgpt hjälpte mig här
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return BadRequest(new
        {
            success = false,
            errors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Invalid login attempt." } },
                { "Password", new[] { "Invalid login attempt." } }
            }
        });

    }
  


    [Route("register")]
    public IActionResult SignUpPage()
    {
        var model = new SignUpViewModel
        {
            Title = "Create Account"
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
        else
        {
            var result = await _authService.SingUpAsync(model);
            if (result)
            {
                return RedirectToAction("SignInPage", "Auth");
            }
            return View(model);
        }
        
    }

    
    public async Task<IActionResult> SignOut()
    {
        await _authService.SignOutAsync();
        return LocalRedirect("~/");
    }
}
