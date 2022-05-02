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

    [HttpPost("{idUser:int},{idPost:int}")]
    public async Task<IActionResult> AddNewLike([FromRoute] int idUser,[FromRoute] int idPost)
    {

        var posts = await _likesPostDbRepository.AddLikeToPost(idUser, idPost);
        if (posts is null)
        {
            return NotFound("Cannot add like");
        }

        return Ok(posts);
    }
    [HttpDelete("{idUser:int},{idPost:int}")]
    public async Task<IActionResult> DeleteLike([FromRoute] int idUser,[FromRoute] int idPost)
    {
        var posts = await _likesPostDbRepository.DeleteLikeToPost(idUser, idPost);
        if (posts is null)
        {
            return NotFound("Cannot delete like");
        }

        return Ok(posts);
    }
}