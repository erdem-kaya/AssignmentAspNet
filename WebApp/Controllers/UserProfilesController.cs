using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.ViewModels.UserProfile;

namespace WebApp.Controllers;

[Authorize]
[Route("users")]
public class UserProfilesController(IUsersProfileService userProfileService, IWebHostEnvironment environment) : Controller
{
    private readonly IUsersProfileService _userProfileService = userProfileService;
    private readonly IWebHostEnvironment _environment = environment;


    [HttpGet]
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
        var userImg = Request.Form.Files["ProfilePicture"];
        if (userImg != null)
            form.ProfilePicture = await ImageUploadHelper.UploadAsync(userImg, _environment);
        

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

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> UpdateUser(string id)
    {
        var user = await _userProfileService.GetUserProfileByIdAsync(id);
        if (user == null)
            return NotFound();

        var viewModel = new UserUpdateFormViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            JobTitle = user.JobTitle,
            Address = user.Address,
            City = user.City,
            Day = user.Birthday.Day,
            Month = user.Birthday.Month,
            Year = user.Birthday.Year,
            ProfilePicture = user.ProfilePicture
        };

        return PartialView("~/Views/Shared/Partials/Components/UsersPartials/_UpdateUserProfile.cshtml", viewModel);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> UpdateUser(string id, UserUpdateFormViewModel form)
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

        var userImg = Request.Form.Files["ProfilePicture"];
        if (userImg != null)
            form.ProfilePicture = await ImageUploadHelper.UploadAsync(userImg, _environment);

        var result = await _userProfileService.UpdateUserProfileAsync(id, form);
        if (result)
            return RedirectToAction("UsersList");

        return BadRequest(new
        {
            success = false,
            globalError = "Failed to update user"
        });
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userProfileService.DeleteUserProfileAsync(id);
        if (result)
            return RedirectToAction("UsersList");

        return BadRequest();
    }
}
