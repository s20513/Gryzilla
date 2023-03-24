using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Helpers;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ArticleDbRepository: IArticleDbRepository
{
    private readonly GryzillaContext _context;

    public ArticleDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    private void CreateMissingTagsAndBindWithArticle(String [] newArticleTags, Article article, List<Tag> dbTags)
    {
        List<Tag>?    articleDbTags;
        List<string>? articleTagList;
        List<string>? allTagsFromDb;
        List<string>? tagsToCreate;
        
        articleTagList = new List<string>();
        articleTagList.AddRange(newArticleTags.Select(tag => tag));
        
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
    private async Task<IEnumerable<ArticleDto>?> GetTableSort(Article[] allArticles)
    {
        var articleDto = new List<ArticleDto>();

        foreach (var article in allArticles)
        {
            var newArticle =
                await _context
                    .Articles
                    .Where(x => x.IdArticle == article.IdArticle)
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
                            .Where(t => t.IdArticle ==article.IdArticle)
                            .SelectMany(t => x.IdTags)
                            .Select(x => x.NameTag)
                            .ToArray(),

                        LikesNum = _context
                            .Articles
                            .Where(l => l.IdArticle == x.IdArticle)
                            .SelectMany(l => l.IdUsers)
                            .Count(),
                        CommentsNum = _context
                            .CommentArticles
                            .Count(c => c.IdArticle == x.IdArticle),
                        Comments = GetCommentArticle(article.IdArticle),
                    })
                    .SingleOrDefaultAsync();
            
            if (newArticle != null)
            {
                articleDto.Add(newArticle);
            }
        }

        return articleDto;
    }
    
    private List<ArticleCommentDto> GetCommentArticle(int idArticle)
    {
        var articleComment =  _context
            .CommentArticles
            .Where(x => x.IdArticle == idArticle)
            .Include(c => c.IdUserNavigation)
            .Select(x => new ArticleCommentDto()
            {
                Content = x.DescriptionArticle,
                IdComment = x.IdCommentArticle,
                IdArticle = x.IdArticle,
                IdUser = x.IdUser,
                Nick   = x.IdUserNavigation.Nick,
                CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(x.CreatedAt)
            })
            .ToList();
        List <ArticleCommentDto> list = articleComment.Take(2).ToList();
        return list;
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
                    Comments = GetCommentArticle(idArticle),
                })
                .SingleOrDefaultAsync();

        return article ?? null;
    }

    public async Task<ArticleQtyDto?> GetQtyArticlesFromDb(int qtyArticles)
    {
        bool next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var allArticles = await _context
            .Articles
            .OrderBy(x => x.IdArticle)
            .Skip(qtyArticles-5)
            .Take(5)
            .ToArrayAsync();
        
        if (allArticles.Length == 0)
        {
            return null;
        }

        var nextArticle = await _context
            .Articles
            .CountAsync();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }
        
        var articleDto = await GetTableSort(allArticles);
        
        return new ArticleQtyDto()
        {
            Articles = articleDto,
            IsNext = next
        };
    }

    public async Task<ArticleQtyDto?> GetQtyArticlesByMostLikesFromDb(int qtyArticles, DateTime time)
    {
        bool next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var allArticles = await _context
            .Articles
            .ToArrayAsync();
        
        if (allArticles.Length == 0)
        {
            return null;
        }
        
        var articlesDto = await GetTableSort(allArticles);

        articlesDto = articlesDto
            .Where(x =>x.CreatedAt < time)
            .OrderByDescending(x => x.LikesNum)
            .Skip(qtyArticles - 5)
            .Take(5)
            .ToArray();
        
        var nextArticle = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }
        
        return new ArticleQtyDto()
        {
            Articles = articlesDto,
            IsNext = next
        };
    }


    public async Task<ArticleQtyDto?> GetQtyArticlesByEarliestDateFromDb(int qtyArticles, DateTime time)
    {
        bool next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allArticles = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .OrderByDescending(e => e.CreatedAt)
            .ToArrayAsync();
        
        if (allArticles.Length == 0)
        {
            return null;
        }
        
        var articlesDto = await GetTableSort(allArticles);

        
        articlesDto = articlesDto
            .Skip(qtyArticles - 5)
            .Take(5)
            .ToArray();
        
        var nextArticle = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }
        
        return new ArticleQtyDto()
        {
            Articles = articlesDto,
            IsNext = next
        };
    }

    public async Task<ArticleQtyDto?> GetQtyArticlesByOldestDateFromDb(int qtyArticles, DateTime time)
    {
        bool next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var allArticles = await _context
            .Articles
            .Where(e=>e.CreatedAt < time)
            .OrderBy(e => e.CreatedAt)
            .ToArrayAsync();
        
        if (allArticles.Length == 0)
        {
            return null;
        }
        
        var articlesDto = await GetTableSort(allArticles);

        
        articlesDto = articlesDto
            .Skip(qtyArticles - 5)
            .Take(5)
            .ToArray();
        
        var nextArticle = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }
        
        return new ArticleQtyDto()
        {
            Articles = articlesDto,
            IsNext = next
        };
    }
    public async Task<IEnumerable<ArticleDto>?> GetTopArticles()
    {
        var allArticles = await _context
            .Articles
            .ToArrayAsync();

        if (allArticles.Length == 0)
        {
            return null;
        }

        var articlesDtos = await GetTableSort(allArticles);

        articlesDtos = articlesDtos
            .OrderByDescending(order => order.LikesNum)
            .Skip(0)
            .Take(3)
            .ToList();

        return articlesDtos;
    }
    public async Task<ArticleQtyDto?> GetQtyArticlesByCommentsFromDb(int qtyArticles, DateTime time)
    {
        bool next = false;
        
        if (qtyArticles < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allArticles = await _context
            .Articles
            .ToArrayAsync();
        
        if (allArticles.Length == 0)
        {
            return null;
        }
        
        var articlesDto = await GetTableSort(allArticles);

        articlesDto = articlesDto
            .Where(x=>x.CreatedAt < time)
            .OrderByDescending(x => x.CommentsNum)
            .Skip(qtyArticles - 5)
            .Take(5)
            .ToArray();
        
        var nextArticle = await _context
            .Articles
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyArticles < nextArticle)
        {
            next = true;
        }
        
        return new ArticleQtyDto()
        {
            Articles = articlesDto,
            IsNext = next
        };
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
        return articles?.OrderByDescending(order => Convert.ToDateTime(order.CreatedAt));
    }

    public async Task<IEnumerable<ArticleDto>?> GetArticlesByOldestDateFromDb()
    {
        var articles = await GetAllArticlesFromDb();
        return articles?.OrderBy(order => Convert.ToDateTime(order.CreatedAt));
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

        if (articleDto.Tags.Length != 0)
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
                .ToArray(),
            CommentsNum = 0,
            LikesNum = 0,
            Comments = new List<ArticleCommentDto>()
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
            CreatedAt  = article.CreatedAt,
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
            Tags        = articleDbTags.
                Select(x => x.NameTag)
                .ToArray(),
            
            LikesNum = _context
                .Articles
                .Where(e => e.IdArticle == article.IdArticle)
                .SelectMany(e => e.IdUsers)
                .Count(),
            CommentsNum = _context
                .Articles
                .Where(e => e.IdArticle == article.IdArticle)
                .SelectMany(e => e.CommentArticles)
                .Count(),
            Comments = GetCommentArticle(article.IdArticle),
        };
    }

    public async Task<IEnumerable<ArticleDto>?> GetUserArticlesFromDb(int idUser)
    {
        var userArticles = await _context
            .Articles
            .Where(e => e.IdUser == idUser)
            .ToArrayAsync();

        if (userArticles.Length == 0)
        {
            return null;
        }

        var articlesDtos = await GetTableSort(userArticles);

        return articlesDtos;
    }

    private async Task<IEnumerable<ArticleDto>?> GetAllArticlesFromDb()
    {
        var allArticles = await _context.Articles.ToArrayAsync();
        
        var articleDto = new List<ArticleDto>();
        
        foreach (var article in allArticles)
        {
            var newArticle = await _context
                .Articles
                .Include(x => x.IdUserNavigation)
                .Where(x=>x.IdArticle == article.IdArticle)
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
                        .Select(x => x.NameTag)
                        .ToArray(),

                    LikesNum = _context
                        .Articles
                        .Where(l => l.IdArticle == x.IdArticle)
                        .SelectMany(l => l.IdUsers)
                        .Count(),
                    CommentsNum = _context
                        .CommentArticles
                        .Count(c => c.IdArticle == x.IdArticle),
                    Comments = GetCommentArticle(article.IdArticle),
                }).SingleOrDefaultAsync();
            
            if (newArticle != null)
            {
                articleDto.Add(newArticle);
            }
        }

        return articleDto.Any() ? articleDto : null;
    }



}