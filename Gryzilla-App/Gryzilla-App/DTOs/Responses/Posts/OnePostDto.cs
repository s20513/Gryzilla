using Gryzilla_App.DTOs.Responses.PostComment;

namespace Gryzilla_App.DTO.Responses.Posts;

public class OnePostDto
{
    public int idPost { get; set; }
    public string Nick { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? [] Tags { get; set; }
    
    public int? CommentsNumber{ get; set; }
    public PostCommentDto? [] Comments { get; set; }
    public int? Likes{ get; set; }
}