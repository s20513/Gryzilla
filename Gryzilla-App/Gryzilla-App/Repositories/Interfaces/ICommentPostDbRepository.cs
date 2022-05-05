using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ICommentPostDbRepository
{
    public Task<CommentDto?> PostNewCommentToDb(AddCommentDto addCommentDto);
    public Task<ModifyCommentDto?> ModifyCommentToDb(PutCommentDto modifyCommentDto, int idComment);
    public Task<DeleteCommentDto?> DeleteCommentFromDb(int idComment);
}