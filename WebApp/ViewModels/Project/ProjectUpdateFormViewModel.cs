using Business.Models.Project;
using Business.Models.UserProfile;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels.Project;

public class ProjectUpdateFormViewModel
{
    
    public int Id { get; set; }

    [Display(Name = "Project name", Prompt = "Enter the project name")]
    [DataType(DataType.Text)]
    public string? ProjectName { get; set; }

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [Display(Name = "Start date", Prompt = "Select a start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End date", Prompt = "Select an end date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0 - Enter your budget")]
    [DataType(DataType.Currency)]
    public decimal Budget { get; set; } = 0;

    [Display(Name = "Project image", Prompt = "Upload an image")]
    public string? ProjectImage { get; set; }

    [Display(Name = "Client", Prompt = "Select a client")]
    public int ClientId { get; set; }

    [Display(Name = "Project status", Prompt = "Select a project status")]
    public int ProjectStatusId { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Members")]
    public string ProjectWithUsersRaw { get; set; } = "";

    [NotMapped]
    [Display(Name = "Members")]
    public List<string> ProjectWithUsers =>
    ProjectWithUsersRaw?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [];

    public List<User> Users { get; set; } = [];

    [Required(ErrorMessage = "Required")]
    public List<SelectListItem> ClientList { get; set; } = [];


    public static implicit operator ProjectUpdateForm(ProjectUpdateFormViewModel viewModel)
    {
        return new ProjectUpdateForm
        {
            Id = viewModel.Id,
            ProjectName = viewModel.ProjectName!,
            Description = viewModel.Description,
            StartDate = viewModel.StartDate,
            EndDate = viewModel.EndDate,
            Budget = viewModel.Budget,
            ProjectImage = viewModel.ProjectImage,
            ClientId = viewModel.ClientId,
            ProjectStatusId = viewModel.ProjectStatusId,
            ProjectWithUsers = viewModel.ProjectWithUsers
        };
    }
}