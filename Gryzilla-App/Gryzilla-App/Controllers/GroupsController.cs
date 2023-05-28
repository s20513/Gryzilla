using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupsController: Controller
{
    private readonly IGroupDbRepository _groupDbRepository;

    public GroupsController(IGroupDbRepository groupDbRepository)
    {
        _groupDbRepository = groupDbRepository;
    }
    /// <summary>
    /// Get group by id
    /// </summary>
    /// <param name="idGroup">int - Group Identifier </param>
    /// <returns>
    /// NotFound - There is no group with given id
    /// Ok - return group with this id
    /// </returns>
    [HttpGet("{idGroup:int}")]
    public async Task<IActionResult> GetGroup([FromRoute] int idGroup)
    {
        var group = await _groupDbRepository.GetGroup(idGroup);

        if (group is null)
        {
            return NotFound(new StringMessageDto{ Message = "There is no group with given id" });
        }

        return Ok(group);
    }
    /// <summary>
    /// Get list of groups
    /// </summary>
    /// <returns>
    /// NotFound - if any group doesn't exist
    /// Ok - List of group
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var group = await _groupDbRepository.GetGroups();

        return Ok(group);
    }
    /// <summary>
    /// Modify group
    /// </summary>
    /// <param name="idGroup"></param>
    /// <param name="groupRequestDto"></param>
    /// <returns>
    /// BadRequest - Id from route and Id in body have to be same or group with this name exist
    /// NotFound - There is no group with given id
    /// Ok - return modified group
    /// </returns>
    [HttpPut("{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyGroup([FromRoute] int idGroup, [FromBody] GroupRequestDto groupRequestDto)
    {
        if (idGroup != groupRequestDto.IdGroup)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }

        try
        {
            var result = await _groupDbRepository.ModifyGroup(idGroup, groupRequestDto, User);
            
            if (result is null)
            {
                return NotFound(new StringMessageDto{ Message = "There is no group with given id" });
            }
            
            return Ok(result);
            
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
    }
    /// <summary>
    /// Delete group
    /// </summary>
    /// <param name="idGroup">int - Group Identifier</param>
    /// <returns>
    /// NotFound - There is no group with given id
    /// Ok - return body of group
    /// </returns>
    [HttpDelete("{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteGroup([FromRoute] int idGroup)
    {
        var result = await _groupDbRepository.DeleteGroup(idGroup, User);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "There is no group with given id" });
        }

        return Ok(result);
    }
    /// <summary>
    /// Create new group
    /// </summary>
    /// <param name="newGroupRequestDto">Dto - to store information about new group</param>
    /// <returns>
    /// BadRequest - if group with this name exist
    /// NotFound - Cannot add group or wrong userId
    /// Ok - return new post
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> CreateNewGroup([FromBody] NewGroupRequestDto newGroupRequestDto)
    {
        try
        {
            var result = await _groupDbRepository.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto);

            if (result is null)
            {
                return NotFound(new StringMessageDto{ Message = "Cannot add group or wrong userId" });
            }
            return Ok(result);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
    }
    /// <summary>
    /// Delete user from group
    /// </summary>
    /// <param name="idGroup">int - Group Identifier</param>
    /// <param name="userToGroupDto">Body - to store information</param>
    /// <returns>
    /// BadRequest - Id from route and Id in body have to be same
    /// NotFound - User or Group not found
    /// Ok - Return group
    /// </returns>
    [HttpDelete("user/{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> RemoveUserFromGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        if (idGroup != userToGroupDto.IdGroup)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }

        try
        {
            var result = await _groupDbRepository.RemoveUserFromGroup(idGroup, userToGroupDto, User);

            if (result is null)
            {
                return NotFound(new StringMessageDto{ Message = "Group or user was not found" }); 
            }

            return Ok(result);
        }
        catch (UserCreatorException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
    }
    /// <summary>
    /// Add user to group
    /// </summary>
    /// <param name="idGroup">int - Group Identifier</param>
    /// <param name="userToGroupDto">Body - to store information</param>
    /// <returns>
    /// BadRequest - Id from route and Id in body have to be same
    /// NotFound - User or Group not found
    /// Ok - Return group
    /// </returns>
    [HttpPost("user/{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddUserToGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        if (idGroup != userToGroupDto.IdGroup)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }

        var result = await _groupDbRepository.AddUserToGroup(idGroup, userToGroupDto);

        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "User or Group not found" }); 
        }

        return Ok(result);
    }
    /// <summary>
    /// method to check if there is a user in the group
    /// </summary>
    /// <param name="idGroup">int - Group Identifier </param>
    /// <param name="idUser">int - User Identifier</param>
    /// <returns>
    /// return true - if exist
    /// </returns>
    [HttpGet("{idUser:int}/{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult>ExistUserInTheGroup([FromRoute] int idGroup, [FromRoute] int idUser)
    {
        var result = await _groupDbRepository.UserIsInGroup(idGroup, idUser);
        
        return Ok(result);
    }

    /// <summary>
    /// Return user groups
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <returns>
    /// NotFound - if user don't have any groups
    /// Ok - list of groups
    /// </returns>
    [HttpGet("user/{idUser:int}")]
    public async Task<IActionResult> GetUserGroups([FromRoute] int idUser)
    {
        var group = await _groupDbRepository.GetUserGroups(idUser);
        
        return Ok(group);
    }
    
    [HttpPost("photo/{idGroup:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> SetGroupPhoto([FromForm] IFormFile file,[FromRoute] int idGroup)
    {
        var res = await _groupDbRepository.SetGroupPhoto(file, idGroup, User);
        
        return Ok(res);
    }
    
    [HttpGet("photo/{idGroup:int}")]
    public async Task<IActionResult> GetGroupPhoto([FromRoute] int idGroup)
    {
        var res = await _groupDbRepository.GetGroupPhoto(idGroup);
        
        return Ok(res);
    }
}