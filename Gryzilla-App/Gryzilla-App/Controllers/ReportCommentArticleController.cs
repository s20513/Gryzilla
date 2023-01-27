using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
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
    public async Task<IActionResult> GetReports()
    {
        var reports = await _reportCommentArticleDbRepository.GetReportCommentArticlesToDb();
        
        if (reports.IsNullOrEmpty())
        {
            return NotFound("No reports");
        }
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
    public async Task<IActionResult> GetReport([FromRoute] int idUser, [FromRoute] int idComment, [FromRoute] int idReason)
    {
        var report = await _reportCommentArticleDbRepository.GetOneReportCommentArticleToDb(idUser, idComment, idReason);

        if (report is null)
        {
            return NotFound("No report with given id found");
        }
        
        return Ok(report);
    }
    
    /// <summary>
    /// Add Report Article Comment
    /// </summary>
    /// <param name="newReportCommentArticle">NewReportCommentArticleDto</param>
    /// <returns>NewReportCommentArticleDto</returns>
    [HttpPost]
    public async Task<IActionResult> AddReportArticleComment([FromBody] NewReportCommentArticleDto newReportCommentArticle)
    {
        ReportCommentArticleDto? report;
        try
        {
            report = await _reportCommentArticleDbRepository.AddReportCommentArticleToDb(newReportCommentArticle);

            if (report is null)
            {
                return NotFound("User, comment or reason is wrong");
            } 
        }
        catch (UserCreatorException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(report);
    }
    
    /// <summary>
    /// Delete Report Article Comment
    /// </summary>
    /// <param name="reportCommentArticle">DeleteReportArticleComment</param>
    /// <returns>DeleteReportArticleComment</returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteReportArticleComment([FromBody] DefaultReportCommentArticleDto reportCommentArticle)
    {
        var report = await _reportCommentArticleDbRepository.DeleteReportCommentArticleToDb(reportCommentArticle);

        if (report is null)
        {
            return NotFound("No report with given id found");
        }

        return Ok(report);
    }
    
    /// <summary>
    /// Update Report CommentArticle Dto
    /// </summary>
    /// <param name="reportCommentArticle">UpdateReportCommentArticleDto</param>
    /// <returns>UpdateReportCommentArticleDto</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateReportPostComment([FromBody] UpdateReportCommentArticleDto reportCommentArticle)
    {
        var report = await _reportCommentArticleDbRepository.UpdateReportCommentArticleToDb(reportCommentArticle);

        if (report is null)
        {
            return NotFound("No report with given id found");
        }

        return Ok(report);
    }

}