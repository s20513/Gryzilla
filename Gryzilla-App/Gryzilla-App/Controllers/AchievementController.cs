using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.Exceptions;
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
    
    /// <summary>
    /// Find all achievements from db
    /// </summary>
    /// <returns>Return Status OK - if any Achievement exists, return user Achievements</returns>
    /// <returns>Return Status Not Found - no Achievements in db</returns>
    [HttpGet]
    public async Task<IActionResult> GetAchievements()
    {
        var achievements= await _achievementDbRepository.GetAchievementsFromDb();

        if (achievements is null)
        {
            return NotFound("Not found any achievements");
        }
        
        return Ok(achievements);
    }
    
    /// <summary>
    /// Modify Achievement 
    /// </summary>
    /// <param name="idAchievement">Int - Achievement Identifier</param>
    /// <param name="putAchievementDto">Dto to store new Achievement information</param>
    /// <returns>Return Status Ok - information about Achievement modified correctly, returns Achievement body</returns>
    /// <returns>Return Status Not Found - Achievement doesn't exist</returns>
    /// <returns>Return Status Bad Request - Id from route and Id in body are not some</returns>
    [HttpPut("{idAchievement:int}")]
    public async Task<IActionResult> ModifyAchievement([FromRoute] int idAchievement, [FromBody] PutAchievementDto putAchievementDto)
    {
        if (idAchievement != putAchievementDto.IdAchievement)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }
            
        var modifiedAchievement= await _achievementDbRepository.ModifyAchievement(idAchievement, putAchievementDto);

        if (modifiedAchievement is null)
        {
            return NotFound("Achievement not found");
        }
        
        return Ok(modifiedAchievement);
    }

    /// <summary>
    /// Add new Achievement
    /// </summary>
    /// <param name="addAchievementDto">Dto to store Achievement information</param>
    /// <returns> Return Status Ok - New Achievement added correctly, returns Achievement body</returns>
    /// <returns>Return status Bad Request - Achievement with same name exists in db</returns>
    [HttpPost]
    public async Task<IActionResult> AddNewAchievement([FromBody] AddAchievementDto addAchievementDto )
    {
        try
        {
            var achievement= await _achievementDbRepository.AddNewAchievement(addAchievementDto);
            
            return Ok(achievement);
        }
        catch (SameNameException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Delete achievement
    /// </summary>
    /// <param name="idAchievement">Achievement Id</param>
    /// <returns> Return Status Ok - Achievement deleted correctly, returns Achievement body</returns>
    /// <returns>Return status Bad Request - Can't delete Achievement. There are users who have it</returns>
    /// <returns>Return status Not Found - Achievement not found</returns>
    [HttpDelete("{idAchievement:int}")]
    public async Task<IActionResult> DeleteAchievement([FromRoute] int idAchievement)
    {
        try
        {
            var achievement = await _achievementDbRepository.DeleteAchievement(idAchievement);

            if (achievement is null)
            {
                return NotFound("Achievement not found");
            }

            return Ok(achievement);
        }
        catch (ReferenceException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Add new UserAchievement
    /// </summary>
    /// <param name="idAchievement">Achievement Id</param>
    /// <param name="idUser">User Id</param>
    /// <returns> Return Status Ok - Achievement added to User</returns>
    /// <returns>Return status Bad Request - User already has Achievement</returns>
    /// <returns>Return status Not Found - Achievement or User not found</returns>
    [HttpPost("{idAchievement:int}/{idUser:int}")]
    public async Task<IActionResult> AddNewUserAchievement([FromRoute] int idAchievement, [FromRoute] int idUser )
    {
        try
        {
            var achievement = await _achievementDbRepository.AddNewUserAchievement(idAchievement, idUser);
            
            if (achievement is null)
            {
                return NotFound("Cannot find user or achievement");
            }
        
            return Ok(achievement);
        }
        catch (ReferenceException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Delete UserAchievement
    /// </summary>
    /// <param name="idAchievement">Achievement Id</param>
    /// <param name="idUser">User Id</param>
    /// <returns> Return Status Ok - Achievement removed from User</returns>
    /// <returns>Return status Not Found - Achievement, User or UserAchievement not found</returns>
    [HttpDelete("{idAchievement:int}/{idUser:int}")]
    public async Task<IActionResult> DeleteUserAchievement([FromRoute] int idAchievement, [FromRoute] int idUser )
    {
        var achievement= await _achievementDbRepository.DeleteUserAchievement(idAchievement, idUser);

        if (achievement is null)
        {
            return NotFound("Cannot delete achievement from user");
        }
        
        return Ok(achievement);
    }
    
    /// <summary>
    /// Find all user achievements
    /// </summary>
    /// <param name="idUser">User Id</param>
    /// <returns> Return Status Ok - Return User Achievements</returns>
    /// <returns>Return status Not Found - Not found User with given id</returns>
    [HttpGet("/user/{idUser:int}")]
    public async Task<IActionResult> GetUserAchievements([FromRoute] int idUser)
    {
        var achievements= await _achievementDbRepository.GetUserAchievements(idUser);

        if (achievements is null)
        {
            return NotFound("User not found");
        }
        
        return Ok(achievements);
    }

}