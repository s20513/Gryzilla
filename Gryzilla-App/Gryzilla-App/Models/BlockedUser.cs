using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public class BlockedUser
    {
        public int IdUser { get; set; }
        public int IdUserBlocked { get; set; }
        public string? Comment { get; set; }
        public int UserBlockedRankId { get; set; }

        public virtual UserDatum IdUserBlockedNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
