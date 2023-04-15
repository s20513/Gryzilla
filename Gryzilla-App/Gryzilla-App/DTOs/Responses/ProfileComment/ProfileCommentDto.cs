namespace Gryzilla_App.DTOs.Responses;

public class ProfileCommentDto
{
    public int idProfileComment { get; set; }
    public int IdUser{ get; set; }
    public int IdUserComment { get; set; }
    public string? Nick { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}