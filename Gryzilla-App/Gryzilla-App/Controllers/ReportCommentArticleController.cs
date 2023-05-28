using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/reportCommentArticle")]

public class ReportCommentArticleController: Controller
{
    private readonly IReportCommentArticleDbRepository _reportCommentArticleDbRepository;

    public ReportCommentArticleController(IReportCommentArticleDbRepository reportCommentArticleDbRepository)
    {
        _reportCommentArticleDbRepository = reportCommentArticleDbRepository;
    }

    /// <summary>
    ///  Get reports 
    /// </summary>
    /// <returns> List of reports</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportCommentArticleDbRepository.GetReportCommentArticlesFromDb();
        
        return Ok(reports);
    }

    /// <summary>
    /// Get one report
    /// </summary>
    /// <param name="idUser">User identifier</param>
    /// <param name="idComment">Comment identifier</param>
    /// <param name="idReason">Reason identifier</param>
    /// <returns>Get one report</returns>
    [HttpGet("one/{idUser:int}/{idComment:int}/{idReason:int}")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetReport([FromRoute] int idUser, [FromRoute] int idComment, [FromRoute] int idReason)
    {
        var report = await _reportCommentArticleDbRepository.GetOneReportCommentArticleFromDb(idUser, idComment, idReason);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Add Report Article Comment
    /// </summary>
    /// <param name="newReportCommentArticle">NewReportCommentArticleDto</param>
    /// <returns>NewReportCommentArticleDto</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddReportArticleComment([FromBody] NewReportCommentArticleDto newReportCommentArticle)
    {
        ReportCommentArticleDto? report;
        try
        {
            report = await _reportCommentArticleDbRepository.AddReportCommentArticleToDb(newReportCommentArticle);

            if (report is null)
            {
                return NotFound(new StringMessageDto{ Message = "User, comment or reason is wrong"});
            } 
        }
        catch (UserCreatorException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        return Ok(report);
    }
    
    /// <summary>
    /// Delete Report Article Comment
    /// </summary>
    /// <param name="reportCommentArticle">DeleteReportArticleComment</param>
    /// <returns>DeleteReportArticleComment</returns>
    [HttpDelete]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> DeleteReportArticleComment([FromBody] DeleteReportCommentArticleDto reportCommentArticle)
    {
        var report = await _reportCommentArticleDbRepository.DeleteReportCommentArticleFromDb(reportCommentArticle);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }

        return Ok(report);
    }
    
    /// <summary>
    /// Update Report CommentArticle Dto
    /// </summary>
    /// <param name="reportCommentArticle">UpdateReportCommentArticleDto</param>
    /// <returns>UpdateReportCommentArticleDto</returns>
    [HttpPut]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> UpdateReportPostComment([FromBody] UpdateReportCommentArticleDto reportCommentArticle)
    {
        var report = await _reportCommentArticleDbRepository.UpdateReportCommentArticleFromDb(reportCommentArticle);

        if (report is null)
        {
            return NotFound(new StringMessageDto{ Message = "No report with given id found"});
        }

        return Ok(report);
    }

}