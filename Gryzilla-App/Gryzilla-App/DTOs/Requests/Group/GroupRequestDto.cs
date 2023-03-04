using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Group;

public class GroupRequestDto
{
    [Required]
    public int IdGroup { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "Max length : 30")]
    public string GroupName { get; set; }
    
    [Required]
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string Content { get; set; }

}