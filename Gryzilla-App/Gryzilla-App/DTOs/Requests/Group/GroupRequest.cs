using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Group;

public class GroupRequest
{
    [Required]
    [MaxLength(30, ErrorMessage = "Max length : 30")]
    public string GroupName { get; set; }
    
    [Required]
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string Description { get; set; }

}