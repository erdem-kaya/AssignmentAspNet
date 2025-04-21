using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class RoleViewModel
{
    public string UserId { get; set; } = null!;
    public string Roles { get; set; } = null!;
    public List<SelectListItem> RoleList { get; set; } = [];
}
