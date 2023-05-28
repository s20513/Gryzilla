using Gryzilla_App.DTOs.Requests.ProfileComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            return NotFound(new StringMessageDto{ Message = "There are no comments for the profile" });
        }

        return Ok(profileComment);
    }
    
    [HttpPut("{idProfileComment:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyProfileComment([FromRoute] int idProfileComment, [FromBody] ModifyProfileComment modifyProfileComment)
    {
        
        var profileComment = await _profileCommentDbRepository.ModifyProfileCommentFromDb(idProfileComment, modifyProfileComment, User);
    
        if (profileComment == null)
        { 
            return NotFound(new StringMessageDto{ Message = "There is no comment for the profile" });
        }
    
        return Ok(profileComment);
        
    }
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddProfileComment([FromBody] NewProfileComment newProfileComment)
    {
        var profileComment = await _profileCommentDbRepository.AddProfileCommentToDb(newProfileComment);
    
        if (profileComment == null)
        { 
            return NotFound(new StringMessageDto{ Message = "User doesn't exist" });
        }
    
        return Ok(profileComment);
        
    }
    
 
    [HttpDelete("{idProfileComment:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteProfileComment([FromRoute] int idProfileComment)
    {
        var user = await _profileCommentDbRepository.DeleteProfileCommentFromDb(idProfileComment, User);
        
        if (user == null)
        { 
            return NotFound(new StringMessageDto{ Message = "Profile comment doesn't exist" });
        }
        
        return Ok(user);
    }
    
}