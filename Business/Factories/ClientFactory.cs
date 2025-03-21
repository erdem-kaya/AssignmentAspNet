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

    public static Client Create (ClientsEntity entity) => new()
    {
        Id = entity.Id,
        ClientName = entity.ClientName,
        Email = entity.Email,
    };

    public static void Update(ClientsEntity entity, ClientRegistrationForm form)
    {
        entity.ClientName = form.ClientName;
        entity.Email = form.Email;
    }
}
