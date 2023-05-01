using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Responses.Articles;
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

        if (!users.Any())
        {
            return null;
        }
        
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
        
        if (!allSearchPosts.Any())
        {
            return null;
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
        
        if (!allSearchArticles.Any())
        {
            return null;
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
}