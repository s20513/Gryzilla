using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;
[ApiController]
[Route("api/links")]
public class LinkController : Controller
    {
    private readonly ILinkDbRepository _linkDbRepository;

    public LinkController(ILinkDbRepository linkDbRepository)
    {
        _linkDbRepository = linkDbRepository;
    }
    
    /// <summary>
    /// Get users steam and discord links
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinksDto</returns>
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUserLinks([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.GetUserLinks(idUser);
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        return Ok(link);
    }
    
    /// <summary>
    /// Enter discord link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpPut("discord")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> PutDiscordLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkDiscord(linkDto, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Enter steam link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpPut("steam")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> PutSteamLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkSteam(linkDto, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Enter playstation link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpPut("ps")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> PutPsLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkPs(linkDto, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Enter xbox link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpPut("xbox")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> PutXboxLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkXbox(linkDto, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Enter epic link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpPut("epic")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> PutEpicLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkEpic(linkDto, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Delete discord link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpDelete("discord/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteDiscordLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkDiscord(idUser, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Delete steam link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpDelete("steam/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteSteamLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkSteam(idUser, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Delete playstation link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpDelete("ps/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeletePsLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkPs(idUser, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Delete xbox link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpDelete("xbox/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteXboxLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkXbox(idUser, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
    
    /// <summary>
    /// Delete epic link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpDelete("epic/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteEpicLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkEpic(idUser, User);
        
        if (link is null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" }); 
        }
        
        return Ok(new StringMessageDto{ Message = link });
    }
}