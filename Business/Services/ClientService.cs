using Business.Factories;
using Business.Interfaces;
using Business.Models.Client;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class ClientService(ClientsRepository clientRepository) : IClientService
{
    private readonly ClientsRepository _clientRepository = clientRepository;

    public async Task<ClientForm> CreateAsync(ClientRegistrationForm form)
    {
        if (form == null)
            throw new ArgumentNullException(nameof(form), "Client form can't be null");
        await _clientRepository.BeginTransactionAsync();
        try
        {
            var client = ClientFactory.Create(form);
            var result = await _clientRepository.CreateAsync(client);
            await _clientRepository.CommitTransactionAsync();
            return result != null ? ClientFactory.Create(result) : null!;
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Client not created, {ex.Message}");
            return null!;
        }
    }

    public async Task<IEnumerable<ClientForm>> GetAllClientsAsync()
    {
        try
        {
            var allClients = await _clientRepository.GetAllAsync();
            var result = allClients.Select(ClientFactory.Create).ToList();
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting all clients, {ex.Message}");
            return [];
        }
    }

    public async Task<ClientForm?> GetClientByIdAsync(int id)
    {
        try
        {
            var client = await _clientRepository.GetItemAsync(x => x.Id == id);
            return client != null ? ClientFactory.Create(client) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting client by id, {ex.Message}");
            return null;
        }
    }

    public async Task<ClientForm> UpdateAsync(int id, UpdateClientForm updateForm)
    {

        try
        {
            await _clientRepository.BeginTransactionAsync();
            var findClient = await _clientRepository.GetItemAsync(x => x.Id == id) ?? throw new Exception($"Client with id {id} not found");
            ClientFactory.Update(findClient, updateForm);
            var updatedClient = await _clientRepository.UpdateAsync(x => x.Id == id, findClient);
            var result = updatedClient != null ? ClientFactory.Create(updatedClient) : null!;
            await _clientRepository.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Client not updated, {ex.Message}");
            return null!;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _clientRepository.BeginTransactionAsync();
        try
        {
            var deleteClient = await _clientRepository.DeleteAsync(x => x.Id == id);
            if (!deleteClient)
                throw new Exception($"Client with id {id} not found");

            await _clientRepository.CommitTransactionAsync();
            return true;

        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Client not deleted, {ex.Message}");
            return false;
        }
    }
}



