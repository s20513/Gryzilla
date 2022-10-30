using System.ComponentModel.DataAnnotations;
using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Requests.Article;

public class PutArticleRequestDto
{
    [Required]
    public int IdArticle { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "Max length : 30")]
    public string Title { get; set; }
    
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string Content { get; set; }

    public List<TagDto>? Tags { get; set; }
}