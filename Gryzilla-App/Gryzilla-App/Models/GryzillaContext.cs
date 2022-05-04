using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Gryzilla_App;

namespace Gryzilla_App.Models
{
    public partial class GryzillaContext : DbContext
    {
        private readonly string? _connectionString;
        public GryzillaContext()
        {
            
        }

        public GryzillaContext(DbContextOptions<GryzillaContext> options)
            : base(options)
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GryzillaDatabase-Local"].ConnectionString;
        }

        public virtual DbSet<Achievement> Achievements { get; set; }
        public virtual DbSet<AchievementUser> AchievementUsers { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Blocked> Blockeds { get; set; }
        public virtual DbSet<CommentArticle> CommentArticles { get; set; }
        public virtual DbSet<CommentPost> CommentPosts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<ProfileComment> ProfileComments { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Reason> Reasons { get; set; }
        public virtual DbSet<ReportCommentArticle> ReportCommentArticles { get; set; }
        public virtual DbSet<ReportCommentPost> ReportCommentPosts { get; set; }
        public virtual DbSet<ReportPost> ReportPosts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<UserDatum> UserData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             if (!optionsBuilder.IsConfigured){
                 if (_connectionString != null) optionsBuilder.UseSqlServer(_connectionString);
             }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.HasKey(e => e.IdAchievement)
                    .HasName("Achievement_pk");

                entity.ToTable("Achievement");

                entity.Property(e => e.IdAchievement).HasColumnName("idAchievement");

                entity.Property(e => e.AchievementName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("achievementName");

                entity.Property(e => e.Descripion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripion");

                entity.Property(e => e.Points)
                    .HasColumnType("decimal(6, 4)")
                    .HasColumnName("points");
            });

            modelBuilder.Entity<AchievementUser>(entity =>
            {
                entity.HasKey(e => new { e.IdAchievement, e.IdUser })
                    .HasName("AchievementUser_pk");

                entity.ToTable("AchievementUser");

                entity.Property(e => e.IdAchievement).HasColumnName("idAchievement");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.ReceivedAt)
                    .HasColumnType("date")
                    .HasColumnName("receivedAt");

                entity.HasOne(d => d.IdAchievementNavigation)
                    .WithMany(p => p.AchievementUsers)
                    .HasForeignKey(d => d.IdAchievement)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AchievementUser_Achievement");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.AchievementUsers)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AchievementUser_User");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticle)
                    .HasName("Article_pk");

                entity.ToTable("Article");

                entity.Property(e => e.IdArticle).HasColumnName("idArticle");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("createdAt");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Article_User");

                entity.HasMany(d => d.IdTags)
                    .WithMany(p => p.IdArticles)
                    .UsingEntity<Dictionary<string, object>>(
                        "ArticleTag",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("IdTag").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("ArticleTag_Tag"),
                        r => r.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("ArticleTag_Article"),
                        j =>
                        {
                            j.HasKey("IdArticle", "IdTag").HasName("ArticleTag_pk");

                            j.ToTable("ArticleTag");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("idArticle");

                            j.IndexerProperty<int>("IdTag").HasColumnName("idTag");
                        });
            });

            modelBuilder.Entity<Blocked>(entity =>
            {
                entity.HasKey(e => e.IdBlocked)
                    .HasName("Blocked_pk");

                entity.ToTable("Blocked");

                entity.Property(e => e.IdBlocked).HasColumnName("idBlocked");

                entity.Property(e => e.DateEnded)
                    .HasColumnType("datetime")
                    .HasColumnName("dateEnded");

                entity.Property(e => e.DateStarted)
                    .HasColumnType("datetime")
                    .HasColumnName("dateStarted");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Blockeds)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Blocked_User");
            });

            modelBuilder.Entity<CommentArticle>(entity =>
            {
                entity.HasKey(e => e.IdCommentArticle)
                    .HasName("CommentArticle_pk");

                entity.ToTable("CommentArticle");

                entity.Property(e => e.IdCommentArticle).HasColumnName("idCommentArticle");

                entity.Property(e => e.DescriptionArticle)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descriptionArticle");

                entity.Property(e => e.IdArticle).HasColumnName("idArticle");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.CommentArticles)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_15_Article");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.CommentArticles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_15_User");
            });

            modelBuilder.Entity<CommentPost>(entity =>
            {
                entity.HasKey(e => e.IdComment)
                    .HasName("CommentPost_pk");

                entity.ToTable("CommentPost");

                entity.Property(e => e.IdComment).HasColumnName("idComment");

                entity.Property(e => e.DescriptionPost)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descriptionPost");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.CommentPosts)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_Post");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.CommentPosts)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Comment_User");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.IdGroup)
                    .HasName("Group_pk");

                entity.ToTable("Group");

                entity.Property(e => e.IdGroup).HasColumnName("idGroup");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("createdAt");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("groupName");

                entity.Property(e => e.IdUserCreator).HasColumnName("idUserCreator");

                entity.HasOne(d => d.IdUserCreatorNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.IdUserCreator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Group_User");

                entity.HasMany(d => d.IdUsers)
                    .WithMany(p => p.IdGroups)
                    .UsingEntity<Dictionary<string, object>>(
                        "GroupUser",
                        l => l.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("GroupUser_User"),
                        r => r.HasOne<Group>().WithMany().HasForeignKey("IdGroup").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("GroupUser_Group"),
                        j =>
                        {
                            j.HasKey("IdGroup", "IdUser").HasName("GroupUser_pk");

                            j.ToTable("GroupUser");

                            j.IndexerProperty<int>("IdGroup").HasColumnName("idGroup");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");
                        });
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.IdNotification)
                    .HasName("Notification_pk");

                entity.ToTable("Notification");

                entity.Property(e => e.IdNotification).HasColumnName("idNotification");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Notification_User");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost)
                    .HasName("Post_pk");

                entity.ToTable("Post");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("createdAt");

                entity.Property(e => e.HighLight).HasColumnName("highLight");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Post_User");
            });

            modelBuilder.Entity<ProfileComment>(entity =>
            {
                entity.HasKey(e => e.IdProfileComment)
                    .HasName("ProfileComment_pk");

                entity.ToTable("ProfileComment");

                entity.Property(e => e.IdProfileComment).HasColumnName("idProfileComment");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdUserComment).HasColumnName("idUserComment");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ProfileCommentIdUserNavigations)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserComment");

                entity.HasOne(d => d.IdUserCommentNavigation)
                    .WithMany(p => p.ProfileCommentIdUserCommentNavigations)
                    .HasForeignKey(d => d.IdUserComment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.HasKey(e => e.IdRank)
                    .HasName("Rank_pk");

                entity.ToTable("Rank");

                entity.Property(e => e.IdRank).HasColumnName("idRank");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.RankLevel).HasColumnName("rankLevel");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.HasKey(e => e.IdReason)
                    .HasName("Reason_pk");

                entity.ToTable("Reason");

                entity.Property(e => e.IdReason).HasColumnName("idReason");

                entity.Property(e => e.ReasonName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reasonName");
            });

            modelBuilder.Entity<ReportCommentArticle>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdCommentArticle, e.IdReason })
                    .HasName("ReportCommentArticle_pk");

                entity.ToTable("ReportCommentArticle");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdCommentArticle).HasColumnName("idCommentArticle");

                entity.Property(e => e.IdReason).HasColumnName("idReason");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.ReportedAt)
                    .HasColumnType("date")
                    .HasColumnName("reportedAt");

                entity.Property(e => e.Viewed).HasColumnName("viewed");

                entity.HasOne(d => d.IdCommentArticleNavigation)
                    .WithMany(p => p.ReportCommentArticles)
                    .HasForeignKey(d => d.IdCommentArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_27_CommentArticle");

                entity.HasOne(d => d.IdReasonNavigation)
                    .WithMany(p => p.ReportCommentArticles)
                    .HasForeignKey(d => d.IdReason)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_27_Reason");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ReportCommentArticles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_27_User");
            });

            modelBuilder.Entity<ReportCommentPost>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdComment })
                    .HasName("ReportCommentPost_pk");

                entity.ToTable("ReportCommentPost");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdComment).HasColumnName("idComment");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.IdReason).HasColumnName("idReason");

                entity.Property(e => e.ReportedAt).HasColumnName("reportedAt");

                entity.Property(e => e.Viewed).HasColumnName("viewed");

                entity.HasOne(d => d.IdCommentNavigation)
                    .WithMany(p => p.ReportCommentPosts)
                    .HasForeignKey(d => d.IdComment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReportComment_Comment");

                entity.HasOne(d => d.IdReasonNavigation)
                    .WithMany(p => p.ReportCommentPosts)
                    .HasForeignKey(d => d.IdReason)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReportComment_Reason");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ReportCommentPosts)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ReportComment_User");
            });

            modelBuilder.Entity<ReportPost>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdPost, e.IdReason })
                    .HasName("ReportPost_pk");

                entity.ToTable("ReportPost");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdReason).HasColumnName("idReason");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.ReportedAt)
                    .HasColumnType("date")
                    .HasColumnName("reportedAt");

                entity.Property(e => e.Viewed).HasColumnName("viewed");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_20_Post");

                entity.HasOne(d => d.IdReasonNavigation)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.IdReason)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Report_Reason");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.ReportPosts)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_20_User");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.IdTag)
                    .HasName("Tag_pk");

                entity.ToTable("Tag");

                entity.Property(e => e.IdTag).HasColumnName("idTag");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.NameTag)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nameTag");

                entity.HasMany(d => d.IdPosts)
                    .WithMany(p => p.IdTags)
                    .UsingEntity<Dictionary<string, object>>(
                        "PostTag",
                        l => l.HasOne<Post>().WithMany().HasForeignKey("IdPost").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PostCategory_Post"),
                        r => r.HasOne<Tag>().WithMany().HasForeignKey("IdTag").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PostCategory_Category"),
                        j =>
                        {
                            j.HasKey("IdTag", "IdPost").HasName("PostTag_pk");

                            j.ToTable("PostTag");

                            j.IndexerProperty<int>("IdTag").HasColumnName("idTag");

                            j.IndexerProperty<int>("IdPost").HasColumnName("idPost");
                        });
            });

            modelBuilder.Entity<UserDatum>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("UserData_pk");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("createdAt");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IdRank).HasColumnName("idRank");

                entity.Property(e => e.Nick)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nick");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.HasOne(d => d.IdRankNavigation)
                    .WithMany(p => p.UserData)
                    .HasForeignKey(d => d.IdRank)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_Rank");

                entity.HasMany(d => d.IdArticles)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "LikeArticle",
                        l => l.HasOne<Article>().WithMany().HasForeignKey("IdArticle").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("LikeArticle_Article"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("LikeArticle_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdArticle").HasName("LikeArticle_pk");

                            j.ToTable("LikeArticle");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdArticle").HasColumnName("idArticle");
                        });

                entity.HasMany(d => d.IdPosts)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "LikePost",
                        l => l.HasOne<Post>().WithMany().HasForeignKey("IdPost").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("LikePost_Post"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("LikePost_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdPost").HasName("LikePost_pk");

                            j.ToTable("LikePost");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdPost").HasColumnName("idPost");
                        });

                entity.HasMany(d => d.IdUserBlockeds)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "BlockedUser",
                        l => l.HasOne<UserDatum>().WithMany().HasForeignKey("IdUserBlocked").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BlockedUser_User"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BlockedUser_User2"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdUserBlocked").HasName("BlockedUser_pk");

                            j.ToTable("BlockedUser");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdUserBlocked").HasColumnName("idUserBlocked");
                        });

                entity.HasMany(d => d.IdUserFriends)
                    .WithMany(p => p.IdUsersNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "Friend",
                        l => l.HasOne<UserDatum>().WithMany().HasForeignKey("IdUserFriend").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Friends_User"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FriendUser"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdUserFriend").HasName("Friends_pk");

                            j.ToTable("Friends");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdUserFriend").HasColumnName("idUserFriend");
                        });

                entity.HasMany(d => d.IdUsers)
                    .WithMany(p => p.IdUserBlockeds)
                    .UsingEntity<Dictionary<string, object>>(
                        "BlockedUser",
                        l => l.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BlockedUser_User2"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUserBlocked").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BlockedUser_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdUserBlocked").HasName("BlockedUser_pk");

                            j.ToTable("BlockedUser");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdUserBlocked").HasColumnName("idUserBlocked");
                        });

                entity.HasMany(d => d.IdUsersNavigation)
                    .WithMany(p => p.IdUserFriends)
                    .UsingEntity<Dictionary<string, object>>(
                        "Friend",
                        l => l.HasOne<UserDatum>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FriendUser"),
                        r => r.HasOne<UserDatum>().WithMany().HasForeignKey("IdUserFriend").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Friends_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdUserFriend").HasName("Friends_pk");

                            j.ToTable("Friends");

                            j.IndexerProperty<int>("IdUser").HasColumnName("idUser");

                            j.IndexerProperty<int>("IdUserFriend").HasColumnName("idUserFriend");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
