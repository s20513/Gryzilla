
namespace Gryzilla_App.DTO.Responses.Posts;

public class PostDto
{
    public string Nick { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto? [] tags { get; set; }
    public int? likes{ get; set; }
}