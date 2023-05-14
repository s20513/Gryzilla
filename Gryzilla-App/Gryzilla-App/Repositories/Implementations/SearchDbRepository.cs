using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class SearchDbRepository : ISearchDbRepository
{
    private readonly GryzillaContext _context;

    public SearchDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<UsersQtyDto?> GetUsersByNameFromDb(int qtyUsers, DateTime time,string nickName)
    {
        if (qtyUsers < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var users = await _context.UserData
            .Where(x=>x.Nick.ToLower().Contains(nickName))
            .Include(x => x.IdRankNavigation)
            .Select(x => new UserDto 
            {
                IdUser      = x.IdUser,
                Nick        = x.Nick,
                Email       = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt   = x.CreatedAt,
                IdRank      = x.IdRank,
                RankName    = x.IdRankNavigation.Name,
                LinkDiscord = x.DiscordLink,
                LinkSteam   = x.SteamLink,
                LinkEpic    = x.EpicLink,
                LinkXbox    = x.XboxLink,
                LinkPs      = x.PsLink
            })
            .ToArrayAsync();

        var filteredUserDtos = users
            .OrderBy(e => e.CreatedAt)
            .Skip(qtyUsers - 5)
            .Take(5)
            .ToList();

        return new UsersQtyDto()
        {
            Users = filteredUserDtos,
            IsNext = qtyUsers < users.Length
        };
    }
    
    public async Task<PostQtySearchDto?> GetPostByWordFromDb(int qtyPosts, DateTime time,string word)
    {
        var idPostsTable = new List<int>();
        var allSearchPosts = new List<PostSearchDto>();
        
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var posts = await _context.Posts
            .Where(x => x.Content.ToLower().Contains(word))
            .Select(x=>x.IdPost)
            .ToArrayAsync();

        for (int i = 0; i < posts.Length; i++)
        {
            var post = await GetPosts(posts, i);

            if (!idPostsTable.Contains(post.idPost))
            {
                idPostsTable.Add(post.idPost);
                allSearchPosts.Add(post);
            }
        }

        posts = await _context.Tags
            .Where(x => x.NameTag.ToLower().Contains(word))
            .SelectMany(x => x.IdPosts)
            .Select(x => x.IdPost).ToArrayAsync();
        
        for (int i = 0; i < posts.Length; i++)
        {
            var post = await GetPosts(posts, i);

            if (post is not null)
            {
                if (!idPostsTable.Contains(post.idPost))
                {
                    idPostsTable.Add(post.idPost);
                    allSearchPosts.Add(post);
                }
            }
        }
        
        var filteredPostDtos = allSearchPosts
            .OrderBy(e => e.CreatedAt)
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();

        return new PostQtySearchDto()
        {
            Posts = filteredPostDtos,
            IsNext = qtyPosts < allSearchPosts.Count
        };
        
    }

    private async Task<PostSearchDto?> GetPosts(int[] table, int i)
    {
        var post = await _context.Posts
                .Where(x => x.IdPost == (int)table.GetValue(i))
                .Include(x => x.IdUserNavigation)
                .Select(a => new PostSearchDto
                {
                    idPost = a.IdPost,
                    Likes = _context
                        .Posts
                        .Where(c => c.IdPost == a.IdPost)
                        .SelectMany(b => b.IdUsers)
                        .Count(),
                    CommentsNumber = _context
                        .Posts
                        .Where(c => c.IdPost == a.IdPost)
                        .SelectMany(b => b.CommentPosts)
                        .Count(),
                    CreatedAt = a.CreatedAt,
                    Content = a.Content,
                    Nick = a.IdUserNavigation.Nick,
                    Type = a.IdUserNavigation.PhotoType,
                    base64PhotoData = Convert.ToBase64String(a.IdUserNavigation.Photo ?? Array.Empty<byte>()),
                    Tags = _context
                        .Posts
                        .Where(x => x.IdPost == a.IdPost)
                        .SelectMany(x => x.IdTags)
                        .Select(x => x.NameTag)
                        .ToArray()
                }).SingleOrDefaultAsync();

        return post;
    }
    
    public async Task<ArticleQtySearchDto?> GetArticleByWordFromDb(int qtyArticles, DateTime time,string word)
    {
        var idArticlesTable = new List<int>();
        var allSearchArticles = new List<ArticleSearchDto>();
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var articles = await _context.Articles
            .Where(x => x.Content.ToLower().Contains(word))
            .Select(x=>x.IdArticle)
            .ToArrayAsync();
        
        for (int i = 0; i < articles.Length; i++)
        {
            var article = await GetArticles(articles, i);

            if (!idArticlesTable.Contains(article.IdArticle))
            {
                idArticlesTable.Add(article.IdArticle);
                allSearchArticles.Add(article);
            }
        }

        articles = await _context.Tags
            .Where(x => x.NameTag.ToLower().Contains(word))
            .SelectMany(x => x.IdArticles)
            .Select(x => x.IdArticle).ToArrayAsync();
        
        for (int i = 0; i < articles.Length; i++)
        {
            var article = await GetArticles(articles, i);

            if (article is not null)
            {
                if (!idArticlesTable.Contains(article.IdArticle))
                {
                    idArticlesTable.Add(article.IdArticle);
                    allSearchArticles.Add(article);
                }
            }
        }
        
        
        var filteredArticlesDtos = allSearchArticles
            .OrderBy(e => e.CreatedAt)
            .Skip(qtyArticles - 5)
            .Take(5)
            .ToList();

        return new ArticleQtySearchDto()
        {
            Articles = filteredArticlesDtos,
            IsNext = qtyArticles < allSearchArticles.Count
        };
        
    }

    private async Task<ArticleSearchDto?> GetArticles(int[] table, int i)
    {
        var article = await _context
                .Articles
                .Where(x => x.IdArticle == (int)table.GetValue(i))
                .Include(x => x.IdUserNavigation)
                .Select(x => new ArticleSearchDto
                {
                    IdArticle  = x.IdArticle,
                    Author     = new ReducedUserResponseDto
                    {
                        IdUser = x.IdUser,
                        Nick   = x.IdUserNavigation.Nick
                    },
                    Title     = x.Title,
                    Content   = x.Content,
                    CreatedAt = x.CreatedAt,
                    Tags = _context
                        .Articles
                        .Where(t => t.IdArticle == x.IdArticle)
                        .SelectMany(t => x.IdTags)
                        .Select(t => t.NameTag)
                        .ToArray(),
                    LikesNum = _context
                        .Articles
                        .Where(l => l.IdArticle == x.IdArticle)
                        .SelectMany(l => l.IdUsers)
                        .Count(),
                    CommentsNum = _context
                       .CommentArticles
                       .Count(c => c.IdArticle == x.IdArticle),
                })
                .SingleOrDefaultAsync();

        return article;
    }
    
      public async Task<ArticleQtyDto> GetArticlesByTagFromDb(int qtyArticles, DateTime time,string nameTag)
    {
        var next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }

        var articles = await GetArticlesByTag(nameTag);
        
        var filteredArticleDtos = articles
            .OrderBy(x => x.IdArticle)
            .Skip(qtyArticles-5)
            .Take(5)
            .ToArray();

        var nextArticle = articles.Count();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }

        return new ArticleQtyDto()
        {
            Articles = filteredArticleDtos,
            IsNext = next
        };
    }

    private async Task<List<ArticleDto>?> GetArticlesByTag(string nameTag)
    {
        List<ArticleDto> articles = new List<ArticleDto>();
        
        var tags = await _context.Tags
            .Where(x => x.NameTag == nameTag)
            .SelectMany(x => x.IdArticles)
            .Select(x => x.IdArticle).ToArrayAsync();

        for (var i = 0; i < tags.Length; i++)
        {
            var article = await _context
                .Articles
                .Where(x=>x.IdArticle == (int)tags.GetValue(i))
                .Include(x => x.IdUserNavigation)
                .Select(x => new ArticleDto
                {
                    IdArticle = x.IdArticle,
                    Author = new ReducedUserResponseDto
                    {
                        IdUser = x.IdUser,
                        Nick = x.IdUserNavigation.Nick,
                        base64PhotoData = Convert.ToBase64String(x.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                        Type = x.IdUserNavigation.PhotoType,
                    },
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,

                    Tags = _context
                        .Articles
                        .Where(t => t.IdArticle == x.IdArticle)
                        .SelectMany(t => x.IdTags)
                        .Select(t => t.NameTag)
                        .ToArray(),

                    LikesNum = _context
                        .Articles
                        .Where(l => l.IdArticle == x.IdArticle)
                        .SelectMany(l => l.IdUsers)
                        .Count(),
                    CommentsNum = _context
                        .CommentArticles
                        .Count(c => c.IdArticle == x.IdArticle),
                    Comments = _context
                        .CommentArticles
                        .Where(c => c.IdArticle == x.IdArticle)
                        .Include(c => c.IdUserNavigation)
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(c => new ArticleCommentDto()
                        {
                            Content = c.DescriptionArticle,
                            IdComment = c.IdCommentArticle,
                            IdArticle = c.IdArticle,
                            IdUser = c.IdUser,
                            Nick = c.IdUserNavigation.Nick,
                            CreatedAt = c.CreatedAt,
                            base64PhotoData = Convert.ToBase64String(c.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                            Type = c.IdUserNavigation.PhotoType,
                        })
                        .Take(2)
                        .ToList(),
                })
                .SingleOrDefaultAsync();

            if (article is not null)
            {
                articles.Add(article);
            }
        }
        return articles;
    }
    
     public async Task<PostQtyDto?> GetPostsByTagFromDb(int qtyPosts, DateTime time,string nameTag)
    {
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allPosts = await GetPostsByTag(nameTag);

        var filteredPostDtos = allPosts
            .Where(x=>x.CreatedAt < time)
            .OrderBy(e => e.CreatedAt)
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();

        return new PostQtyDto()
        {
            Posts = filteredPostDtos,
            IsNext = qtyPosts < allPosts.Count
        };
        
    }

    private async Task<List<PostDto>?> GetPostsByTag(string nameTag)
    {
        List<PostDto> posts = new List<PostDto>();
        
        var tags = await _context.Tags
            .Where(x => x.NameTag == nameTag)
            .SelectMany(x => x.IdPosts)
            .Select(x => x.IdPost).ToArrayAsync();


        for (var i = 0; i < tags.Length; i++)
        {
            var post = await _context
                .Posts
                .Where(x=>x.IdPost == (int)tags.GetValue(i))
                .Select(a => new PostDto
                {
                    idPost = a.IdPost,
                    Likes  = _context
                        .Posts
                        .Where(c => c.IdPost == a.IdPost)
                        .SelectMany(b => b.IdUsers)
                        .Count(),
                    CommentsNumber = _context
                        .Posts
                        .Where(c => c.IdPost == a.IdPost)
                        .SelectMany(b => b.CommentPosts)
                        .Count(),
                    CommentsDtos  = _context.CommentPosts
                        .Where(x => x.IdPost == a.IdPost)
                        .Include(x => x.IdUserNavigation)
                        .OrderByDescending(c => c.CreatedAt)
                        .Select(x => new PostCommentDto
                        {
                            Content = x.DescriptionPost,
                            IdComment = x.IdComment,
                            IdPost = x.IdPost,
                            IdUser = x.IdUser,
                            Nick  =x.IdUserNavigation.Nick,
                            CreatedAt = x.CreatedAt,
                            base64PhotoData = Convert.ToBase64String(x.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                            Type = x.IdUserNavigation.PhotoType,
                        })
                        .Take(2)
                        .ToList(),
                    CreatedAt       = a.CreatedAt,
                    Content         = a.Content,
                    Nick            = a.IdUserNavigation.Nick,
                    Type            = a.IdUserNavigation.PhotoType,
                    base64PhotoData = Convert.ToBase64String(a.IdUserNavigation.Photo ?? Array.Empty<byte>()),
                    Tags            = _context
                        .Posts
                        .Where(x => x.IdPost == a.IdPost)
                        .SelectMany(x => x.IdTags)
                        .Select(x => x.NameTag)
                        .ToArray()
                }).SingleOrDefaultAsync();

            if (post is not null)
            {
                posts.Add(post);
            }
        }    
        return posts;
    }
}