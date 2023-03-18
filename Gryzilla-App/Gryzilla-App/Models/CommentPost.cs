using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class CommentPost
    {
        public CommentPost()
        {
            ReportCommentPosts = new HashSet<ReportCommentPost>();
        }

        public int IdComment { get; set; }
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public string DescriptionPost { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual Post IdPostNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
        public virtual ICollection<ReportCommentPost> ReportCommentPosts { get; set; }
    }
}
