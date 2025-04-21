using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Helpers;
using WebApp.ViewModels;
using WebApp.ViewModels.UserProfile;

namespace WebApp.Controllers;

[Authorize]
[Route("users")]
public class UserProfilesController(IUsersProfileService userProfileService, IWebHostEnvironment environment, UserManager<ApplicationUserEntity> userManager, RoleManager<IdentityRole> roleManager) : Controller
{
    private readonly IUsersProfileService _userProfileService = userProfileService;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


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

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userProfileService.DeleteUserProfileAsync(id);
        if (result)
            return RedirectToAction("UsersList");

        return BadRequest();
    }


    // USER SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Json(new List<object>());

        var users = await _userProfileService.GetAllUsersProfileAsyc();

        var likeUsers = users
            .Where(u => (u.FirstName + " " + u.LastName).Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Select(u => new
            {
                id = u.Id,
                profilePicture = u.ProfilePicture,
                fullName = $"{u.FirstName} {u.LastName}"
            })
            .ToList();

        return Json(likeUsers);
    }

    //Add role för user
    [HttpGet("add-role/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _roleManager.Roles.ToListAsync();
        var viewModel = new RoleViewModel
        {
            UserId = user.Id,
            RoleList = roles.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name,
            }).ToList()
        };

        return PartialView("~/Views/Shared/Partials/Components/UsersPartials/_AddRoleToUser.cshtml", viewModel);
    }

    [HttpPost("add-role/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRole(string id, RoleViewModel form)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var result = await _userManager.AddToRoleAsync(user, form.Roles);

        if (result.Succeeded)
            return RedirectToAction("UsersList");

        return BadRequest(new
        {
            success = false,
            globalError = "Failed to update user"
        });
    }
}


