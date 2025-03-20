using System.ComponentModel.DataAnnotations;

namespace Business.Models.JobTitle;

public class JobTitleUpdateForm
{
    public int Id { get; set; }

    [Display(Name = "Your Job Title")]
    [DataType(DataType.Text)]
    public string? Title { get; set; }
}
