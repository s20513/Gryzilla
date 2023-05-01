using Gryzilla_App.DTO.Responses;

namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleSearchDto
{
    public int IdArticle { get; set; }
    public ReducedUserResponseDto? Author { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string[]? Tags { get; set; }
    public int? LikesNum { get; set; }
    public int? CommentsNum { get; set; }
}