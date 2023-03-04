namespace Gryzilla_App.DTOs.Requests.ReportUser;

public class NewReportUserDto
{
    public int IdUserReported { get; set; }
    
    public int IdUserReporting{ get; set; }
    
    public int IdReason { get; set; }
    
    public string Content{ get; set; }
    
}