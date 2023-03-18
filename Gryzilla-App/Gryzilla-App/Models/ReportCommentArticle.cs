using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class ReportCommentArticle
    {
        public int IdUser { get; set; }
        public int IdCommentArticle { get; set; }
        public int IdReason { get; set; }
        public string Description { get; set; } = null!;
        public DateTime? ReportedAt { get; set; }
        public bool Viewed { get; set; }

        public virtual CommentArticle IdCommentArticleNavigation { get; set; } = null!;
        public virtual Reason IdReasonNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
