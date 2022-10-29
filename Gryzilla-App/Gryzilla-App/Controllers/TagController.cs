using Gryzilla_App.DTOs.Requests.Tag;
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
    
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagDbRepository.GetTagsFromDb();
        
        if (tags is null)
        {
            return NotFound("No tags found");
        }

        return Ok(tags);
    }
    
    [HttpGet("{idTag:int}")]
    public async Task<IActionResult> GetTag([FromRoute] int idTag)
    {
        var tag = await _tagDbRepository.GetTagFromDb(idTag);
        
        if (tag is null)
        {
            return NotFound("No tags found");
        }

        return Ok(tag);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetTag([FromBody] NewTagDto newTagDto)
    {
        try
        {
            var tag = await _tagDbRepository.AddTagToDb(newTagDto);

            return Ok(tag);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }
}