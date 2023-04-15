namespace Gryzilla_App.DTOs.Requests.GroupUserMessage;

public class AddGroupUserMessageDto
{
    public int IdUser { get; set; }
    public int IdGroup { get; set; }
    public string Content { get; set; }
}