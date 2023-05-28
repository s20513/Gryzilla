using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Get user's friends
    /// </summary>
    /// <param name="idUser">User id</param>
    /// <returns>Return Status Ok - Returns list of user's friends</returns>
    /// <returns>Return Status Not Found - User does not exists</returns>
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetFriends([FromRoute] int idUser)
    {
        var friends = await _friendsDbRepository.GetFriendsFromDb(idUser);
        
        if (friends is null)
        {
            return NotFound(new StringMessageDto{ Message = "User does not exists!" });
        }
        
        return Ok(friends);
    }
    
    /// <summary>
    /// Deletes user's friend
    /// </summary>
    /// <param name="idUser">User id</param>
    /// <param name="idUserFriend">Id of user's friend</param>
    /// <returns>Return Status Ok - Returns nick and id of ex-friend</returns>
    /// <returns>Return Status Not Found - One of the users does not exists</returns>
    /// <returns>Return Bad Request - Given Ids are the same</returns>
    [HttpDelete("{idUser:int}/{idUserFriend:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteFriend([FromRoute] int idUser, [FromRoute] int idUserFriend)
    {
        if (idUser == idUserFriend)
        {
            return BadRequest(new StringMessageDto{ Message = "Ids must have different values!" });
        }
        
        var friend = await _friendsDbRepository.DeleteFriendFromDb(idUser, idUserFriend);
        
        if (friend is null)
        {
            return NotFound(new StringMessageDto{ Message = "One of the users does not exists!" });
        }
        return Ok(friend);
    }
    
    /// <summary>
    /// Adds user1 to user2 as friend and vice versa
    /// </summary>
    /// <param name="idUser">User id</param>
    /// <param name="idUserFriend">Id of user's friend</param>
    /// <returns>Return Status Ok - Returns nick and id of friend</returns>
    /// <returns>Return Status Not Found - One of the users does not exists</returns>
    /// /// <returns>Return Bad Request - Given Ids are the same or users are already friends</returns>
    [HttpPost("{idUser:int}/{idUserFriend:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddNewFriend([FromRoute] int idUser, [FromRoute] int idUserFriend)
    {
        if (idUser == idUserFriend)
        {
            return BadRequest(new StringMessageDto{ Message = "Ids must have different values!" });
        }
        
        try
        {
            var friend = await _friendsDbRepository.AddNewFriendToDb(idUser, idUserFriend);
            
            if (friend is null)
            {
                return NotFound(new StringMessageDto{ Message = "One of the users does not exists!" });
            }

            return Ok(friend);
        }
        catch (ReferenceException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
    }
}