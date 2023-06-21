namespace Gryzilla_App.DTOs.Responses.GroupUserMessageDto;

public class GroupUserMessageDto
{
    public int IdMessage { get; set; }
    public int IdGroup { get; set; }
    public int IdUser { get; set; }
    public string? Nick { get; set; }
    public string Content { get; set; }
    public string? Type { get; set; }
    public string? base64PhotoData { get; set; }
    public DateTime CreatedAt { get; set; }
}