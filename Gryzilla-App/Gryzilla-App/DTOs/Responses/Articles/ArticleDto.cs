using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Responses.Articles;

public class ArticleDto
{
    public int IdArticle { get; set; }
    public int IdUser { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto[]? Tags { get; set; }
    public int LikesNum { get; set; }
    public int CommentsNum { get; set; }
}