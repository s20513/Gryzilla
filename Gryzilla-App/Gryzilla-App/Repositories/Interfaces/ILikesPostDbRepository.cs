namespace Gryzilla_App.Repositories.Interfaces;

public interface ILikesPostDbRepository
{
    public Task<string?> AddLikeToPost(int idUser, int idPost);
    public Task<string?> DeleteLikeFromPost(int idUser, int idPost);
    public Task<bool?> ExistLike(int idUser, int idPost);
}