using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Reason;

public class NewReasonDto
{
    [Required]
    [MaxLength(50,ErrorMessage = "Max length : 50")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Name { get; set; }
}