using Gryzilla_App.DTOs.Requests.ReportProfileComment;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.ReportProfileComment;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;
[ApiController]
[Route("api/reportProfileComment")]
public class ReportProfileCommentController: Controller
{
    private readonly IReportProfileCommentDbRepository _reportProfileCommentDbRepository;

    public ReportProfileCommentController(IReportProfileCommentDbRepository reportProfileCommentDbRepository)
    {
        _reportProfileCommentDbRepository= reportProfileCommentDbRepository;
    }
    
    
    /// <summary>
    /// Get all reports 
    /// </summary>
    /// <returns> List of reports</returns>
    [HttpGet("all")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportProfileCommentDbRepository.GetReportProfileCommentsFromDb();
        
        return Ok(reports);
    }
    
    /// <summary>
    /// Get one specific report
    /// </summary>
    /// <param name="reportProfileCommentIdsRequestDto">ReportProfileCommentIdsRequestDto</param>
    /// <returns>ReportProfileCommentResponseDto</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReport([FromQuery] ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto)
    {
        var report = await _reportProfileCommentDbRepository.GetReportProfileCommentFromDb(reportProfileCommentIdsRequestDto);
        
        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Add Report Post
    /// </summary>
    /// <param name="newReportProfileCommentRequestDto">NewReportProfileCommentRequestDto</param>
    /// <returns>ReportProfileCommentResponseDto</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddReport([FromBody] NewReportProfileCommentRequestDto newReportProfileCommentRequestDto)
    {
        ReportProfileCommentResponseDto? report;
        try
        {
            report = await _reportProfileCommentDbRepository.AddReportProfileCommentToDb(newReportProfileCommentRequestDto);

            if (report is null)
            {
                return NotFound(new StringMessageDto{ Message = "User, profile comment or reason does not exist"});
            }

        }
        catch (UserCreatorException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
       
        return Ok(report);
    }
    
    /// <summary>
    /// Update specific Report Post
    /// </summary>
    /// <param name="updateReportProfileCommentRequestDto">UpdateReportProfileCommentRequestDto</param>
    /// <returns>ReportProfileCommentResponseDto</returns>
    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> UpdateReport([FromBody] UpdateReportProfileCommentRequestDto updateReportProfileCommentRequestDto)
    {
        var report = await _reportProfileCommentDbRepository.UpdateReportProfileCommentFromDb(updateReportProfileCommentRequestDto);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Deletes specific Report Post
    /// </summary>
    /// <param name="reportProfileCommentIdsRequestDto">ReportProfileCommentIdsRequestDto</param>
    /// <returns>ReportProfileCommentResponseDto</returns>
    [HttpDelete]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> DeleteReport([FromBody] ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto)
    {
        var report = await _reportProfileCommentDbRepository.DeleteReportProfileCommentFromDb(reportProfileCommentIdsRequestDto);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given data found"});
        }
        
        return Ok(report);
    }
}