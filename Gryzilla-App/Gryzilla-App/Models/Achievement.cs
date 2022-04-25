﻿namespace Gryzilla_App.Models
{
    public partial class Achievement
    {
        public Achievement()
        {
            AchievementUsers = new HashSet<AchievementUser>();
        }

        public int IdAchievement { get; set; }
        public decimal Points { get; set; }
        public string Descripion { get; set; } = null!;
        public string AchievementName { get; set; } = null!;

        public virtual ICollection<AchievementUser> AchievementUsers { get; set; }
    }
}
