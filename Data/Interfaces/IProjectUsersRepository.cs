using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IProjectUsersRepository : IBaseRepository<ProjectUsersEntity>
{
    Task<bool> ManyMemberDeleteAsync(Expression<Func<ProjectUsersEntity, bool>> predicate);
}
