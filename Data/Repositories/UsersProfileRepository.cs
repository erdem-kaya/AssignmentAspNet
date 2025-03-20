using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class UsersProfileRepository(DataContext context) : BaseRepository<UsersProfileEntity>(context), IUsersProfileRepository
{
}
