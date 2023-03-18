using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class ReportPost
    {
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public int IdReason { get; set; }
        public DateTime ReportedAt { get; set; }
        public string Description { get; set; } = null!;
        public bool Viewed { get; set; }

        public virtual Post IdPostNavigation { get; set; } = null!;
        public virtual Reason IdReasonNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
