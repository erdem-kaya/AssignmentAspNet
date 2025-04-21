using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace WebApp.ViewModels.Project;

public class ProjectStatusUpdateViewModel
{
    public int Id { get; set; }

    [DisplayName("Project Status")]
    public int ProjectStatusId { get; set; }

    public List<SelectListItem> ProjectStatusList { get; set; } = [];
}
