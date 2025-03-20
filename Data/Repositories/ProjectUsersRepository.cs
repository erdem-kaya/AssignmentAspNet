using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ProjectUsersRepository(DataContext context) : BaseRepository<ProjectUsersEntity>(context), IProjectUsersRepository
{
}
