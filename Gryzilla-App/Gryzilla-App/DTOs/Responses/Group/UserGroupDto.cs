namespace Gryzilla_App.DTOs.Responses.Group;

public class UserGroupDto
{
    public int IdGroup { get; set; }
    public int IdUserCreator { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}