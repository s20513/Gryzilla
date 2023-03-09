using Gryzilla_App.DTOs.Responses.LikesArticle;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesArticleDbRepository
{
    public Task<LikesArticleDto?> ExistLikeArticle(int idUser, int idArticle);
    public Task<object?> DeleteLikeFromArticle(int idUser, int idArticle);
    public Task<object?> AddLikeToArticle(int idUser, int idArticle);
}