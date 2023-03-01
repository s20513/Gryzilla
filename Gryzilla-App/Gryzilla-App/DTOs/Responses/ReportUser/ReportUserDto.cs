namespace Gryzilla_App.DTOs.Requests.ReportUser;

public class ReportUserDto
{
    public int idReport { get; set; }
    public int IdUserReported { get; set; }
    
    public int IdUserReporting{ get; set; }
    
    public int IdReason { get; set; }
    
    public string Description{ get; set; }
    
    public DateTime ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}