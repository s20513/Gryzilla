using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class AddCommentDto
{
    [Required]
    public int IdUser { get; set; }
    [Required]
    public int IdPost { get; set; }
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string DescriptionPost { get; set; }
}