using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ProfileComment;

public class NewProfileComment
{
    [Required]
    public int IdUser { get; set; }
    [Required]
    public int IdUserComment { get; set; }
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content { get; set; }
}