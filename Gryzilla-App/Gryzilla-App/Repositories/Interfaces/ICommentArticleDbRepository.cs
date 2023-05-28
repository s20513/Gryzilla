using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.DTOs.Responses.ArticleComment;

namespace Gryzilla_App.Repositories.Interfaces;

public interface ICommentArticleDbRepository
{
    public Task<ArticleCommentDto?> AddCommentToArticle(NewArticleCommentDto newArticleCommentDto);

    public Task<ArticleCommentDto?> ModifyArticleCommentFromDb(PutArticleCommentDto putArticleCommentDto, int idComment, ClaimsPrincipal userClaims);

    public Task<ArticleCommentDto?> DeleteArticleCommentFromDb(int idComment, ClaimsPrincipal userClaims);
    public Task<GetArticleCommentDto> GetArticleCommentsFromDb(int idArticle);
}