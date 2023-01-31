namespace Gryzilla_App.DTOs.Responses;

public class ProfileCommentDto
{
    public int IdUserAccount { get; set; }
    public int IdUser { get; set; }
    public string Nick { get; set; }
    public string Description { get; set; }
}