namespace Gryzilla_App.DTOs.Responses.BlockedUser;

public class BlockingUserDto
{
    public int IdUserBlocking { get; set; }
    public string UserBlockingNick { get; set; }
    public int UserBlockingIdRank { get; set; }
    public string UserBlockingRankName { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string? Comment { get; set; }
}