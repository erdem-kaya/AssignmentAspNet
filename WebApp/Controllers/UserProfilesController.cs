using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.UserProfile;

namespace WebApp.Controllers;

public class UserProfilesController(IUsersProfileService userProfileService) : Controller
{
    private readonly IUsersProfileService _userProfileService = userProfileService;

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

    public IActionResult AddUser()
    {
        var model = new UserProfileViewModel
        {
            Title = "Create User"
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
