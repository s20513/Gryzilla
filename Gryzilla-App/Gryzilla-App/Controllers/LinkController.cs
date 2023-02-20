using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.Repositories.Interfaces;
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
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(link);
    }
    
    /// <summary>
    /// Enter discord link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpPut("discord")]
    public async Task<IActionResult> PutDiscordLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkDiscord(linkDto);
        
        if (link is null)
        {
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(link);
    }
    
    /// <summary>
    /// Enter steam link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpPut("steam")]
    public async Task<IActionResult> PutSteamLink([FromBody] LinkDto linkDto)
    {
        var link = await _linkDbRepository.PutLinkDiscord(linkDto);
        
        if (link is null)
        {
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(link);
    }
    
    /// <summary>
    /// Delete discord link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>string</returns>
    [HttpDelete("discord/{idUser:int}")]
    public async Task<IActionResult> DeleteDiscordLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkDiscord(idUser);
        
        if (link is null)
        {
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(link);
    }
    
    /// <summary>
    /// Delete steam link
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <returns>LinkDto</returns>
    [HttpDelete("steam/{idUser:int}")]
    public async Task<IActionResult> DeleteSteamLink([FromRoute] int idUser)
    {
        var link = await _linkDbRepository.DeleteLinkSteam(idUser);
        
        if (link is null)
        {
            return NotFound("User doesn't exist"); 
        }
        
        return Ok(link);
    }
}