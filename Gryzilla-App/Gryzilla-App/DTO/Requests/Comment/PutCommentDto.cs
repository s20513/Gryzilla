using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class PutCommentDto
{
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string DescriptionPost { get; set; }
}