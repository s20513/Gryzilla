namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesArticleDbRepository
{
    public Task<bool?> ExistLikeArticle(int idUser, int idArticle);
    public Task<object?> DeleteLikeFromArticle(int idUser, int idArticle);
    public Task<object?> AddLikeToArticle(int idUser, int idArticle);
}