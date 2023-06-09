using System.ComponentModel.DataAnnotations;
using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Requests.Article;

public class PutArticleRequestDto
{
    [Required]
    public int IdArticle { get; set; }
    
    [Required]
    [MaxLength(100, ErrorMessage = "Max length : 30")]
    public string Title { get; set; }
    
    public string Content { get; set; }

    public string[]? Tags { get; set; }
}