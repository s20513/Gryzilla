using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ArticleComment;

public class NewArticleCommentDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdArticle { get; set; }
    
    [Required]
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string Description { get; set; }
}