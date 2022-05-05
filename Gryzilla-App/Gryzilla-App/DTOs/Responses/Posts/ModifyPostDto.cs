namespace Gryzilla_App.DTO.Responses.Posts;

public class ModifyPostDto
{
    public int idPost { get; set; }
    public int idUser { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public TagDto? [] Tags { get; set; }
}