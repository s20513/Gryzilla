namespace Gryzilla_App.DTOs.Requests.User;

public class ChangePasswordDto
{
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}