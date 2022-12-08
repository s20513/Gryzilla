namespace Gryzilla_App.DTOs.Responses.User;

public class LoginResponseDto
{
    public int Id { get; set; }
    public string Nick { get; set; }

    public string Token { get; set; }
}