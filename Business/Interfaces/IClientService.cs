using Business.Models.Client;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(ClientRegistrationForm form);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> UpdateAsync(UpdateClientForm updateForm);
    }
}