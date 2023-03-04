using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Responses.Posts;

public class PostQtyDto
{
    public IEnumerable<PostDto>? Posts { get; set; }
    public bool  IsNext { get; set; }
}