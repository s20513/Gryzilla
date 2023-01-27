using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportCommentArticle;

public class DefaultReportCommentArticleDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdCommentArticle{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
}