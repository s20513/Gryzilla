using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.Article;

public class PutArticleRequestDto
{
    [Required]
    public int IdPost { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "Max length : 30")]
    public string? Title { get; set; }
    
    [MaxLength(200, ErrorMessage = "Max length : 200")]
    public string? Content { get; set; }
}