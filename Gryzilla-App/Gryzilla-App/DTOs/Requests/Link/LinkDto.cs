using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Link;

public class LinkDto
{
    [Required]
    public int IdUser { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "Max length : 100")]
    public string Link { get; set; }
}