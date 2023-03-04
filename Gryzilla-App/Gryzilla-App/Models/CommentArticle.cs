using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class CommentArticle
    {
        public CommentArticle()
        {
            ReportCommentArticles = new HashSet<ReportCommentArticle>();
        }

        public int IdCommentArticle { get; set; }
        public int IdUser { get; set; }
        public int IdArticle { get; set; }
        public string DescriptionArticle { get; set; } = null!;
        
        public DateTime? CreatedAt { get; set; }

        public virtual Article IdArticleNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
        public virtual ICollection<ReportCommentArticle> ReportCommentArticles { get; set; }
    }
}
