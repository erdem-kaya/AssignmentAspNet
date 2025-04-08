using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public class ClientsRepository(DataContext context) : BaseRepository<ClientsEntity>(context), IClientsRepository
{
    //public async Task<ClientsEntity?> GetClientWithName(string name)
    //{
    //    if (string.IsNullOrEmpty(name))
    //        return null;
    //    try
    //    {
    //        var client = await _dbSet.FirstOrDefaultAsync(x => x.ClientName == name);
    //        return client;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"Error getting client with name {name}, {ex.Message}");
    //        return null;
    //    }
    //}
}
