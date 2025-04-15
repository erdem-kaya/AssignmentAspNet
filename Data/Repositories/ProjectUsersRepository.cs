using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ProjectUsersRepository(DataContext context) : BaseRepository<ProjectUsersEntity>(context), IProjectUsersRepository
{
    public async Task<bool> ManyMemberDeleteAsync(Expression<Func<ProjectUsersEntity, bool>> predicate)
    {
        var items = await _context.Set<ProjectUsersEntity>().Where(predicate).ToListAsync();
        if (items.Count != 0)
        {
            _context.Set<ProjectUsersEntity>().RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
