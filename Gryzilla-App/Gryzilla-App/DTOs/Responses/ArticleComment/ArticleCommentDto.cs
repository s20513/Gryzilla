namespace Gryzilla_App.DTOs.Responses.ArticleComment;

public class ArticleCommentDto
{
    public int IdComment { get; set; }
    public int IdArticle { get; set; }
    public int IdUser { get; set; }
    public string? Nick { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Type { get; set; }
    public string? base64PhotoData { get; set; }
}