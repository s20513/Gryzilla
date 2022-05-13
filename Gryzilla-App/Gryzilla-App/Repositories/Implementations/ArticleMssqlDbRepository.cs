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
    
    public Task<ArticleDto?> GetPostFromDb(int idPost)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetArticlesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>> GetPostsByLikesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetPostsByLeastLikesFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetPostsByDataFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleDto>?> GetPostsByOldestDateFromDb()
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto?> AddNewPostToDb(NewArticleRequestDto articleDto)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto?> DeletePostFromDb(int idPost)
    {
        throw new NotImplementedException();
    }

    public Task<ArticleDto?> ModifyPostFromDb(PutArticleRequestDto putArticleRequestDto, int idPost)
    {
        throw new NotImplementedException();
    }
}