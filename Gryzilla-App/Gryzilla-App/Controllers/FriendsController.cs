using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/friends")]
public class FriendsController : Controller
{
    private readonly IFriendsDbRepository _friendsDbRepository;
    public FriendsController(IFriendsDbRepository friendsDbRepository)
    {
        _friendsDbRepository = friendsDbRepository;
    }

    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetFriends([FromRoute] int idUser)
    {
        
        var friends = await _friendsDbRepository.GetFriendsFromDb(idUser);
        if (friends is null)
        {
            return NotFound("No friends found");
        }
        return Ok(friends);
    }
    [HttpDelete("{idUser:int}")]
    public async Task<IActionResult> DeleteFriend([FromRoute] int idUser, int idUserFriend)
    {
        if (idUser == null || idUserFriend == null)
        {
            return NotFound("IdUser or IdUserFriend is null");
        }
        var friends = await _friendsDbRepository.DeleteFriendFromDb(idUser, idUserFriend);
        if (friends is null)
        {
            return NotFound("Cannot removed friend");
        }
        return Ok(friends);
    }
    
    [HttpPost("{idUser:int}")]
    public async Task<IActionResult> AddNewFriend([FromRoute] int idUser, int idUserFriend)
    {
        if (idUser == null || idUserFriend == null)
        {
            return NotFound("IdUser or IdUserFriend is null");
        }
        var friends = await _friendsDbRepository.AddNewFriendFromDb(idUser, idUserFriend);
        if (friends is null)
        {
            return NotFound("Cannot added friend");
        }
        else if (friends == "is friend")
        {
            return Ok("The user is already your friend");
        }
        return Ok(friends);
    }
}