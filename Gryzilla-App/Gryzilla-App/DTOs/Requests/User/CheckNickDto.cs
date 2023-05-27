using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.User;

public class CheckNickDto
{
    [Required]
    public string Nick { get; set; }
}