using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ICommentPostDbRepository
{
    public Task<string?> PostNewCommentToDb(AddCommentDto addCommentDto);
    public Task<string?> ModifyCommentToDb(PutCommentDto modifyCommentDto, int idComment);
    public Task<string?> DeleteCommentFromDb(int idComment);
}