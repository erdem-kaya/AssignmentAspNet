using Business.Models.Client;
using Data.Entities;

namespace Business.Factories;

public class ClientFactory
{
    public static ClientsEntity Create(ClientRegistrationForm form) => new() 
    {
        ClientName = form.ClientName,
        Email = form.Email,
    };

    public static ClientForm Create (ClientsEntity entity) => new()
    {
        Id = entity.Id,
        ClientName = entity.ClientName,
        Email = entity.Email,
    };

    public static void Update(ClientsEntity entity, UpdateClientForm form)
    {
        if (!string.IsNullOrWhiteSpace(form.ClientName))
        {
            entity.ClientName = form.ClientName;
        }

        if (!string.IsNullOrWhiteSpace(form.Email))
        {
            entity.Email = form.Email;
        }
    }
}
