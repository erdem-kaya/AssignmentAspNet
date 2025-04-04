using Business.Models.Client;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Client;

public class ClientRegistrationFormViewModel
{
    [Display(Name = "Client name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    public string? ClientName { get; set; }

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    public static implicit operator ClientRegistrationForm(ClientRegistrationFormViewModel viewModel)
    {
        return new ClientRegistrationForm
        {
            ClientName = viewModel.ClientName!,
            Email = viewModel.Email,
        };
    }
}
