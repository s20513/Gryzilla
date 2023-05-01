namespace Gryzilla_App.DTOs.Responses.User;

public class UsersQtyDto
{
    public IEnumerable<UserDto>? Users { get; set; }
    public bool  IsNext { get; set; }
}