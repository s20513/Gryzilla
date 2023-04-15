using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/profileComments")]
public class ProfileCommentController : Controller
{
    private readonly IProfileCommentDbRepository _profileCommentDbRepository;
    public ProfileCommentController(IProfileCommentDbRepository profileCommentDbRepository)
    {
        _profileCommentDbRepository = profileCommentDbRepository;
    }
    
    
    [HttpGet("{idUserComments:int}")]
    public async Task<IActionResult> GetProfileComments([FromRoute] int idUserComments)
    {
        var profileComment = await _profileCommentDbRepository.GetProfileCommentFromDb(idUserComments);

        if (profileComment is null)
        {
            return NotFound("There are no comments for the profile");
        }

        return Ok(profileComment);
    }
    
    [HttpPut("{idProfileComment:int}")]
    public async Task<IActionResult> ModifyProfileComment([FromRoute] int idProfileComment, [FromBody] ModifyProfileComment modifyProfileComment)
    {
        
        var profileComment = await _profileCommentDbRepository.ModifyProfileCommentFromDb(idProfileComment, modifyProfileComment);
    
        if (profileComment == null)
        { 
            return NotFound("There is no comment for the profile");
        }
    
        return Ok(profileComment);
        
    }
    [HttpPost]
    public async Task<IActionResult> PostProfileComment([FromBody] NewProfileComment newProfileComment)
    {
        var profileComment = await _profileCommentDbRepository.AddProfileCommentToDb(newProfileComment);
    
        if (profileComment == null)
        { 
            return NotFound("User doesn't exist");
        }
    
        return Ok(profileComment);
        
    }
    
 
    [HttpDelete("{idProfileComment:int}")]
    public async Task<IActionResult> DeleteProfileComment([FromRoute] int idProfileComment)
    {
        var user = await _profileCommentDbRepository.DeleteProfileCommentFromDb(idProfileComment);
        
        if (user == null)
        { 
            return NotFound("Profile comment doesn't exist");
        }
        
        return Ok(user);
    }
    
}