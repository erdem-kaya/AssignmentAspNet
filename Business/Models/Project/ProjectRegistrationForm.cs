using System.ComponentModel.DataAnnotations;

namespace Business.Models.Project;

public class ProjectRegistrationForm
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project name", Prompt = "Enter the project name")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; } 

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Budget", Prompt = "0 - Enter your budget")]
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


}
