using Business.Models.Client;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientForm> CreateAsync(ClientRegistrationForm form);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ClientForm>> GetAllClientsAsync();
        Task<ClientForm?> GetClientByIdAsync(int id);
        Task<ClientForm> UpdateAsync(int id, UpdateClientForm updateForm);
    }
}