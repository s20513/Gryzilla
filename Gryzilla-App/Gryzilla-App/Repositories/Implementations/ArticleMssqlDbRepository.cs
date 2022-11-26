using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.ArticleComment;
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
    private void CreateMissingTagsAndBindWithArticle(IEnumerable<TagDto> newArticleTags, Article article, List<Tag> dbTags)
    {
        List<Tag>?    articleDbTags;
        List<string>? articleTagList;
        List<string>? allTagsFromDb;
        List<string>? tagsToCreate;
        
        articleTagList = new List<string>();
        articleTagList.AddRange(newArticleTags.Select(tag => tag.NameTag));
        
        allTagsFromDb = dbTags.Select(x => x.NameTag).ToList();
        tagsToCreate = articleTagList.Where(x => !allTagsFromDb.Contains(x)).ToList();

        if (tagsToCreate.Count != 0)
        {
            dbTags.AddRange(tagsToCreate.Select(tagName => new Tag { NameTag = tagName }));
        }
            
        articleDbTags = dbTags.Where(e => articleTagList.Contains(e.NameTag)).ToList();

        foreach (var tag in articleDbTags)
        {
            article.IdTags.Add(tag);
        }
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
                        .Select(c => new ArticleCommentDto
                        {
                            IdArticle   = c.IdArticle,
                            Nick        = c.IdUserNavigation.Nick,
                            IdComment   = c.IdCommentArticle,
                            IdUser      = c.IdUserNavigation.IdUser,
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
        int        articleId;
        Article?   article;
        List<Tag>? tags;
        UserDatum? user;

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

        if (articleDto.Tags.Count != 0)
        {
            tags = await _context.Tags.ToListAsync();
            CreateMissingTagsAndBindWithArticle(articleDto.Tags, article, tags);
        }
        
        await _context.SaveChangesAsync();

        articleId = _context.Articles.Max(e => e.IdArticle);

        return new ArticleDto
        {
            IdArticle  = articleId,
            Author     = new ReducedUserResponseDto
            {
                IdUser = article.IdUser,
                Nick   = user.Nick
            },
            Title      = article.Title,
            Content    = article.Content,
            CreatedAt  = article.CreatedAt,
            Tags       = articleDto
                .Tags
                .Select(e => new TagDto
                {
                    NameTag = e.NameTag
                }
                ).ToArray()
        };
    }
    public async Task<ArticleDto?> DeleteArticleFromDb(int idArticle)
    {
        string?               userNick;
        List<Tag>?            tags;
        Article?              article;
        List<UserDatum>?      likes;
        List<CommentArticle>? comments;
        
        article = await _context
            .Articles
            .Where(e => e.IdArticle == idArticle)
            .Include(e => e.IdTags)
            .Include(e => e.IdUsers)
            .SingleOrDefaultAsync();

        if (article is null)
        {
             return null;
        }

        
        likes = await _context
            .Articles
            .Where(e => e.IdArticle == idArticle)
            .SelectMany(e => e.IdUsers)
            .ToListAsync();

        foreach (var like in likes)
        {
            article.IdUsers.Remove(like);
        }

        
        tags = await _context
            .Articles
            .Where(e => e.IdArticle == idArticle)
            .SelectMany(e => e.IdTags)
            .ToListAsync();

        foreach (var tag in tags)
        {
            article.IdTags.Remove(tag);
        }
        
        
        comments = await _context
                .CommentArticles
                .Where(e => e.IdArticle == idArticle)
                .ToListAsync();

        foreach (var comment in comments)
        {
            _context.CommentArticles.Remove(comment);
        }
        
        
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        
        userNick = await _context
            .UserData
            .Where(x => x.IdUser == article.IdUser)
            .Select(x => x.Nick)
            .SingleOrDefaultAsync();
        
        return new ArticleDto
        {
            IdArticle  = article.IdArticle,
            Author     = new ReducedUserResponseDto
            {
                IdUser = article.IdUser,
                Nick   = userNick
            },
            Title      = article.Title,
            Content    = article.Content,
            CreatedAt  = article.CreatedAt
        };
    }

    public async Task<ArticleDto?> ModifyArticleFromDb(PutArticleRequestDto putArticleRequestDto, int idArticle)
    {
        Article?          article;
        List<Tag>?        dbTags;
        ICollection<Tag>? articleDbTags;

        article = await _context
            .Articles
            .Where(e => e.IdArticle == idArticle)
            .Include(e => e.IdTags)
            .Include(e => e.IdUserNavigation)
            .SingleOrDefaultAsync();
        
        if (article is null)
        {
            return null;
        }
        
        article.Title   = putArticleRequestDto.Title;
        article.Content = putArticleRequestDto.Content;

        articleDbTags = article.IdTags;
        
        foreach (var tag in articleDbTags)
        {
            article.IdTags.Remove(tag);
        }

        if (putArticleRequestDto.Tags is not null)
        {
            dbTags = await _context.Tags.ToListAsync();
            CreateMissingTagsAndBindWithArticle(putArticleRequestDto.Tags, article, dbTags);
        }

        await _context.SaveChangesAsync();

        return new ArticleDto
        {
            IdArticle   = article.IdArticle,
            Author      = new ReducedUserResponseDto
            {
                IdUser  = article.IdUser,
                Nick    = article.IdUserNavigation.Nick
            },
            Title       = article.Title,
            Content     = article.Content,
            CreatedAt   = article.CreatedAt,
            Tags        = articleDbTags.Select(e => new TagDto
            {
                NameTag = e.NameTag
            }).ToArray(),
            
            LikesNum = _context
                .Articles
                .Where(e => e.IdArticle == article.IdArticle)
                .SelectMany(e => e.IdUsers)
                .Count(),
            
            Comments = _context
                .CommentArticles
                .Where(e => e.IdArticle == article.IdArticle)
                .Include(e => e.IdUserNavigation)
                .Select(e => new ArticleCommentDto
                {
                    IdArticle   = e.IdArticle,
                    Nick        = e.IdUserNavigation.Nick,
                    IdComment   = e.IdCommentArticle,
                    IdUser      = e.IdUserNavigation.IdUser,
                    Description = e.DescriptionArticle
                }).ToArray()
        };
    }

    private async Task<IEnumerable<ArticleDto>?> GetAllArticlesFromDb()
    {
        var articles =  await _context
            .Articles
            .Include(x => x.IdUserNavigation)
            .Select(x => new ArticleDto
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
                    .Select(c => new ArticleCommentDto
                    {
                        IdArticle   = c.IdArticle,
                        Nick        = c.IdUserNavigation.Nick,
                        IdComment   = c.IdCommentArticle,
                        IdUser      = c.IdUserNavigation.IdUser,
                        Description = c.DescriptionArticle
                    }).ToArray()
            }).ToListAsync();

        return articles.Any() ? articles : null;
    }
    
 
}