using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ProjectStatusRepository(DataContext context) : BaseRepository<ProjectStatusEntity>(context), IProjectStatusRepository
{
}
