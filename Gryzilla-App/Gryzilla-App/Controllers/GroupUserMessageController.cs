using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/groupsMessage")]
public class GroupUserMessageController : Controller
{
    private readonly IGroupUserMessageDbRepository _groupUserMessageDbRepository;

    public GroupUserMessageController(IGroupUserMessageDbRepository groupUserMessageDbRepository)
    {
        _groupUserMessageDbRepository = groupUserMessageDbRepository;
    }
    
    /// <summary>
    /// Get list of messages
    /// </summary>
    /// <returns>
    /// NotFound - if any message doesn't exist
    /// Ok - List of group
    /// </returns>
    [HttpGet("{idGroup:int}")]
    public async Task<IActionResult> GetMessages([FromRoute] int idGroup)
    {
        var group = await _groupUserMessageDbRepository.GetMessages(idGroup);

        if (group.Length < 1)
        {
            return NotFound("There is no messages for this group");
        }
        
        return Ok(group);
    }
    /// <summary>
    /// Modify message
    /// </summary>
    /// <param name="idMessage">Id message</param>
    /// <param name="updateGroupUserMessage">update group user message</param>
    /// <returns>
    /// BadRequest - Id from route and Id in body have to be same
    /// NotFound - There is no group with given id
    /// Ok - return modified group
    /// </returns>
    [HttpPut("{idMessage:int}")]
    public async Task<IActionResult> ModifyMessage([FromRoute] int idMessage, [FromBody] UpdateGroupUserMessageDto updateGroupUserMessage)
    {
        if (idMessage != updateGroupUserMessage.IdMessage)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }
        try
        {
            var result = await _groupUserMessageDbRepository.ModifyMessage(idMessage, updateGroupUserMessage);
            
            if (result is null)
            {
                return NotFound("There is no message with given id");
            }
            
            return Ok(result);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }
    /// <summary>
    /// Delete message
    /// </summary>
    /// <param name="idMessage">int - Message Identifier</param>
    /// <returns>
    /// NotFound - There is no group with given id
    /// Ok - return body of group
    /// </returns>
    [HttpDelete("{idMessage:int}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] int idMessage)
    {
        var result = await _groupUserMessageDbRepository.DeleteMessage(idMessage);

        if (result is null)
        {
            return NotFound("There is no message with given id");
        }

        return Ok(result);
    }
    /// <summary>
    /// Create new message
    /// </summary>
    /// <param name="addGroupUserMessageDto">Dto - to store information about new message</param>
    /// <returns>
    /// NotFound - Cannot add group or wrong userId
    /// Ok - return new post
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateNewMessage([FromBody] AddGroupUserMessageDto addGroupUserMessageDto)
    {
        var result = await _groupUserMessageDbRepository.AddMessage(addGroupUserMessageDto);

        if (result is null)
        {
            return NotFound("Wrong userId or groupId");
        }
        return Ok(result);
    }
}