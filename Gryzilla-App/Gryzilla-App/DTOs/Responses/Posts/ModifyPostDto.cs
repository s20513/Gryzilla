namespace Gryzilla_App.DTO.Responses.Posts;

public class ModifyPostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string? Type { get; set; }
    public string? base64PhotoData { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? [] Tags { get; set; }
}