using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class BlockedUser
    {
        public int IdUser { get; set; }
        public int IdUserBlocked { get; set; }
        public int UserBlockedRankId { get; set; }
        public string? Comment { get; set; }

        public virtual UserDatum IdUserBlockedNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
