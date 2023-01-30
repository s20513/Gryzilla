using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class ReportCommentPost
    {
        public int IdComment { get; set; }
        public int IdUser { get; set; }
        public int IdReason { get; set; }
        public string Description { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
        public bool Viewed { get; set; }

        public virtual CommentPost IdCommentNavigation { get; set; } = null!;
        public virtual Reason IdReasonNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
