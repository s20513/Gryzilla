
namespace Gryzilla_App.DTO.Responses.Posts;

public class PostDto
{
    public int idPost { get; set; }
    public string Nick { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? [] Tags { get; set; }
    public int? Likes{ get; set; }
    public int? Comments { get; set; }
}