using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/articles/comments")]
public class CommentArticleController: Controller
{
    private readonly ICommentArticleDbRepository _commentArticleDbRepository;
    
    public CommentArticleController(ICommentArticleDbRepository commentArticleDbRepository)
    {
        _commentArticleDbRepository = commentArticleDbRepository;
    }

    /// <summary>
    /// Create Article Comment
    /// </summary>
    /// <param name="newArticleCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Article Comment created correctly, returns  Article Comment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> CreateNewArticleComment([FromBody] NewArticleCommentDto newArticleCommentDto)
    {
        var result = await _commentArticleDbRepository.AddCommentToArticle(newArticleCommentDto);
        
        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "User or article not found" });
        }
            
        return Ok(result);
    }

    /// <summary>
    /// Modify Article Comment
    /// </summary>
    /// <param name="idComment">Article Comment id</param>
    /// <param name="putArticleCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Article Comment modified correctly, returns  Article Comment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    /// <returns>Return Status Bad Request - Id from route and Id in body are not some</returns>
    [HttpPut("{idComment:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyArticleComment(PutArticleCommentDto putArticleCommentDto, int idComment)
    {
        if (putArticleCommentDto.IdComment != idComment)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }
        
        var result = await _commentArticleDbRepository
            .ModifyArticleCommentFromDb(putArticleCommentDto, idComment);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Comment not found" });
        }
            
        return Ok(result);
    }

    /// <summary>
    /// Delete Article Comment
    /// </summary>
    /// <param name="idComment">Article Comment Id</param>
    /// <returns> Return Status Ok - Article Comment deleted correctly, returns Article Comment body</returns>
    /// <returns>Return status Not Found - Article Comment not found</returns>
    [HttpDelete("{idComment:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteArticleComment([FromRoute] int idComment)
    {
        var result = await _commentArticleDbRepository.DeleteArticleCommentFromDb(idComment);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Comment not found" });
        }
            
        return Ok(result);
    }
    
    /// <summary>
    /// Get Article Comments
    /// </summary>
    /// <param name="idArticle">Article Id</param>
    /// <returns> Return Status Ok and return article comments</returns>
    /// <returns>Return status Not Found - Article Comment not found</returns>
    [HttpGet("{idArticle:int}")]
    public async Task<IActionResult> GetArticleComment([FromRoute] int idArticle)
    {
        var result = await _commentArticleDbRepository.GetArticleCommentsFromDb(idArticle);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Comments not found" });
        }
            
        return Ok(result);
    }
}