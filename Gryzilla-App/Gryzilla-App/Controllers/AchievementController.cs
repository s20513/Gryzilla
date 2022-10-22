using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;
[ApiController]
[Route("api/achievement")]
public class AchievementController : Controller
{
    private readonly IAchievementDbRepository _achievementDbRepository;
    public AchievementController(IAchievementDbRepository achievementDbRepository)
    {
        _achievementDbRepository = achievementDbRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAchievements()
    {
        var achievementsFrom= await _achievementDbRepository.GetAchievementsFromDb();
        if (achievementsFrom is null)
            return NotFound("Not found any achievements");
        
        return Ok(achievementsFrom);
    }

    [HttpPut("{idAchievement:int}")]
    public async Task<IActionResult> ModifyAchievement([FromRoute] int idAchievement, [FromBody] PutAchievementDto putAchievementDto)
    {
        if (idAchievement != putAchievementDto.IdAchievement)
            return BadRequest();
        var achievementsFrom= await _achievementDbRepository.ModifyAchievement(idAchievement,putAchievementDto);
        if (achievementsFrom is null)
            return NotFound("Cannot modify achievement");
        
        return Ok(achievementsFrom);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewAchievement([FromBody] AddAchievementDto addAchievementDto ){
        var achievementsFrom= await _achievementDbRepository.AddNewAchievement(addAchievementDto);
        if (achievementsFrom is null)
            return NotFound("Cannot add new achievement");
        
        return Ok(achievementsFrom);
    }
    
    [HttpDelete("{idAchievement:int}")]
    public async Task<IActionResult> DeleteAchievement([FromRoute] int idAchievement)
    {
        var achievementsFrom= await _achievementDbRepository.DeleteAchievement(idAchievement);
        if (achievementsFrom is null)
            return NotFound("Cannot delete achievement");
        
        return Ok(achievementsFrom);
       
    }
    
    [HttpPost("{idAchievement:int}/{idUser:int}")]
    public async Task<IActionResult> AddNewUserAchievement([FromRoute] int idAchievement, [FromRoute] int idUser ){
        var achievementsFrom= await _achievementDbRepository.AddNewUserAchievement(idAchievement,idUser);
        
        if (achievementsFrom is null)
            return NotFound("Cannot add achievement to user");
        
        return Ok(achievementsFrom);
    }
    
    [HttpDelete("{idAchievement:int}/{idUser:int}")]
    public async Task<IActionResult> DeleteUserAchievement([FromRoute] int idAchievement, [FromRoute] int idUser )
    {
        var achievementsFrom= await _achievementDbRepository.DeleteUserAchievement(idAchievement, idUser);
        if (achievementsFrom is null)
            return NotFound("Cannot delete achievement from user");
        
        return Ok(achievementsFrom);
       
    }
    
}