using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Tag;

public class NewTagDto
{
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    [MinLength(1,ErrorMessage = "Min length : 1")]
    public string Name { get; set; }
}