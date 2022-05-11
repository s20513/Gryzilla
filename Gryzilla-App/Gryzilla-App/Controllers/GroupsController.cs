using Gryzilla_App.DTOs.Requests.Group;
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
            return NotFound("There is no group with given id");

        return Ok(group);
    }

    [HttpPut("{idGroup:int}")]
    public async Task<IActionResult> ModifyGroup([FromRoute] int idGroup, [FromBody] GroupRequestDto groupRequestDto)
    {
        var result = await _groupDbRepository.ModifyGroup(idGroup, groupRequestDto);

        if (result is null)
            return NotFound("There is no group with given id");

        return Ok(result);
    }

    [HttpDelete("{idGroup:int}")]
    public async Task<IActionResult> DeleteGroup([FromRoute] int idGroup)
    {
        var result = await _groupDbRepository.DeleteGroup(idGroup);
        
        if (result is null)
            return NotFound("There is no group with given id");

        return Ok(result);
    }

    [HttpDelete("user/{idGroup:int}")]
    public async Task<IActionResult> RemoveUserFromGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        var result = await _groupDbRepository.RemoveUserFromGroup(idGroup, userToGroupDto);

        if (result is null)
            return NotFound("Group or user was not found");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewGroup([FromBody] NewGroupRequestDto newGroupRequestDto)
    {
        var result =
            await _groupDbRepository.AddNewGroup(newGroupRequestDto.IdUser, newGroupRequestDto.GroupRequestDto);

        if (result is null)
            return NotFound("Group name already taken or wrong user id");

        return Ok(result);
    }

    [HttpPut("user/{idGroup:int}")]
    public async Task<IActionResult> AddUserToGroup([FromRoute] int idGroup, [FromBody] UserToGroupDto userToGroupDto)
    {
        var result = await _groupDbRepository.AddUserToGroup(idGroup, userToGroupDto);

        if (result is null)
            return NotFound("User or Group not found or user is already in group");

        return Ok(result);
    }
}