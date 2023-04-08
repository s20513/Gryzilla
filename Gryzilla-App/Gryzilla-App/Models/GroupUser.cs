using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class GroupUser
    {
        public GroupUser()
        {
            GroupUserMessages = new HashSet<GroupUserMessage>();
        }

        public int IdGroup { get; set; }
        public int IdUser { get; set; }
        public int? Siema { get; set; }

        public virtual Group IdGroupNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
        public virtual ICollection<GroupUserMessage> GroupUserMessages { get; set; }
    }
}
