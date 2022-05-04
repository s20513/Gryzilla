namespace Gryzilla_App.DTO.Responses;

public class UserDto
{
    public int IdUser { get; set; }
    public int IdRank { get; set; }
    public string Nick { get; set; } = null!;
    //public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}