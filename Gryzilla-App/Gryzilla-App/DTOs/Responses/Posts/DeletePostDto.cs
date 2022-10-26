namespace Gryzilla_App.DTO.Responses.Posts;

public class DeletePostDto
{
    public int IdPost { get; set; }
    public int IdUser { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DeletedAt { get; set; }
}