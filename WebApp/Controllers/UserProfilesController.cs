using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.UserProfile;

namespace WebApp.Controllers;

[Route("users")]
public class UserProfilesController(IUsersProfileService userProfileService) : Controller
{
    private readonly IUsersProfileService _userProfileService = userProfileService;

    
    [HttpGet("")]
    public async Task<IActionResult> UsersList()
    {
        var model = new UserProfileViewModel
        {
            UserProfileList = await _userProfileService.GetAllUsersProfileAsyc(),
        };

        return View(model);
    }

    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(UserRegistrationFormViewModel form)
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

        var result = await _userProfileService.CreateUsersProfileAsync(form);
        if (result)
            return RedirectToAction("UsersList");
        
        return BadRequest(new
        {
            success = false,
            globalError = "Failed to create user"
        });
    }

    public IActionResult UpdateUser(UserUpdateFormViewModel form)
    {
     
        return View();
    }

    public IActionResult DeleteUser(string id)
    {
       
        return View();
    }
}
