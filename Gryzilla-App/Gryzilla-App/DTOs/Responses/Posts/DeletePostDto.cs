namespace Gryzilla_App.DTO.Responses.Posts;

public class DeletePostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string Content { get; set; }
    public string DeletedAt { get; set; }
}