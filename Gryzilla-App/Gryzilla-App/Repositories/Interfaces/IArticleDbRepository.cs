using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Requests.Post;
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
    public Task<IEnumerable<ArticleDto>?> GetTopArticles();
    public Task<ArticleQtyDto?> GetQtyArticlesFromDb(int qtyArticles);
    public Task<ArticleQtyDto?> GetQtyArticlesByMostLikesFromDb(int qtyArticles, DateTime time);
    public Task<ArticleQtyDto?> GetQtyArticlesByCommentsFromDb(int qtyArticles, DateTime time);
    public Task<ArticleQtyDto?> GetQtyArticlesByEarliestDateFromDb(int qtyArticles, DateTime time);
    public Task<ArticleQtyDto?> GetQtyArticlesByOldestDateFromDb(int qtyArticles, DateTime time);
    public Task<ArticleDto?> AddNewArticleToDb(NewArticleRequestDto articleDto);
    public Task<ArticleDto?> DeleteArticleFromDb(int idArticle);
    public Task<ArticleDto?> ModifyArticleFromDb(PutArticleRequestDto putArticleRequestDto, int idArticle);
    public Task<IEnumerable<ArticleDto>> GetUserArticlesFromDb(int idUser);
}