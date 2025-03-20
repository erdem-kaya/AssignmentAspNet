using Data.Entities;

namespace Data.Interfaces;

public interface IProjectsRepository : IBaseRepository<ProjectsEntity>
{
    Task<IEnumerable<ProjectsEntity>> GetProjectsByStatusAsync(int statusId);
}
