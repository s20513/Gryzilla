using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ArticleComment;

public class PutArticleCommentDto
{
    [Required]
    public int IdComment { get; set; }

    [Required]
    public int IdArticle { get; set; }
    
    [Required]
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string Content { get; set; }
}