using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class JobTitlesRepository(DataContext context) : BaseRepository<JobTitlesEntity>(context), IJobTitlesRepository
{
}
