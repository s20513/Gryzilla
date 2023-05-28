using System.Security.Claims;
using Gryzilla_App.DTOs.Responses.LikesArticle;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesArticleDbRepository
{
    public Task<LikesArticleDto?> ExistLikeArticle(int idUser, int idArticle);
    public Task<string?> DeleteLikeFromArticle(int idUser, int idArticle, ClaimsPrincipal userClaims);
    public Task<string?> AddLikeToArticle(int idUser, int idArticle);
}