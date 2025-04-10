using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using WebApp.Helpers;
using WebApp.ViewModels.Project;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectController(IProjectService projectService, IClientService clientService, IWebHostEnvironment environment) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IWebHostEnvironment _environment = environment;


    [HttpGet("")]
    public async Task<IActionResult> ProjectsList()
    {
        var clients = await _clientService.GetAllClientsAsync();


        var model = new ProjectViewModel
        {
            ProjectList = await _projectService.GetAllProjectsAsync(),
            RegistrationForm = new ProjectRegistrationFormViewModel
            {
                ClientList = clients.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ClientName
                }).ToList()
            },

        };

        return View(model);
    }

    public IActionResult AddProject()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(ProjectRegistrationFormViewModel form)
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

        var projectImg = Request.Form.Files["ProjectImage"];
        if (projectImg != null)
            form.ProjectImage = await ImageUploadHelper.UploadAsync(projectImg, _environment); 

        var result = await _projectService.CreateAsync(form);
        if (result != null)
            return RedirectToAction("ProjectsList");
        return BadRequest(new
        {
            success = false,
            globalError = "Failed to create project"
        });
    }
}
