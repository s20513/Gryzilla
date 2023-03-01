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
    
    
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetProfileComments([FromRoute] int idUser)
    {
        var profileComment = await _profileCommentDbRepository.GetProfileCommentFromDb(idUser);

        if (profileComment is null)
        {
            return NotFound("There are no comments for the profile");
        }

        return Ok(profileComment);
    }
    
    [HttpPut("{idUser:int}")]
    public async Task<IActionResult> ModifyProfileComment([FromRoute] int idUser, [FromBody] ModifyProfileComment modifyProfileComment)
    {
        
        var profileComment = await _profileCommentDbRepository.ModifyProfileCommentFromDb(idUser, modifyProfileComment);
    
        if (profileComment == null)
        { 
            return NotFound("There is no comment for the profile");
        }
    
        return Ok(profileComment);
        
    }
    [HttpPost("{idUser:int}")]
    public async Task<IActionResult> PostProfileComment([FromBody] NewProfileComment newProfileComment, [FromRoute] int idUser)
    {
        var profileComment = await _profileCommentDbRepository.AddProfileCommentToDb(idUser, newProfileComment);
    
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