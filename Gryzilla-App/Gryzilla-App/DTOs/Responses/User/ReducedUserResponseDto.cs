namespace Gryzilla_App.DTO.Responses;

public class ReducedUserResponseDto
{
    public int IdUser { get; set; }
    public string? Nick { get; set; }
    public string? Type { get; set; }
    public string? base64PhotoData { get; set; }
}