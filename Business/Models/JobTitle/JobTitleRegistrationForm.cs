using System.ComponentModel.DataAnnotations;

namespace Business.Models.JobTitle;

public class JobTitleRegistrationForm
{
    [Required]
    [Display(Name = "Your Job Title")]
    [DataType(DataType.Text)]
    public string Title { get; set; } = null!;
}
