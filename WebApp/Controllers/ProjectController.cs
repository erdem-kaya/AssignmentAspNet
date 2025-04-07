using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Project;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [HttpGet]
    public async Task<IActionResult> ProjectsList()
    {
        var model = new ProjectViewModel
        {
            ProjectList = await _projectService.GetAllProjectsAsync(),
        };

        return View(model);
    }
}
