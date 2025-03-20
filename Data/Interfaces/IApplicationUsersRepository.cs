using Data.Entities;

namespace Data.Interfaces;

public interface IApplicationUsersRepository : IBaseRepository<ApplicationUserEntity>
{
    Task<bool> AssignRoleToUserAsync(ApplicationUserEntity user, string roleName);
    Task<bool> RemoveRoleFromUserAsync(ApplicationUserEntity user, string roleName);
}
