using System;
using System.Collections.Generic;

namespace Gryzilla_App.Models
{
    public partial class Rank
    {
        public Rank()
        {
            UserData = new HashSet<UserDatum>();
        }

        public int IdRank { get; set; }
        public string Name { get; set; } = null!;
        public int RankLevel { get; set; }

        public virtual ICollection<UserDatum> UserData { get; set; }
    }
}
