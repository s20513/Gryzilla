namespace Gryzilla_App.DTOs.Responses.ReportProfileComment;

public class ReportProfileCommentResponseDto
{
    public int IdUser { get; set; }
    public string? NickReported { get; set; }
    public int IdUserReported { get; set; }
    
    public int IdProfileComment{ get; set; }
    
    public int IdReason { get; set; }
    
    public string? ReasonName { get; set; }
    
    public string Content{ get; set; }
    
    public DateTime ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}