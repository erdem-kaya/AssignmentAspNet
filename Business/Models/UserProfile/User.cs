﻿using System.ComponentModel.DataAnnotations;

namespace Business.Models.UserProfile;

public class User
{
    public string Id { get; set; } = null!;

    [Display(Name = "Profile picture")]
    [DataType(DataType.ImageUrl)]
    public string? ProfilePicture { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "First name", Prompt = "Enter your first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last name", Prompt = "Enter your last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^((\+46|0046|07)[0-9]{8})$", ErrorMessage = "Invalid Swedish phone number format")]
    [Display(Name = "Phone number", Prompt = "Enter your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job title", Prompt = "Enter your job title")]
    [DataType(DataType.Text)]
    public string? JobTitle { get; set; }

    [Display(Name = "Address", Prompt = "Enter your address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }

    [Display(Name = "City", Prompt = "Enter your City")]
    [DataType(DataType.Text)]
    public string? City { get; set; }

    public DateTime Birthday { get; set; }
}
