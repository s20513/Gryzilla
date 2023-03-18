using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupUsers = new HashSet<GroupUser>();
        }

        public int IdGroup { get; set; }
        public int IdUserCreator { get; set; }
        public string GroupName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual UserDatum IdUserCreatorNavigation { get; set; } = null!;
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
    }
}
