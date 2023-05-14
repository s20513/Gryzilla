namespace Gryzilla_App.DTOs.Responses.Group;

public class GroupsQtySearchDto
{
    public IEnumerable<GroupDto>? Groups { get; set; }
    public bool IsNext { get; set; }
}