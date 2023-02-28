using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class NewPostCommentDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdPost { get; set; }
    
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    
    public string Content { get; set; }
}