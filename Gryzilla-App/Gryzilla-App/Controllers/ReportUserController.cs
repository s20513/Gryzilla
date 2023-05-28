using Gryzilla_App.DTOs.Requests.ReportUser;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/reportUser")]
public class ReportUserController: Controller
{
    private readonly IReportUserDbRepository _reportUserDbRepository;

    public ReportUserController(IReportUserDbRepository reportUserDbRepository)
    {
        _reportUserDbRepository = reportUserDbRepository;
    }
    
    
    [HttpGet("all")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportUserDbRepository.GetUsersReportsFromDb();
        
        return Ok(reports);
    }
    
    [HttpGet("{idReport:int}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReport([FromRoute] int idReport)
    {
        var report = await _reportUserDbRepository.GetUserReportFromDb(idReport);
        
        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
    
  
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddReport(NewReportUserDto reportUserDto)
    {
        ReportUserDto? report;
        try
        {
            report = await _reportUserDbRepository.AddReportUserToDb(reportUserDto);

            if (report is null)
            {
                return NotFound(new StringMessageDto{ Message = "Users or reason does not exist"});
            }

        }
        catch (UserCreatorException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
       
        return Ok(report);
    }
    
   
    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> UpdateReport(ModifyReportUser reportUser)
    {
        var report = await _reportUserDbRepository.UpdateReportUserFromDb(reportUser);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
    
    
    [HttpDelete("{idReport:int}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> DeleteReport([FromRoute] int idReport)
    {
        var report = await _reportUserDbRepository.DeleteReportUserFromDb(idReport);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
}