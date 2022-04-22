using System;
using System.Collections.Generic;
using Gryzilla_App.Models;

namespace Gryzilla_App
{
    public partial class Group
    {
        public Group()
        {
            IdUsers = new HashSet<UserDatum>();
        }

        public int IdGroup { get; set; }
        public int IdUserCreator { get; set; }
        public string GroupName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual UserDatum IdUserCreatorNavigation { get; set; } = null!;

        public virtual ICollection<UserDatum> IdUsers { get; set; }
    }
}
