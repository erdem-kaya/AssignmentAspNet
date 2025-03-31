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
            Title = "Team Members",
            ErrorMessages = "No item!",
            UserProfileList = await _userProfileService.GetAllUsersProfileAsyc(),
        };

        return View(model);
    }

    public IActionResult AddUser(UserRegistrationFormViewModel form)
    {
        var model = new UserProfileViewModel
        {
            Title = "Create User",
            RegistrationForm = form,
        };
        return View(model);
    }

    public IActionResult UpdateUser(string id)
    {
        var model = new UserProfileViewModel
        {
            Title = "Update User"
        };
        return View(model);
    }

    public IActionResult DeleteUser(string id)
    {
        var model = new UserProfileViewModel
        {
            Title = "Delete User"
        };
        return View(model);
    }
}
