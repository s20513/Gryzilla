using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class AddPostDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    [MaxLength(30,ErrorMessage = "Max length : 30")]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    public string Content { get; set; }
    
    public TagDto [] Tags { get; set; }
}