using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ProfileComment;

public class ModifyProfileComment
{
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content { get; set; }
}
