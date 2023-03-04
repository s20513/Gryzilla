namespace Gryzilla_App.DTO.Responses.Posts;

public class ModifyPostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string Content { get; set; }
    public string CreatedAt { get; set; }
    public string? [] Tags { get; set; }
}