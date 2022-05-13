using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.Articles;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IArticleDbRepository
{
    public Task<ArticleDto?> GetArticleFromDb(int idArticle);
    public Task<IEnumerable<ArticleDto>?> GetArticlesFromDb();
    public Task<IEnumerable<ArticleDto>?> GetArticlesByMostLikesFromDb();
    public Task<IEnumerable<ArticleDto>?> GetArticlesByLeastLikesFromDb();
    public Task<IEnumerable<ArticleDto>?> GetArticlesByEarliestDateFromDb();
    public Task<IEnumerable<ArticleDto>?> GetArticlesByOldestDateFromDb();
    public Task<ArticleDto?> AddNewArticleToDb(NewArticleRequestDto articleDto);
    public Task<ArticleDto?> DeleteArticleFromDb(int idArticle);
    public Task<ArticleDto?> ModifyArticleFromDb(PutArticleRequestDto putArticleRequestDto, int idArticle);
    
}