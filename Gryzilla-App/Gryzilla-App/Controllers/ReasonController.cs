using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Reason;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;
[ApiController]
[Route("api/reason")]

public class ReasonController: Controller
{
    private readonly IReasonDbRepository _reasonDbRepository;

    public ReasonController(IReasonDbRepository reasonDbRepository)
    {
        _reasonDbRepository = reasonDbRepository;
    }

    /// <summary>
    /// Find all Reasons from db
    /// </summary>
    /// <returns>Return Status Ok - returns list of Reasons</returns>
    [HttpGet]
    public async Task<IActionResult> GetReasons()
    {
        var reasons = await _reasonDbRepository.GetReasonsFromDb();
        
        return Ok(reasons);
    }

    /// <summary>
    /// Find Reason with given id
    /// </summary>
    /// <param name="idReason">Int - Reason id</param>
    /// <returns>Return Status Ok - returns Reason with given id</returns>
    /// <returns>Return Status NotFound - returns message that there is no Reason with given id</returns>
    [HttpGet("{idReason:int}")]
    public async Task<IActionResult> GetReason(int idReason)
    {
        var reason = await _reasonDbRepository.GetReasonFromDb(idReason);

        if (reason is null)
        {
            return NotFound(new StringMessageDto{ Message ="No reason with given id found"});
        }
        
        return Ok(reason);
    }
    
    /// <summary>
    /// Adds new Reason to database
    /// </summary>
    /// <param name="newReasonDto">NewReasonDto - data for creating new Reason in db</param>
    /// <returns>Return Status Ok - returns list of Reasons</returns>
    /// <returns>Return Status BadRequest - returns message that Reason with given name already exists</returns>
    [HttpPost]
    public async Task<IActionResult> AddReason([FromBody] NewReasonDto newReasonDto)
    {
        FullTagDto reason;
        
        try
        {
            reason = await _reasonDbRepository.AddReasonToDb(newReasonDto);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        
        return Ok(reason);
    }
    
    /// <summary>
    /// Deletes Reason with given id
    /// </summary>
    /// <param name="idReason">Int - Reason id</param>
    /// <returns>Return Status Ok - returns deleted Reason</returns>
    /// <returns>Return Status NotFound - returns message that there is no Reason with given id</returns>
    [HttpDelete("{idReason:int}")]
    public async Task<IActionResult> DeleteReason(int idReason)
    {
        var reason = await _reasonDbRepository.DeleteReasonFromDb(idReason);

        if (reason is null)
        {
            return NotFound(new StringMessageDto{ Message = "No reason with given id found"});
        }

        return Ok(reason);
    }
}