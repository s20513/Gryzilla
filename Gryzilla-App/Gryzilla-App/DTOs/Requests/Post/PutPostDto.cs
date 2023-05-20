using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTO.Responses.Posts;

public class PutPostDto
{
    [Required]
    public int IdPost { get; set; }

    [Required]
    [MaxLength(2000,ErrorMessage = "Max length : 2000")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content { get; set; }
    
    public string [] Tags { get; set; }
}