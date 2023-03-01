using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportPost;

public class ReportPostIdsRequestDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdPost{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
}