namespace Gryzilla_App.DTOs.Requests.ReportUser;

public class ModifyReportUser
{
    public int IdReport { get; set; }
    
    public int IdReason { get; set; }
    
    public string Content{ get; set; }
    
    public bool Viewed { get; set; }

}