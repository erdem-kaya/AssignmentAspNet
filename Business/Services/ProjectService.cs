using Business.Factories;
using Business.Interfaces;
using Business.Models.Project;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(ProjectsRepository projectsRepository) : IProjectService
{
    private readonly ProjectsRepository _projectsRepository = projectsRepository;

    public async Task<ProjectForm> CreateAsync(ProjectRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Project form can't be null");
        try
        {
            await _projectsRepository.BeginTransactionAsync();
            var project = ProjectFactory.Create(form);
            var result = await _projectsRepository.CreateAsync(project);
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
            var allProjects = await _projectsRepository.GetAllAsync();
            var result = allProjects.Select(ProjectFactory.Create).ToList();
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
            var project = await _projectsRepository.GetItemAsync(x => x.Id == id);
            return project != null ? ProjectFactory.Create(project) : null;
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
            await _projectsRepository.BeginTransactionAsync();
            var findProject = await _projectsRepository.GetItemAsync(x => x.Id == id) ?? throw new ArgumentNullException(nameof(id), "Project not found");
            ProjectFactory.Update(findProject, updateForm);
            var updateProject = await _projectsRepository.UpdateAsync(x => x.Id == id, findProject);
            var result = updateProject != null ? ProjectFactory.Create(updateProject) : null!;
            await _projectsRepository.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await _projectsRepository.RollbackTransactionAsync();
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
}
