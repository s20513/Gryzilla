namespace Gryzilla_App.DTOs.Responses.BlockedUser;

public class UserBlockingHistoryDto
{
    public int IdUser {get; set;}
    public string Nick {get; set;}
    public int IdRank {get; set;}
    public string RankName {get; set;} = null!;
    public List<BlockingUserDto> History { get; set; }
}