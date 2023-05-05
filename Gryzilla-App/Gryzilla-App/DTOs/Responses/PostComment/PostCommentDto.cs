namespace Gryzilla_App.DTOs.Responses.PostComment;

public class PostCommentDto
{
    public int IdComment { get; set; }
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string? Nick { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Type { get; set; }
    public string? base64PhotoData { get; set; }
}