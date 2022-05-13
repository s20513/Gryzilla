using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Responses.Articles;

namespace Gryzilla_App.Repositories.Interfaces;

public interface IArticleDbRepository
{
    public Task<ArticleDto?> GetPostFromDb(int idPost);
    public Task<IEnumerable<ArticleDto>?> GetArticlesFromDb();
    public Task<IEnumerable<ArticleDto>> GetPostsByLikesFromDb();
    public Task<IEnumerable<ArticleDto>?> GetPostsByLeastLikesFromDb();
    public Task<IEnumerable<ArticleDto>?> GetPostsByDataFromDb();
    public Task<IEnumerable<ArticleDto>?> GetPostsByOldestDateFromDb();
    public Task<ArticleDto?> AddNewPostToDb(NewArticleRequestDto articleDto);
    public Task<ArticleDto?> DeletePostFromDb(int idPost);
    public Task<ArticleDto?> ModifyPostFromDb(PutArticleRequestDto putArticleRequestDto, int idPost);
}