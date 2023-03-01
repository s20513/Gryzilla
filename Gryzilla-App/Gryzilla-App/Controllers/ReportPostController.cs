using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.DTOs.Responses.ReportPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/reportPost")]
public class ReportPostController: Controller
{
    private readonly IReportPostDbRepository _reportPostDbRepository;

    public ReportPostController(IReportPostDbRepository reportPostDbRepository)
    {
        _reportPostDbRepository = reportPostDbRepository;
    }
    
    /// <summary>
    /// Get all reports 
    /// </summary>
    /// <returns> List of reports</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportPostDbRepository.GetReportPostsFromDb();
        
        if (reports.IsNullOrEmpty())
        {
            return NotFound("No reports");
        }
        return Ok(reports);
    }
    
    /// <summary>
    /// Get one specific report
    /// </summary>
    /// <param name="reportPostIdsRequestDto">ReportPostIdsRequestDto</param>
    /// <returns>ReportPostIdsRequestDto</returns>
    [HttpGet]
    public async Task<IActionResult> GetReports([FromBody] ReportPostIdsRequestDto reportPostIdsRequestDto)
    {
        var report = await _reportPostDbRepository.GetReportPostFromDb(reportPostIdsRequestDto);
        
        if (report is null)
        {
            return NotFound("No report with given data found");
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Add Report Post
    /// </summary>
    /// <param name="newReportPostRequestDto">NewReportPostRequestDto</param>
    /// <returns>ReportPostResponseDto</returns>
    [HttpPost]
    public async Task<IActionResult> AddReport([FromBody] NewReportPostRequestDto newReportPostRequestDto)
    {
        ReportPostResponseDto? report;
        try
        {
            report = await _reportPostDbRepository.AddReportPostToDb(newReportPostRequestDto);

            if (report is null)
            {
                NotFound("User, post or reason does not exist");
            }

        }
        catch (UserCreatorException e)
        {
            return BadRequest(e.Message);
        }
       
        return Ok(report);
    }
    
    /// <summary>
    /// Update specific Report Post
    /// </summary>
    /// <param name="updateReportPostRequestDto">UpdateReportPostRequestDto</param>
    /// <returns>ReportPostResponseDto</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateReport([FromBody] UpdateReportPostRequestDto updateReportPostRequestDto)
    {
        var report = await _reportPostDbRepository.UpdateReportPostFromDb(updateReportPostRequestDto);

        if (report is null)
        {
            return NotFound("No report with given data found");
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Deletes specific Report Post
    /// </summary>
    /// <param name="reportPostIdsRequestDto">ReportPostIdsRequestDto</param>
    /// <returns>ReportPostIdsRequestDto</returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteReport([FromBody] ReportPostIdsRequestDto reportPostIdsRequestDto)
    {
        var report = await _reportPostDbRepository.DeleteReportPostFromDb(reportPostIdsRequestDto);

        if (report is null)
        {
            return NotFound("No report with given data found");
        }
        
        return Ok(report);
    }


}