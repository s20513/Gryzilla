using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Requests.User;

public class PutUserDto
{
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    public string Nick { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
}