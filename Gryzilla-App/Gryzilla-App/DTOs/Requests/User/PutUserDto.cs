using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Gryzilla_App.DTOs.Requests.User;

public class PutUserDto
{
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(8, ErrorMessage = "Min lenght : 3")]
    public string Nick { get; set; } = null!;

    [Required]
    [PasswordPropertyText]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(8, ErrorMessage = "Min lenght : 8")]
    public string Password { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Phone]
    public string PhoneNumber { get; set; } = null!;
}