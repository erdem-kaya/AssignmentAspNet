using System.ComponentModel.DataAnnotations;

namespace Business.Models.JobTitle;

public class JobTitle
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Your Job Title")]
    [DataType(DataType.Text)]
    public string Title { get; set; } = null!;
}
