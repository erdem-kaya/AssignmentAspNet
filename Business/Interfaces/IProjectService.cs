using Business.Models.Project;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectForm> CreateAsync(ProjectRegistrationForm form);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProjectForm>> GetAllProjectsAsync();
        Task<ProjectForm?> GetProjectByIdAsync(int id);
        Task<ProjectForm> UpdateAsync(int id, ProjectUpdateForm updateForm);
    }
}