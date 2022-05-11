using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Group;

public class UserToGroupDto
{
    [Required]
    public int IdGroup { get; set; }
    
    [Required]
    public int IdUser { get; set; }
}