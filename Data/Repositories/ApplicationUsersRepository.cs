using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Data.Repositories;

public class ApplicationUsersRepository(DataContext context, UserManager<ApplicationUserEntity> userManager) : BaseRepository<ApplicationUserEntity>(context), IApplicationUsersRepository
{
    private readonly UserManager<ApplicationUserEntity> _userManager = userManager;

    public async Task<bool> AssignRoleToUserAsync(ApplicationUserEntity user, string roleName)
    {
        try
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error assigning role to user : {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(ApplicationUserEntity user, string roleName)
    {
        try
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error assigning role to user : {ex.Message}");
            return false;
        }
    }
}