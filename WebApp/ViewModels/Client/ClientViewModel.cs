using Business.Models.Client;

namespace WebApp.ViewModels.Client;

public class ClientViewModel
{
    public ClientRegistrationFormViewModel RegistrationForm { get; set; } = new();
    public ClientUpdateFormViewModel UpdateForm { get; set; } = new();
    public ClientDeleteViewModel Delete { get; set; } = new();
    public IEnumerable<ClientForm> ClientList { get; set; } = [];
}
