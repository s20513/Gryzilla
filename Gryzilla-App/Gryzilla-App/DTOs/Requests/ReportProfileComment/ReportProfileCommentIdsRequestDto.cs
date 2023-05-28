using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportProfileComment;

public class ReportProfileCommentIdsRequestDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdProfileComment{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
}