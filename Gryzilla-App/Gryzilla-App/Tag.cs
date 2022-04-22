using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class Tag
    {
        public Tag()
        {
            IdArticles = new HashSet<Article>();
            IdPosts = new HashSet<Post>();
        }

        public int IdTag { get; set; }
        public string NameTag { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Article> IdArticles { get; set; }
        public virtual ICollection<Post> IdPosts { get; set; }
    }
}
