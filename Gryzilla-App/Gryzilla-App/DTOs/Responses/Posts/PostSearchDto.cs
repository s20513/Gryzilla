namespace Gryzilla_App.DTOs.Responses.Posts;

public class PostSearchDto
{
    public int idPost { get; set; }
    public string Nick { get; set; }
    public string? Type { get; set; }
    public string base64PhotoData { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? [] Tags { get; set; }
    public int? Likes{ get; set; }
    public int? CommentsNumber{ get; set; }
}