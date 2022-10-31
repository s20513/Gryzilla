using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/likesPost")]
public class LikesController : Controller
{
    private readonly ILikesPostDbRepository _likesPostDbRepository;

    public LikesController(ILikesPostDbRepository likesPostDbRepository)
    {
        _likesPostDbRepository= likesPostDbRepository;
    }

    /// <summary>
    ///  Add new like
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns> 
    /// Status NotFound - If didn't find post or post  
    /// Status NotFound - If like has been assigned before
    /// Status Ok - added like successfully
    /// </returns>
    [HttpPost("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> AddNewLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.AddLikeToPost(idUser, idPost);
        
        if (likes != null && !likes.Equals("Added like"))
        {
            return NotFound(likes);
        }
        
        return Ok(likes);
    }
    
    /// <summary>
    ///  Delete like
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns> 
    /// Status NotFound - If didn't find post or post  
    /// Status NotFound - If like not been assigned before
    /// Status Ok - deleted like successfully
    /// </returns>
    [HttpDelete("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.DeleteLikeFromPost(idUser, idPost);
        
        if (likes != null && !likes.Equals("Deleted like"))
        {
            return NotFound(likes);
        }

        return Ok(likes);
    }
    
    /// <summary>
    ///  Exist method
    /// </summary>
    /// <param name="idUser">int idUser - User Identifier </param>
    /// <param name="idPost">int idPost - Post Identifier </param>
    /// <returns>
    /// true if like has been assigned, false - if not
    /// NotFound if user or post doesn't exist
    /// </returns>
    [HttpGet("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> GetLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.ExistLike(idUser, idPost);
        
        if (likes is null)
        {
            return NotFound("Post or user doesn't exist"); 
        }
        return Ok(likes);
    }
}