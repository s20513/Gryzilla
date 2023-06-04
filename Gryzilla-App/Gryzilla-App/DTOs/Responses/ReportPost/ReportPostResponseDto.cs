namespace Gryzilla_App.DTOs.Responses.ReportPost;

public class ReportPostResponseDto
{
    public int IdUser { get; set; }
    public string? NickReported { get; set; }
    public int IdUserReported { get; set; }
    
    public int IdPost{ get; set; }
    
    public int IdReason { get; set; }
    public string? ReasonName { get; set; }
    
    public string Content{ get; set; }
    
    public DateTime ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}