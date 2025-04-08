using Business.Models.Project;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Project;

public class ProjectRegistrationFormViewModel
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project name", Prompt = "Enter the project name")]
    [DataType(DataType.Text)]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Start date", Prompt = "Select a start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "End date", Prompt = "Select an end date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Budget", Prompt = "0 - Enter your budget")]
    [DataType(DataType.Currency)]
    public decimal Budget { get; set; } = 0;

    [Display(Name = "Project image", Prompt = "Upload an image")]
    public string? ProjectImage { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client", Prompt = "Select a client")]
    public int ClientId { get; set; }

    [Required]
    [Display(Name = "Project status", Prompt = "Select a project status")]
    public int ProjectStatusId { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Members", Prompt = "Select a members or members")]
    public List<String> ProjectWithUsers { get; set; } = [];

    public List<SelectListItem> ClientList { get; set; } = [];

    public static implicit operator ProjectRegistrationForm(ProjectRegistrationFormViewModel viewModel)
    {
        return new ProjectRegistrationForm
        {
            ProjectName = viewModel.ProjectName,
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
