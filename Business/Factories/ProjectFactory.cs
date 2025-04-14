using Business.Models.Project;
using Business.Models.UserProfile;
using Data.Entities;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectsEntity Create(ProjectRegistrationForm form) => new()
    {
        ProjectName = form.ProjectName,
        Description = form.Description,
        StartDate = form.StartDate,
        EndDate = form.EndDate,
        Budget = form.Budget,
        ProjectImage = form.ProjectImage,
        ProjectStatusId = 1, // Default status
        ClientId = form.ClientId,
    };

    public static ProjectForm Create(ProjectsEntity entity) => new()
    {
        Id = entity.Id,
        ProjectName = entity.ProjectName,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate ?? default,
        Budget = entity.Budget,
        ProjectImage = entity.ProjectImage,
        ClientId = entity.ClientId,
        ProjectStatusId = entity.ProjectStatusId,
    };

    public static void Update(ProjectsEntity entity, ProjectUpdateForm form)
    {
        if (!string.IsNullOrWhiteSpace(form.ProjectName))
        {
            entity.ProjectName = form.ProjectName;
        }
        if (!string.IsNullOrWhiteSpace(form.Description))
        {
            entity.Description = form.Description;
        }
        if (form.StartDate != default)
        {
            entity.StartDate = form.StartDate;
        }
        if (form.EndDate != default)
        {
            entity.EndDate = form.EndDate;
        }
        if (form.Budget > 0)
        {
            entity.Budget = form.Budget;
        }
        if (form.ProjectImage != null)
        {
            entity.ProjectImage = form.ProjectImage;
        }
        if (form.ClientId > 0)
        {
            entity.ClientId = form.ClientId;
        }
        if (form.ProjectStatusId > 0)
        {
            entity.ProjectStatusId = form.ProjectStatusId;
        }
    }
}
