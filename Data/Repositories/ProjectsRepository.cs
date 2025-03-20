using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public class ProjectsRepository(DataContext context) : BaseRepository<ProjectsEntity>(context), IProjectsRepository
{
    public async Task<IEnumerable<ProjectsEntity>> GetProjectsByStatusAsync(int statusId)
    {
        try
        {
            var result = await _context.Projects.Where(p => p.ProjectStatusId == statusId).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting projects by status : {ex.Message}");
            return [];
        }
    }
}
