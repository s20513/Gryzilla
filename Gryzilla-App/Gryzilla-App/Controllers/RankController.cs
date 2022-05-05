using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;


[ApiController]
[Route("api/rank")]
public class RankController : Controller
{
    private readonly IRankDbRepository _ranksDbRepository;

    public RankController(IRankDbRepository ranksDbRepository)
    {
        _ranksDbRepository = ranksDbRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostNewRank([FromBody] AddRankDto addRankDto)
    {
        var rank = await _ranksDbRepository.AddNewRank(addRankDto);
        if (rank is null)
        {
            return NotFound("Cannot add new rank");
        }
        return Ok(rank);
    }
    
    [HttpDelete("{idRank:int}")]
    public async Task<IActionResult> PostNewRank([FromRoute] int idRank)
    {
        var rank = await _ranksDbRepository.DeleteRank(idRank);
        if (rank is null)
        {
            return NotFound("Cannot delete rank");
        }
        return Ok(rank);
    }
    
    [HttpPut("{idRank:int}")]
    public async Task<IActionResult> PostNewRank([FromBody] PutRankDto putRankDto, [FromRoute] int idRank)
    {
        if (idRank != putRankDto.idRank)
            return BadRequest();
        var rank = await _ranksDbRepository.ModifyRank(putRankDto, idRank);
        if (rank is null)
        {
            return NotFound("Cannot modify rank");
        }
        return Ok(rank);
    }
    
}