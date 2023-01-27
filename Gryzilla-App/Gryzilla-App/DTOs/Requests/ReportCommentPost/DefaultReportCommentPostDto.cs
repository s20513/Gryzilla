using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportCommentPost;

public class DefaultReportCommentPostDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdComment{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
}