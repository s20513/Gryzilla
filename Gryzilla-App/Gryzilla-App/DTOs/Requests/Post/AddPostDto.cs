using System.ComponentModel.DataAnnotations;
using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Requests.Post;

public class AddPostDto
{
    [Required]
    public int IdUser { get; set; }

    [Required]
    [MaxLength(2000,ErrorMessage = "Max length : 2000")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content { get; set; }
    
    public string [] Tags { get; set; }
}