namespace Gryzilla_App.DTOs.Requests.ReportUser;

public class ReportUserDto
{
    public int idReport { get; set; }
    public int IdUserReported { get; set; }
    
    public int IdUserReporting{ get; set; }
    
    public int IdReason { get; set; }
    
    public string Content{ get; set; }
    
    public string ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}