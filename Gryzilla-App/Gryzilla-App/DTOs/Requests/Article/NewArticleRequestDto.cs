using System.ComponentModel.DataAnnotations;
using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Requests.Article;

public class NewArticleRequestDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "Max length : 30")]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public string[]? Tags { get; set; }
}