using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class ReportProfileComment
    {
        public int IdProfileComment { get; set; }
        public int IdUser { get; set; }
        public int IdReason { get; set; }
        public string Description { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
        public bool Viewed { get; set; }

        public virtual ProfileComment IdProfileCommentNavigation { get; set; } = null!;
        public virtual Reason IdReasonNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
