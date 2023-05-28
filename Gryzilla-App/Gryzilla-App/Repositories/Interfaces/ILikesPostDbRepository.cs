using System.Security.Claims;
using Gryzilla_App.DTOs.Responses.LikesPost;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesPostDbRepository
{
    public Task<string?> AddLikeToPost(int idUser, int idPost);
    public Task<string?> DeleteLikeFromPost(int idUser, int idPost, ClaimsPrincipal userClaims);
    public Task<LikesPostDto?> ExistLike(int idUser, int idPost);
}