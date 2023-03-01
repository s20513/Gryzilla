using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class ReportUser
    {
        public int IdReport { get; set; }
        public string Description { get; set; } = null!;
        public DateTime ReportedAt { get; set; }
        public bool Viewed { get; set; }
        public int IdUserReporting { get; set; }
        public int IdUserReported { get; set; }
        public int IdReason { get; set; }

        public virtual Reason IdReasonNavigation { get; set; } = null!;
        public virtual UserDatum IdUserReportedNavigation { get; set; } = null!;
        public virtual UserDatum IdUserReportingNavigation { get; set; } = null!;
    }
}
