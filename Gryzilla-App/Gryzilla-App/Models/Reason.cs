namespace Gryzilla_App.Models
{
    public partial class Reason
    {
        public Reason()
        {
            ReportCommentArticles = new HashSet<ReportCommentArticle>();
            ReportCommentPosts = new HashSet<ReportCommentPost>();
            ReportPosts = new HashSet<ReportPost>();
        }

        public int IdReason { get; set; }
        public string ReasonName { get; set; } = null!;

        public virtual ICollection<ReportCommentArticle> ReportCommentArticles { get; set; }
        public virtual ICollection<ReportCommentPost> ReportCommentPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }
    }
}
