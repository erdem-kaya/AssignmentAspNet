using Business.Models.Project;
using System.ComponentModel.DataAnnotations;

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

    [Display(Name = "Members", Prompt = "Select a members or members")]
    public List<String> ProjectWithUsers { get; set; } = [];


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