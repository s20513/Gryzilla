namespace Gryzilla_App.DTOs.Responses.ReportCommentArticle;

public class ReportCommentArticleDto
{
    public int IdUser { get; set; }
    
    public int IdComment{ get; set; }
    
    public int IdReason { get; set; }
    
    public string Content{ get; set; }
    
    public DateTime ReportedAt { get; set; }
    
    public bool Viewed { get; set; }
}