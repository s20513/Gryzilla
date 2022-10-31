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
    
    [HttpDelete("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.DeleteLikeToPost(idUser, idPost);
        
        if (likes != null && !likes.Equals("Deleted like"))
        {
            return NotFound(likes);
        }

        return Ok(likes);
    }
    
    [HttpGet("{idUser:int}/{idPost:int}")]
    public async Task<IActionResult> GetLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var likes = await _likesPostDbRepository.ExistLike(idUser, idPost);
        
        return Ok(likes);
    }
}