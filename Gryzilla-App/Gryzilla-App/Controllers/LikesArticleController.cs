using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/likesArticle")]
[Authorize(Roles = "Admin, User, Moderator, Redactor")]

public class LikesArticleController : Controller
{
    private readonly ILikesArticleDbRepository _likesArticleDbRepository;

    public LikesArticleController(ILikesArticleDbRepository likesArticleDbRepository)
    {
        _likesArticleDbRepository = likesArticleDbRepository;
    }
    /// <summary>
    ///  Add new like to article
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <param name="idArticle">int - Article Identifier</param>
    /// <returns>
    /// NotFound if user or article doesn't exist
    /// NotFound if like has been assigned
    /// Ok - if created successfully
    /// </returns>
    [HttpPost("{idUser:int}/{idArticle:int}")]
    public async Task<IActionResult> AddNewLike([FromRoute] int idUser,[FromRoute] int idArticle)
    {
        var likes = await _likesArticleDbRepository.AddLikeToArticle(idUser, idArticle);
        
        if (likes is not "Added like")
        {
            return BadRequest(new StringMessageDto{ Message = likes });
        }
        
        return Ok(new StringMessageDto{ Message = likes });
    }
    
    /// <summary>
    /// Delete like from article
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <param name="idArticle">int - Article Identifier</param>
    /// <returns>
    /// NotFound if user or article doesn't exist
    /// NotFound if like has not been assigned
    /// Ok - if deleted successfully
    /// </returns>
    [HttpDelete("{idUser:int}/{idArticle:int}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int idUser,[FromRoute] int idArticle)
    {
        var likes = await _likesArticleDbRepository.DeleteLikeFromArticle(idUser, idArticle);
        
        if (likes is not "Deleted like")
        {
            return BadRequest(new StringMessageDto{ Message = likes });
        }

        return Ok(new StringMessageDto{ Message = likes });
    }
    
    /// <summary>
    ///  Exist like
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <param name="idArticle">int - Article Identifier</param>
    /// <returns>
    /// NotFound if article or user doesn't exist
    /// return true - if exist
    /// </returns>
    [HttpGet("{idUser:int}/{idArticle:int}")]
    public async Task<IActionResult> ExistLike([FromRoute] int idUser,[FromRoute] int idArticle)
    {
        var likes = await _likesArticleDbRepository.ExistLikeArticle(idUser, idArticle);

        if (likes is null)
        {
            return NotFound(new StringMessageDto{ Message = "Article or user doesn't exist" }); 
        }
        
        return Ok(likes);
    }
}