namespace Gryzilla_App.DTOs.Responses.BlockedUser;

public class BlockedUserDto
{
    public int IdUserBlocked {get; set;}
    public string Nick {get; set;} = null!;
    public int IdRank {get; set;}
    public string RankName {get; set;} = null!;
    public int IdUserBlocking { get; set; }
    public string Start { get; set; }
    public string? End { get; set; }
    public string? Comment { get; set; }
}