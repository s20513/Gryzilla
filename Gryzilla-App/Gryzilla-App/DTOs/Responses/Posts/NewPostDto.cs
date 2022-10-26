namespace Gryzilla_App.DTO.Responses.Posts;

public class NewPostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto? [] Tags { get; set; }
}