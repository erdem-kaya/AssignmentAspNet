using Business.Factories;
using Business.Interfaces;
using Business.Models.Project;
using Business.Models.UserProfile;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(ProjectsRepository projectsRepository, ProjectUsersRepository projectsUsersRepository, ClientsRepository clientsRepository, UsersProfileRepository usersProfileRepository, ProjectStatusRepository projectStatusRepository, INotificationService notificationService) : IProjectService
{
    private readonly ProjectsRepository _projectsRepository = projectsRepository;
    private readonly ProjectUsersRepository _projectsUsersRepository = projectsUsersRepository;
    private readonly ClientsRepository _clientsRepository = clientsRepository;
    private readonly UsersProfileRepository _usersProfileRepository = usersProfileRepository;
    private readonly ProjectStatusRepository _projectStatusRepository = projectStatusRepository;
    private readonly INotificationService _notificationService = notificationService;


    public async Task<ProjectForm> CreateAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project form can't be null");
        try
        {
            await _projectsRepository.BeginTransactionAsync();
            var project = ProjectFactory.Create(form);
            var result = await _projectsRepository.CreateAsync(project);
            if (result == null)
                throw new ArgumentNullException(nameof(result), "Project not created");

            if (form.ProjectWithUsers?.Count > 0)
            {
                foreach (var projectWithUser in form.ProjectWithUsers)
                {
                    var projectUser = new ProjectUsersEntity
                    {
                        ProjectId = result.Id,
                        UserId = projectWithUser,
                    };
                    var projectUserResult = await _projectsUsersRepository.CreateAsync(projectUser);
                }
            }

            // Om projectet skapades om användaren är en admin, skicka en notifikation till admins
            var notification = new NotificationEntity
            {
                Message = $"A new project '{form.ProjectName}' has been created.",
                CreatedAt = DateTime.Now,
                NotificationTypeId = 2,
                TargetGroupId = 2,
            };
            await _notificationService.AddNotificationAsync(notification, "admin");



            await _projectsRepository.CommitTransactionAsync();
            return result != null ? ProjectFactory.Create(result) : null!;
        }
        catch (Exception ex)
        {
            await _projectsRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Project not created, {ex.Message}");
            return null!;
        }
    }

    public async Task<IEnumerable<ProjectForm>> GetAllProjectsAsync()
    {
        try
        {
            // Jag gillar inte den här kodstrukturen ... det skulle vara renare att skriva en separat anpassad klass. Jag ska titta på detta senare.
            var allProjects = await _projectsRepository.GetAllAsync();
            var clientNames = await _clientsRepository.GetAllAsync();
            var allProjectStatus = await _projectStatusRepository.GetAllAsync();
            var allProjectUsers = await _projectsUsersRepository.GetAllAsync();
            var allUserProfiles = await _usersProfileRepository.GetAllAsync();

            var viewProject = allProjects.OrderBy(project => project.ProjectName).ToList();

            var result = viewProject.Select(project =>
            {
                var projectForm = ProjectFactory.Create(project);

                var projectUsers = allProjectUsers
                //chatgpt hjälpde mig med att skriva den här koden
                    .Where(pu => pu.ProjectId == project.Id)
                    .Select(pu => allUserProfiles.FirstOrDefault(up => up.Id == pu.UserId))
                    .Where(up => up != null)
                    .Select(up => new User
                    {
                        Id = up!.Id,
                        FirstName = up.FirstName,
                        LastName = up.LastName,
                        ProfilePicture = up.ProfilePicture,
                    }).ToList();

                var client = clientNames.FirstOrDefault(c => c.Id == project.ClientId);
                if (client != null)
                {
                    projectForm.ClientName = client.ClientName;
                }

                var projectStatus = allProjectStatus.FirstOrDefault(ps => ps.Id == project.ProjectStatusId);
                if (projectStatus != null)
                {
                    projectForm.ProjectStatusName = projectStatus.StatusName;
                }

                projectForm.Users = projectUsers;
                return projectForm;
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting all projects, {ex.Message}");
            return [];
        }
    }

    public async Task<ProjectForm?> GetProjectByIdAsync(int id)
    {
        try
        {
            var project = await _projectsRepository.GetItemAsync(x => x.Id == id) ?? throw new ArgumentNullException(nameof(id), "Project not found");
            var projectForm = ProjectFactory.Create(project);

            var members = await _projectsUsersRepository.GetAllAsync();
            var allUserProfiles = await _usersProfileRepository.GetAllAsync();

            var projectUsers = members
                .Where(pu => pu.ProjectId == project!.Id)
                .Select(pu => allUserProfiles.FirstOrDefault(up => up.Id == pu.UserId))
                .Where(up => up != null)
                .Select(up => new User
                {
                    Id = up!.Id,
                    FirstName = up.FirstName,
                    LastName = up.LastName,
                    ProfilePicture = up.ProfilePicture,
                }).ToList();



            projectForm.Users = projectUsers;
            return projectForm;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting project by id, {ex.Message}");
            return null;
        }
    }

    public async Task<ProjectForm> UpdateAsync(int id, ProjectUpdateForm updateForm)
    {
        try
        {


            var findProject = await _projectsRepository.GetItemAsync(x => x.Id == id)
                ?? throw new ArgumentNullException(nameof(id), "Project not found");

            ProjectFactory.Update(findProject, updateForm);
            var updateProject = await _projectsRepository.UpdateAsync(x => x.Id == id, findProject);
            await _projectsUsersRepository.ManyMemberDeleteAsync(x => x.ProjectId == id);


            if (updateForm.ProjectWithUsers?.Count > 0)
            {
                foreach (var userId in updateForm.ProjectWithUsers)
                {
                    var newMember = new ProjectUsersEntity
                    {
                        ProjectId = id,
                        UserId = userId,
                    };
                    await _projectsUsersRepository.CreateAsync(newMember);
                }
            }

            var result = updateProject != null ? ProjectFactory.Create(updateProject) : null!;
            return result;
        }
        catch (Exception ex)
        {

            Debug.WriteLine($"Project not updated, {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _projectsRepository.BeginTransactionAsync();
            var deleteProject = await _projectsRepository.DeleteAsync(x => x.Id == id);
            if (!deleteProject)
                throw new ArgumentNullException(nameof(id), "Project not found");
            await _projectsRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            await _projectsRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Project not deleted, {ex.Message}");
            return false;
        }
    }


    public async Task<List<SelectListItem>> GetProjectStatusAsync()
    {
        var statusList = await _projectStatusRepository.GetAllAsync();

        return statusList
            .Select(status => new SelectListItem
            {
                Value = status.Id.ToString(),
                Text = status.StatusName
            })
            .ToList();
    }

    public async Task<bool> UpdateProjectStatusAsync(int projectId, int statusId)
    {
        try
        {
            var project = await _projectsRepository.GetItemAsync(x => x.Id == projectId) ?? throw new ArgumentNullException(nameof(projectId), "Project not found");

            project.ProjectStatusId = statusId;
            await _projectsRepository.UpdateAsync(x => x.Id == projectId, project);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating project status, {ex.Message}");
            return false;
        }
    }

}
