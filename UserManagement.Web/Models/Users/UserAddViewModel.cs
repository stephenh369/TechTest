using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserAddViewModel
{
    [Required(ErrorMessage = "Forename is required.")]
    public required string Forename { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    public required string Surname { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    public bool IsActive { get; set; }

    [Range(typeof(DateTime), "1900-01-01", "2099-12-31", ErrorMessage = "Date of birth must be between 01/01/1900 and 31/12/2099.")]
    public required DateTime DateOfBirth { get; set; }
}
