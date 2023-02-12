using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.DTOs.Responses.Posts;

public class PostQtyDto
{
    public IEnumerable<PostDto>? posts { get; set; }
    public bool  isNext { get; set; }
}