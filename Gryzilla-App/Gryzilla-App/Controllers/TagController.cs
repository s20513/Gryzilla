using Gryzilla_App.DTOs.Requests.Tag;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController: Controller
{
    private readonly ITagDbRepository _tagDbRepository;

    public TagController(ITagDbRepository tagDbRepository)
    {
        _tagDbRepository = tagDbRepository;
    }
    
    /// <summary>
    ///  Get all tags
    /// </summary>
    /// <returns>
    /// NotFound - if there is no tag
    /// OK - return list of tags
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagDbRepository.GetTagsFromDb();
        
        if (tags is null)
        {
            return NotFound(new StringMessageDto{ Message = "No tags found"});
        }

        return Ok(tags);
    }
    
    /// <summary>
    /// Get tag by id
    /// </summary>
    /// <param name="idTag"> int - Tag Identifier </param>
    /// <returns>
    /// NotFound if tag doesn't exist
    /// Ok - return body Tag
    /// </returns>
    [HttpGet("{idTag:int}")]
    public async Task<IActionResult> GetTag([FromRoute] int idTag)
    {
        var tag = await _tagDbRepository.GetTagFromDb(idTag);
        
        if (tag is null)
        {
            return NotFound(new StringMessageDto{ Message = "There is no tag with given id"});
        }

        return Ok(tag);
    }
    
    /// <summary>
    /// Add new Tag
    /// </summary>
    /// <param name="newTagDto"> Dto to store Tag information </param>
    /// <returns>
    /// BadRequest - if name of the tag is already taken
    /// Ok - return body Tag
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> AddTag([FromBody] NewTagDto newTagDto)
    {
        try
        {
            var tag = await _tagDbRepository.AddTagToDb(newTagDto);
            
            if (tag is null)
            {
                return NotFound(new StringMessageDto{ Message = "Cannot add new tag"});
            }
            
            return Ok(tag);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
    }
    
    /// <summary>
    ///  Get tags starting, which name starts with startingText
    /// </summary>
    /// <param name="startOfTagName"> string which determines the start on tag names </param>
    /// <returns>
    /// NotFound - if there is no tag which starts with startingText value
    /// OK - return list of tags which starts with startingText value
    /// </returns>
    [HttpGet("{startOfTagName}")]
    public async Task<IActionResult> GetTagsStartingWithParam(string startOfTagName)
    {
        var tags = await _tagDbRepository.GetTagsStartingWithParamFromDb(startOfTagName);

        return Ok(tags);
    }
}