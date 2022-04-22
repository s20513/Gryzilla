using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class UserDatum
    {
        public UserDatum()
        {
            AchievementUsers = new HashSet<AchievementUser>();
            Articles = new HashSet<Article>();
            Blockeds = new HashSet<Blocked>();
            CommentArticles = new HashSet<CommentArticle>();
            CommentPosts = new HashSet<CommentPost>();
            Groups = new HashSet<Group>();
            Notifications = new HashSet<Notification>();
            Posts = new HashSet<Post>();
            ProfileCommentIdUserCommentNavigations = new HashSet<ProfileComment>();
            ProfileCommentIdUserNavigations = new HashSet<ProfileComment>();
            ReportCommentArticles = new HashSet<ReportCommentArticle>();
            ReportCommentPosts = new HashSet<ReportCommentPost>();
            ReportPosts = new HashSet<ReportPost>();
            IdArticles = new HashSet<Article>();
            IdGroups = new HashSet<Group>();
            IdPosts = new HashSet<Post>();
            IdUserBlockeds = new HashSet<UserDatum>();
            IdUserFriends = new HashSet<UserDatum>();
            IdUsers = new HashSet<UserDatum>();
            IdUsersNavigation = new HashSet<UserDatum>();
        }

        public int IdUser { get; set; }
        public int IdRank { get; set; }
        public string Nick { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Rank IdRankNavigation { get; set; } = null!;
        public virtual ICollection<AchievementUser> AchievementUsers { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Blocked> Blockeds { get; set; }
        public virtual ICollection<CommentArticle> CommentArticles { get; set; }
        public virtual ICollection<CommentPost> CommentPosts { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ProfileComment> ProfileCommentIdUserCommentNavigations { get; set; }
        public virtual ICollection<ProfileComment> ProfileCommentIdUserNavigations { get; set; }
        public virtual ICollection<ReportCommentArticle> ReportCommentArticles { get; set; }
        public virtual ICollection<ReportCommentPost> ReportCommentPosts { get; set; }
        public virtual ICollection<ReportPost> ReportPosts { get; set; }

        public virtual ICollection<Article> IdArticles { get; set; }
        public virtual ICollection<Group> IdGroups { get; set; }
        public virtual ICollection<Post> IdPosts { get; set; }
        public virtual ICollection<UserDatum> IdUserBlockeds { get; set; }
        public virtual ICollection<UserDatum> IdUserFriends { get; set; }
        public virtual ICollection<UserDatum> IdUsers { get; set; }
        public virtual ICollection<UserDatum> IdUsersNavigation { get; set; }
    }
}
