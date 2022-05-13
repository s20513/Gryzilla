using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;

namespace Gryzilla_App.Repositories.Implementations;

public class ArticleMssqlDbRepository: IArticleDbRepository
{
    private readonly GryzillaContext _context;

    public ArticleMssqlDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public Task<ArticleDto?> GetArticleFromDb(int idArticle)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetArticlesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>> GetArticlesByLikesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetArticlesByLeastLikesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetArticlesByDataFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetArticlessByOldestDateFromDb()
    {
        throw new NotImplementedException();
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
}