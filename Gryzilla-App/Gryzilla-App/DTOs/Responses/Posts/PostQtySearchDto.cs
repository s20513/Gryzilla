namespace Gryzilla_App.DTOs.Responses.Posts;

public class PostQtySearchDto
{
    public IEnumerable<PostSearchDto>? Posts { get; set; }
    public bool  IsNext { get; set; }
}