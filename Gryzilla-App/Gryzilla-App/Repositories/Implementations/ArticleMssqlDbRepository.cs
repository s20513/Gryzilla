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
        var article = await _context.Articles.Where(e => e.IdArticle == idArticle)
            .Include(e => e.IdUserNavigation)
            .Select(e => new ArticleDto
            {
                IdArticle = e.IdArticle,
                Author = new ReducedUserResponseDto
                {
                    IdUser = e.IdUser,
                    Nick = e.IdUserNavigation.Nick
                },
                Title = e.Title,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
                Tags = _context.Articles.Where(t => t.IdArticle == idArticle).SelectMany(t => e.IdTags)
                    .Select(t => new TagDto { nameTag = t.NameTag }).ToArray(),
                LikesNum = _context.Articles.Where(l => l.IdArticle == e.IdArticle).SelectMany(l => l.IdUsers).Count(),
                Comments = _context.CommentArticles.Where(c => c.IdArticle == e.IdArticle)
                    .Include(c => c.IdUserNavigation).Select(c => new CommentDto
                    {
                        idComment = c.IdCommentArticle,
                        Nick = c.IdUserNavigation.Nick,
                        DescriptionPost = c.DescriptionArticle
                    }).ToArray()
            }).SingleOrDefaultAsync();

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

    public Task<ArticleDto?> AddNewArticleToDb(NewArticleRequestDto articleDto)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto?> DeleteArticleFromDb(int idArticle)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto?> ModifyArticleFromDb(PutArticleRequestDto putArticleRequestDto, int idArticle)
    {
        throw new NotImplementedException();
    }

    private async Task<IEnumerable<ArticleDto>?> GetAllArticlesFromDb()
    {
        return await _context.Articles
            .Include(e => e.IdUserNavigation)
            .Select(e => new ArticleDto
            {
                IdArticle = e.IdArticle,
                Author = new ReducedUserResponseDto
                {
                    IdUser = e.IdUser,
                    Nick = e.IdUserNavigation.Nick
                },
                Title = e.Title,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
                Tags = _context.Articles.Where(t => t.IdArticle == e.IdArticle).SelectMany(t => e.IdTags)
                    .Select(t => new TagDto { nameTag = t.NameTag }).ToArray(),
                LikesNum = _context.Articles.Where(l => l.IdArticle == e.IdArticle).SelectMany(l => l.IdUsers).Count(),
                Comments = _context.CommentArticles.Where(c => c.IdArticle == e.IdArticle)
                    .Include(c => c.IdUserNavigation).Select(c => new CommentDto
                    {
                        idComment = c.IdCommentArticle,
                        Nick = c.IdUserNavigation.Nick,
                        DescriptionPost = c.DescriptionArticle
                    }).ToArray()
            }).ToListAsync();
    }
}