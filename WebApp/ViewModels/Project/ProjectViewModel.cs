using Business.Models.Project;

namespace WebApp.ViewModels.Project;

public class ProjectViewModel
{
    public ProjectRegistrationFormViewModel RegistrationForm { get; set; } = new();
    public ProjectUpdateFormViewModel UpdateForm { get; set; } = new();
    public ProjectStatusUpdateViewModel StatusUpdateForm { get; set; } = new();
    public ProjectDeleteViewModel Delete { get; set; } = new();
    public IEnumerable<ProjectForm> ProjectList { get; set; } = [];
}
