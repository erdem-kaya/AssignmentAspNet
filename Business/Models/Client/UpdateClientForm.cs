﻿using System.ComponentModel.DataAnnotations;

namespace Business.Models.Client;

public class UpdateClientForm
{
    public int Id { get; set; }

    [Display(Name = "Client name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    public string? ClientName { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter client email address")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}
