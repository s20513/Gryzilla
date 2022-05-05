using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Group;

public class UserToGroup
{
    [Required]
    public int IdGroup { get; set; }
    
    [Required]
    public int IdUser { get; set; }
}