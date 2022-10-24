using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ArticleMssqlDbRepository: IArticleDbRepository
{
    private readonly GryzillaContext _context;

    public ArticleMssqlDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<ArticleDto?> GetArticleFromDb(int idArticle)
    {
        var article = 
            await _context
                .Articles
                .Where(x => x.IdArticle == idArticle)
                .Include(x => x.IdUserNavigation)
                .Select(x => new ArticleDto
                {
                    IdArticle = x.IdArticle,
                    Author = new ReducedUserResponseDto
                    {
                        IdUser = x.IdUser,
                        Nick = x.IdUserNavigation.Nick
                    },
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    
                    Tags = _context
                        .Articles
                        .Where(t => t.IdArticle == idArticle)
                        .SelectMany(t => x.IdTags)
                        .Select(t => new TagDto
                        {
                            NameTag = t.NameTag
                        })
                        .ToArray(),
                    
                    LikesNum = _context
                        .Articles
                        .Where(l => l.IdArticle == x.IdArticle)
                        .SelectMany(l => l.IdUsers)
                        .Count(),
                    
                    Comments = _context
                        .CommentArticles
                        .Where(c => c.IdArticle == x.IdArticle)
                        .Include(c => c.IdUserNavigation)
                        .Select(c => new CommentDto
                        {
                            idComment = c.IdCommentArticle,
                            Nick = c.IdUserNavigation.Nick,
                            Description = c.DescriptionArticle
                        }).ToArray()
                })
                .SingleOrDefaultAsync();

        return article ?? null;
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles ?? null;
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesByMostLikesFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles?.OrderByDescending(order => order.LikesNum);
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesByLeastLikesFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles?.OrderBy(order => order.LikesNum);
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesByEarliestDateFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles?.OrderByDescending(order => order.CreatedAt);
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesByOldestDateFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles?.OrderBy(order => order.CreatedAt);
    }

    public async Task<ArticleDto?> AddNewArticleToDb(NewArticleRequestDto articleDto)
    {
        UserDatum?   user;
        Article?     article;
        List<string> tagList;
        List<Tag>?   tags;
        int          articleId;
        
        user = await _context.UserData.SingleOrDefaultAsync(e => e.IdUser == articleDto.IdUser);
        
        if (user is null)
        {
            return null;
        }

        article = new Article
        {
            IdUser = articleDto.IdUser,
            Title = articleDto.Title,
            Content = articleDto.Content,
            CreatedAt = DateTime.Now,
        };
        
        await _context.Articles.AddAsync(article);
        
        tagList = new List<string>();
        
        if (articleDto.Tags is not null)
        {
            tagList.AddRange(articleDto.Tags.Select(tag => tag.NameTag));
        }

        tags = await _context.Tags.Where(e => tagList.Contains(e.NameTag)).ToListAsync();

        foreach (var tag in tags)
        {
            article.IdTags.Add(tag);
        }

        await _context.SaveChangesAsync();

        articleId = _context.Articles.Max(e => e.IdArticle);

        return new ArticleDto
        {
            IdArticle = articleId,
            Author = new ReducedUserResponseDto
            {
                IdUser = article.IdUser,
                Nick = user.Nick
            },
            Title = article.Title,
            Content = article.Content,
            CreatedAt = article.CreatedAt,
            Tags = tagList.Select(e => new TagDto
            {
                NameTag = e
            }).ToArray()
        };
    }

    public async Task<ArticleDto?> DeleteArticleFromDb(int idArticle)
    {
        var article = await _context.Articles
            .Where(e => e.IdArticle == idArticle)
            .Include(e => e.IdTags)
            .Include(e => e.IdUsers)
            .SingleOrDefaultAsync();

        if (article is null)
        {
             return null;
        }

        var likes = await _context.Articles
            .Where(e => e.IdArticle == idArticle)
            .SelectMany(e => e.IdUsers)
            .ToListAsync();

        foreach (var like in likes)
        {
            article.IdUsers.Remove(like);
        }

        var tags = await _context.Articles
            .Where(e => e.IdArticle == idArticle)
            .SelectMany(e => e.IdTags)
            .ToListAsync();

        foreach (var tag in tags)
        {
            article.IdTags.Remove(tag);
        }
        
        var comments = 
            await _context
                .CommentArticles
                .Where(e => e.IdArticle == idArticle)
                .ToListAsync();

        foreach (var comment in comments)
        {
            _context.CommentArticles.Remove(comment);
        }
        
        await _context.SaveChangesAsync();
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        
        return new ArticleDto
        {
            IdArticle = article.IdArticle,
            Author = new ReducedUserResponseDto
            {
                IdUser = article.IdUser
            },
            Title = article.Title,
            Content = article.Content,
            CreatedAt = article.CreatedAt
        };
    }

    public async Task<ArticleDto?> ModifyArticleFromDb(PutArticleRequestDto putArticleRequestDto, int idArticle)
    {
        if (putArticleRequestDto.IdArticle != idArticle)
            return null;
        var article = await _context.Articles
            .Where(e => e.IdArticle == idArticle)
            .Include(e => e.IdTags)
            .Include(e => e.IdUserNavigation)
            .SingleOrDefaultAsync();
        if (article is null)
            return null;
        article.Title = putArticleRequestDto.Title;
        article.Content = putArticleRequestDto.Content;

        var tags = article.IdTags;
        foreach (var tag in tags)
            article.IdTags.Remove(tag);
        
        var tagList = new List<int>();
        if (putArticleRequestDto.Tags is not null) 
            tagList.AddRange(putArticleRequestDto.Tags.Select(tag => tag.idTag));
        
        tags = await _context.Tags.Where(e => tagList.Contains(e.IdTag)).ToListAsync();
        foreach (var tag in tags)
            article.IdTags.Add(tag);

        await _context.SaveChangesAsync();

        return new ArticleDto
        {
            IdArticle = article.IdArticle,
            Author = new ReducedUserResponseDto
            {
                IdUser = article.IdUser,
                Nick = article.IdUserNavigation.Nick
            },
            Title = article.Title,
            Content = article.Content,
            CreatedAt = article.CreatedAt,
            Tags = tags.Select(e => new TagDto
            {
                NameTag = e.NameTag
            }).ToArray(),
            LikesNum =
                _context.Articles.Where(e => e.IdArticle == article.IdArticle).SelectMany(e => e.IdUsers).Count(),
            Comments = _context.CommentArticles.Where(e => e.IdArticle == article.IdArticle)
                .Include(e => e.IdUserNavigation).Select(e => new CommentDto
                {
                    idComment = e.IdCommentArticle,
                    Nick = e.IdUserNavigation.Nick,
                    Description = e.DescriptionArticle
                }).ToArray()
        };
    }

    private async Task<IEnumerable<ArticleDto>?> GetAllArticlesFromDb()
    {
        return await _context
            .Articles
            .Include(x => x.IdUserNavigation)
            .Select(x => new ArticleDto
            {
                IdArticle = x.IdArticle,
                Author = new ReducedUserResponseDto
                {
                    IdUser = x.IdUser,
                    Nick = x.IdUserNavigation.Nick
                },
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                Tags = _context
                    .Articles
                    .Where(t => t.IdArticle == x.IdArticle)
                    .SelectMany(t => x.IdTags)
                    .Select(t => new TagDto
                    {
                        NameTag = t.NameTag
                    })
                    .ToArray(),
                LikesNum = _context
                    .Articles
                    .Where(l => l.IdArticle == x.IdArticle)
                    .SelectMany(l => l.IdUsers)
                    .Count(),
                Comments = _context
                    .CommentArticles
                    .Where(c => c.IdArticle == x.IdArticle)
                    .Include(c => c.IdUserNavigation)
                    .Select(c => new CommentDto
                    {
                        idComment = c.IdCommentArticle,
                        Nick = c.IdUserNavigation.Nick,
                        Description = c.DescriptionArticle
                    }).ToArray()
            }).ToListAsync();
    }
}