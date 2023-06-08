namespace Gryzilla_App.DTOs.Responses.User;

public class SearchUserDto
{
    public int IdUser {get; set;}
    public string Nick {get; set;} = null!;
    public int IdRank {get; set;}
    public string RankName {get; set;} = null!;
}