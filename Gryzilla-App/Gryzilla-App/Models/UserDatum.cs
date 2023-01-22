namespace Gryzilla_App.Models
{
    public sealed partial class UserDatum
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
        public Guid? RefreshToken { get; set; }
        public DateTime? TokenExpire { get; set; }

        public Rank IdRankNavigation { get; set; } = null!;
        public ICollection<AchievementUser> AchievementUsers { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Blocked> Blockeds { get; set; }
        public ICollection<CommentArticle> CommentArticles { get; set; }
        public ICollection<CommentPost> CommentPosts { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<ProfileComment> ProfileCommentIdUserCommentNavigations { get; set; }
        public ICollection<ProfileComment> ProfileCommentIdUserNavigations { get; set; }
        public ICollection<ReportCommentArticle> ReportCommentArticles { get; set; }
        public ICollection<ReportCommentPost> ReportCommentPosts { get; set; }
        public ICollection<ReportPost> ReportPosts { get; set; }

        public ICollection<Article> IdArticles { get; set; }
        public ICollection<Group> IdGroups { get; set; }
        public ICollection<Post> IdPosts { get; set; }
        public ICollection<UserDatum> IdUserBlockeds { get; set; }
        public ICollection<UserDatum> IdUserFriends { get; set; }
        public ICollection<UserDatum> IdUsers { get; set; }
        public ICollection<UserDatum> IdUsersNavigation { get; set; }
    }
}
