using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.BlockedUser;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/block")]
[Authorize(Roles = "Admin, Moderator")]
public class BlockedUserController: Controller
{
    private readonly IBlockedUserDbRepository _blockedUserDbRepository;


    public BlockedUserController(IBlockedUserDbRepository blockedUserDbRepository)
    {
        _blockedUserDbRepository = blockedUserDbRepository;
    }
    
    /// <summary>
    /// Get all blocked Users
    /// </summary>
    /// <returns>Return Status OK - returns list of blocked Users</returns>
    [HttpGet]
    public async Task<IActionResult> GetBlockedUsers()
    {
        var blockedUsers = await _blockedUserDbRepository.GetBlockedUsers();

        return Ok(blockedUsers);
    }
    
    /// <summary>
    /// Block User
    /// </summary>
    /// <param name="blockedUserRequestDto">BlockedUserRequestDto - User id to block, user id blocking comment</param>
    /// <returns>Return Status OK - returns new blocked user info</returns>
    /// <returns>Return Status NotFound - returns string if one of the users does not exist</returns>
    [HttpPost]
    public async Task<IActionResult> BlockUser([FromBody] BlockedUserRequestDto blockedUserRequestDto)
    {
        var blockedUser = await _blockedUserDbRepository.BlockUser(blockedUserRequestDto);

        if (blockedUser is null)
        {
            return NotFound(new StringMessageDto{ Message = "One of the users does not exist" });
        }

        return Ok(blockedUser);
    }
    
    /// <summary>
    /// Block User
    /// </summary>
    /// <param name="idUser">int - User id to unlock</param>
    /// <returns>Return Status OK - returns string info of unlocking user</returns>
    /// <returns>Return Status NotFound - returns string if user does not exist</returns>
    [HttpDelete("{idUser:int}")]
    public async Task<IActionResult> UnlockUser([FromRoute] int idUser)
    {
        var blockedUser = await _blockedUserDbRepository.UnlockUser(idUser);

        if (blockedUser is null)
        {
            return NotFound(new StringMessageDto{ Message = "Users does not exist" });
        }

        return Ok(blockedUser);
    }
    
    /// <summary>
    /// GetUserBlockingHistory
    /// </summary>
    /// <param name="idUser">int - User id whose history to get </param>
    /// <returns>Return Status OK - returns user blocking history</returns>
    /// <returns>Return Status NotFound - returns string if user does not exist</returns>
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUserBlockingHistory(int idUser)
    {
        var userBlockingHistory = await _blockedUserDbRepository.GetUserBlockingHistory(idUser);

        if (userBlockingHistory is null)
        {
            return NotFound(new StringMessageDto{ Message = "Users does not exist" });
        }

        return Ok(userBlockingHistory);
    }
    
    /// <summary>
    /// CheckIfUserIsBlocked
    /// </summary>
    /// <param name="idUser">int - User id </param>
    /// <returns>Return Status OK - bool if user is blocked</returns>
    /// <returns>Return Status NotFound - returns string if user does not exist</returns>
    [HttpGet("check/{idUser:int}")]
    public async Task<IActionResult> CheckIfUserIsBlocked(int idUser)
    {
        var blockedUser = await _blockedUserDbRepository.CheckIfUserIsBlocked(idUser);

        if (blockedUser is null)
        {
            return NotFound(new StringMessageDto{ Message = "Users does not exist" });
        }

        return Ok(blockedUser);
    }
    
}