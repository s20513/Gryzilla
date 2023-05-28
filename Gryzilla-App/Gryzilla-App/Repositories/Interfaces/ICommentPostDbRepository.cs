using System.Security.Claims;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses.PostComment;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ICommentPostDbRepository
{
    public Task<PostCommentDto?> AddCommentToPost(NewPostCommentDto newPostCommentDto);
    public Task<PostCommentDto?> ModifyPostCommentFromDb(PutPostCommentDto modifyPostCommentDto, int idComment, ClaimsPrincipal userClaims);
    public Task<PostCommentDto?> DeleteCommentFromDb(int idComment, ClaimsPrincipal userClaims);
    public Task<GetPostCommentDto?> GetPostCommentsFromDb(int idPost);
}