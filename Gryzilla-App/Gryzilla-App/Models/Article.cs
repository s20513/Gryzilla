namespace Gryzilla_App.Models
{
    public partial class Article
    {
        public Article()
        {
            CommentArticles = new HashSet<CommentArticle>();
            IdTags = new HashSet<Tag>();
            IdUsers = new HashSet<UserDatum>();
        }

        public int IdArticle { get; set; }
        public int IdUser { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual UserDatum IdUserNavigation { get; set; } = null!;
        public virtual ICollection<CommentArticle> CommentArticles { get; set; }

        public virtual ICollection<Tag> IdTags { get; set; }
        public virtual ICollection<UserDatum> IdUsers { get; set; }
    }
}
