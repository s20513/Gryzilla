using System;
using System.Collections.Generic;

namespace Gryzilla_App
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
