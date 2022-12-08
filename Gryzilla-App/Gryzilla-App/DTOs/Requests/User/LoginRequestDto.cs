using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.User;

public class LoginRequestDto
{
    [Required]
    public string Nick { get; set; }
    
    [Required]
    public string Password { get; set; }
}