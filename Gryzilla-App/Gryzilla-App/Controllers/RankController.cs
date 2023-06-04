using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/rank")]
[Authorize(Roles = "Admin, Moderator")]
public class RankController : Controller
{
    private readonly IRankDbRepository _ranksDbRepository;

    public RankController(IRankDbRepository ranksDbRepository)
    {
        _ranksDbRepository = ranksDbRepository;
    }
    
    /// <summary>
    ///  Add new rank
    /// </summary>
    /// <param name="addRankDto"> Dto - to store information about new rank </param>
    /// <returns>
    ///  Return Status NotFound  - if cannot add new rank or the rank name is taken
    ///  Return Status Ok        - If the rank was added successfully
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> PostNewRank([FromBody] AddRankDto addRankDto)
    {
        try
        {
            var rank = await _ranksDbRepository.AddNewRank(addRankDto);

            if (rank is null)
            {
                return NotFound(new StringMessageDto{ Message ="Cannot add new rank"});
            }

            return Ok(rank);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message =e.Message});
        }
    }
    
    /// <summary>
    ///  Delete rank by IdRank
    /// </summary>
    /// <param name="idRank"> Int - Rank Identifier</param>
    /// <returns>
    ///  Return Status NotFound    - If cannot find rank.
    ///  Return Status Ok and body - If the rank was deleted successfully
    ///  Return Status BadRequest  - If the user is assigned this rank
    /// </returns>
    [HttpDelete("{idRank:int}")]
    public async Task<IActionResult> DeleteRank([FromRoute] int idRank)
    {
        try
        {
            var rank = await _ranksDbRepository.DeleteRank(idRank);
        
            if (rank is null)
            {
                return NotFound(new StringMessageDto{ Message ="Cannot delete rank"});
            }
            return Ok(rank);
        }
        catch (ReferenceException e)
        {
            return BadRequest(new StringMessageDto{ Message =e.Message});
        }
       
    }
    
    /// <summary>
    /// Modify rank by idRank
    /// </summary>
    /// <param name="putRankDto">Dto - to store new Rank information</param>
    /// <param name="idRank">Int - Rank Identifier </param>
    /// <returns>
    ///  Return Status BadRequest   - If idRank != putRankDto.IdRank or rank name is taken
    ///  Return Status NotFound     - If cannot find or modify rank
    ///  Return Status Ok and body  - If the rank was modified successfully
    /// </returns>
    [HttpPut("{idRank:int}")]
    public async Task<IActionResult> ModifyRank([FromBody] PutRankDto putRankDto, [FromRoute] int idRank)
    {
        if (idRank != putRankDto.IdRank)
            return BadRequest(new StringMessageDto{ Message ="Id from route and Id in body have to be same"});

        try
        {
            var rank = await _ranksDbRepository.ModifyRank(putRankDto, idRank);
        
            if (rank is null)
            {
                return NotFound(new StringMessageDto{ Message ="Cannot modify rank"});
            }
        
            return Ok(rank);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message =e.Message});
        }
    }
    
    /// <summary>
    /// Get ranks
    /// </summary>
    /// <returns>ranks</returns>
    [HttpGet]
    public async Task<IActionResult> GetRanks()
    {
        var group = await _ranksDbRepository.GetRanks();

        return Ok(group);
    }
}