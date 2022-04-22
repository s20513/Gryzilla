using System;
using System.Collections.Generic;
using Gryzilla_App.Models;

namespace Gryzilla_App
{
    public partial class Blocked
    {
        public int IdBlocked { get; set; }
        public int IdUser { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }

        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
