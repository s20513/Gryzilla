using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Responses.User;

namespace Gryzilla_App.DTOs.Responses.Group;

public class GroupDto
{
    public int IdGroup { get; set; }
    public int IdUserCreator { get; set; }
    public string GroupName { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Type { get; set; }
    public string base64PhotoData { get; set; }
    public List<UserDto> Users { get; set; }
    
    
}