using Gryzilla_App.DTOs.Requests.Group;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
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

    [HttpGet("{idGroup:int}")]
    public async Task<IActionResult> GetGroup([FromRoute] int idGroup)
    {
        var group = await _groupDbRepository.GetGroup(idGroup);

        if (group is null)
        {
            return NotFound("There is no group with given id");
        }

        return Ok(group);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var group = await _groupDbRepository.GetGroups();

        if (group.Length < 1)
        {
            return NotFound("There is no groups");
        }
        return Ok(group);
    }

    [HttpPut("{idGroup:int}")]
    public async Task<IActionResult> ModifyGroup([FromRoute] int idGroup, [FromBody] GroupRequestDto groupRequestDto)
    {
        if (idGroup != groupRequestDto.IdGroup)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }

        try
        {
            var result = await _groupDbRepository.ModifyGroup(idGroup, groupRequestDto);
            
            if (result is null)
            {
                return NotFound("There is no group with given id");
            }
            
            return Ok(result);
            
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{idGroup:int}")]
    public async Task<IActionResult> DeleteGroup([FromRoute] int idGroup)
    {
        var result = await _groupDbRepository.DeleteGroup(idGroup);

        if (result is null)
        {
            return NotFound("There is no group with given id");
        }

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNewGroup([FromBody] NewGroupRequestDto newGroupRequestDto)
    {
        try
        {
            var result = await _groupDbRepository.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto);

            if (result is null)
            {
                return NotFound("Cannot add group or wrong userId");
            }
            return Ok(result);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("user/{idGroup:int}")]
    public async Task<IActionResult> RemoveUserFromGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        if (idGroup != userToGroupDto.IdGroup)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }

        try
        {
            var result = await _groupDbRepository.RemoveUserFromGroup(idGroup, userToGroupDto);

            if (result is null)
            {
                return NotFound("Group or user was not found"); 
            }

            return Ok(result);
        }
        catch (UserCreatorException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("user/{idGroup:int}")]
    public async Task<IActionResult> AddUserToGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        if (idGroup != userToGroupDto.IdGroup)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }

        var result = await _groupDbRepository.AddUserToGroup(idGroup, userToGroupDto);

        if (result is null)
        {
            return NotFound("User or Group not found"); 
        }

        return Ok(result);
    }

    [HttpGet("{idUser:int}/{idGroup:int}")]
    public async Task<IActionResult>ExistUserInTheGroup([FromRoute] int idGroup, [FromRoute] int idUser)
    {
        var result = await _groupDbRepository.ExistUserInTheGroup(idGroup, idUser);
        
        return Ok(result);
    }

    [HttpGet("user/{idUser:int}")]
    public async Task<IActionResult> GetUserGroups([FromRoute] int idUser)
    {
        var group = await _groupDbRepository.GetUserGroups(idUser);

        if (group.Length < 1)
        {
            return NotFound("There is no groups");
        }
        return Ok(group);
    }
}