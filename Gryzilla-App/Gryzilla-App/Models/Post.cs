using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class Post
    {
        public Post()
        {
            CommentPosts = new HashSet<CommentPost>();
            ReportPosts = new HashSet<ReportPost>();
            IdTags = new HashSet<Tag>();
            IdUsers = new HashSet<UserDatum>();
        }

        public int IdPost { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; } = null!;
        public bool HighLight { get; set; }

        public virtual UserDatum IdUserNavigation { get; set; } = null!;
        public virtual ICollection<CommentPost> CommentPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }

        public virtual ICollection<Tag> IdTags { get; set; }
        public virtual ICollection<UserDatum> IdUsers { get; set; }
    }
}
