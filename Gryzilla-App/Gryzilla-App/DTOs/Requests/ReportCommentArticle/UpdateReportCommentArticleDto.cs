using System.ComponentModel.DataAnnotations;

namespace Gryzilla_App.DTOs.Requests.ReportCommentArticle;

public class UpdateReportCommentArticleDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required]
    public int IdCommentArticle{ get; set; }
    
    [Required]
    public int IdReason { get; set; }
    
    [Required]
    [MaxLength(200,ErrorMessage = "Max length : 200")]
    [MinLength(2,ErrorMessage = "Min length : 2")]
    public string Content{ get; set; }
    
    public bool Viewed { get; set; }
}