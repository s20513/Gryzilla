using Gryzilla_App.DTO.Responses;

namespace Gryzilla_App.DTOs.Responses.Group;

public class GroupDto
{
    public int IdGroup { get; set; }
    public int IdUserCreator { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserDto> Users { get; set; }
}