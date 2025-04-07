using Business.Models.Client;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Client;

public class ClientUpdateFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client name", Prompt = "Enter client last name")]
    [DataType(DataType.Text)]
    public string? ClientName { get; set; }

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter client email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public static implicit operator UpdateClientForm(ClientUpdateFormViewModel viewModel)
    {
        return new UpdateClientForm
        {
            Id = viewModel.Id,
            ClientName = viewModel.ClientName!,
            Email = viewModel.Email!,
        };
    }
}
