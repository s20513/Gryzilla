using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class ProfileComment
    {
        public int IdProfileComment { get; set; }
        public int IdUser { get; set; }
        public int IdUserComment { get; set; }
        public string Description { get; set; } = null!;

        public virtual UserDatum IdUserCommentNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}
