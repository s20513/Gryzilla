using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.DTOs.Responses.ReportCommentPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ReportCommentPostDbRepository:IReportCommentPostDbRepository
{
    private readonly GryzillaContext _context;

    public ReportCommentPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Add report comment post to db
    /// </summary>
    /// <param name="newReportCommentDto">NewReportCommentPostDto</param>
    /// <returns>NewReportCommentPostDto</returns>
    public async Task<ReportCommentPostDto?> AddReportCommentPostToDb(NewReportCommentPostDto newReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == newReportCommentDto.IdUser);

        var comment = await _context
            .CommentPosts
            .SingleOrDefaultAsync(x => x.IdComment == newReportCommentDto.IdComment);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == newReportCommentDto.IdReason);
        

        if (reason is null || comment is null || user is null)
        {
            return null;
        }
        
        if (comment.IdUser == user.IdUser)
        {
            throw new UserCreatorException("The creator of the comment cannot report");
        }

        var newReportCommentPost = new ReportCommentPost
        {
            IdUser      = newReportCommentDto.IdUser,
            IdComment   = newReportCommentDto.IdComment,
            IdReason    = newReportCommentDto.IdReason,
            Description = newReportCommentDto.Content,
            Viewed      = false,
            ReportedAt  = DateTime.Now
        };

        await _context.ReportCommentPosts.AddAsync(newReportCommentPost);
        await _context.SaveChangesAsync();

        return new ReportCommentPostDto
        {
            IdUser      = newReportCommentDto.IdUser,
            IdComment   = newReportCommentDto.IdComment,
            IdReason    = newReportCommentDto.IdReason,
            IdPost      =  _context.CommentPosts
                .Where(x => x.IdComment == newReportCommentDto.IdComment)
                .Select(x => x.IdPost)
                .SingleOrDefault(),
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason ==  newReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = newReportCommentDto.Content,
            Viewed      = false,
            ReportedAt  = DateTime.Now
        };
    }

    /// <summary>
    /// Delete report comment post
    /// </summary>
    /// <param name="deleteReportCommentDto">DeleteReportCommentPostDto</param>
    /// <returns>DeleteReportCommentPostDto</returns>
    public async Task<ReportCommentPostDto?> DeleteReportCommentPostFromDb(DefaultReportCommentPostDto deleteReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == deleteReportCommentDto.IdUser);

        var comment = await _context
            .CommentPosts
            .SingleOrDefaultAsync(x => x.IdComment == deleteReportCommentDto.IdComment);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == deleteReportCommentDto.IdReason);

        if (reason is null || comment is null || user is null)
        {
            return null;
        }

        var reportCommentDto = await _context
            .ReportCommentPosts.Where(x => x.IdUser == deleteReportCommentDto.IdUser)
            .Where(x => x.IdComment == deleteReportCommentDto.IdComment)
            .Where(x => x.IdReason == deleteReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }
        
        _context.ReportCommentPosts.Remove(reportCommentDto);
        await _context.SaveChangesAsync();
        
        return new ReportCommentPostDto
        {
            IdUser      = deleteReportCommentDto.IdUser,
            IdComment   = deleteReportCommentDto.IdComment,
            IdReason    = deleteReportCommentDto.IdReason,
            IdPost      =  _context.CommentPosts
                .Where(x => x.IdComment == reportCommentDto.IdComment)
                .Select(x => x.IdPost)
                .SingleOrDefault(),
            ReasonName  = _context.Reasons
                .Where(x=>x.IdReason ==  deleteReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportCommentDto.Description,
            Viewed      = reportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    /// <summary>
    /// Update report comment post 
    /// </summary>
    /// <param name="updateReportCommentDto">UpdateReportCommentPostDto</param>
    /// <returns>UpdateReportCommentPostDto</returns>
    public async Task<ReportCommentPostDto?> UpdateReportCommentPostFromDb(UpdateReportCommentPostDto updateReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == updateReportCommentDto.IdUser);

        var comment = await _context
            .CommentPosts
            .SingleOrDefaultAsync(x => x.IdComment == updateReportCommentDto.IdComment);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == updateReportCommentDto.IdReason);

        if (reason is null || comment is null || user is null)
        {
            return null;
        }

        var reportCommentDto = await _context
            .ReportCommentPosts.Where(x => x.IdUser == updateReportCommentDto.IdUser)
            .Where(x => x.IdComment == updateReportCommentDto.IdComment)
            .Where(x => x.IdReason == updateReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }

        reportCommentDto.Description = updateReportCommentDto.Content;
        reportCommentDto.Viewed = updateReportCommentDto.Viewed;
        await _context.SaveChangesAsync();
        
        return new ReportCommentPostDto
        {
            IdUser      = updateReportCommentDto.IdUser,
            IdComment   = updateReportCommentDto.IdComment,
            IdReason    = updateReportCommentDto.IdReason,
            IdPost   =  _context.CommentPosts
                .Where(x => x.IdComment == reportCommentDto.IdComment)
                .Select(x => x.IdPost)
                .SingleOrDefault(),
            ReasonName  = _context.Reasons
                .Where(x=>x.IdReason ==  updateReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = updateReportCommentDto.Content,
            Viewed      = updateReportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    /// <summary>
    ///  Get One Report Comment Post To Db
    /// </summary>
    /// <param name="idUser">idUser</param>
    /// <param name="idComment">idComment</param>
    /// <param name="idReason">idReason</param>
    /// <returns></returns>
    public async Task<ReportCommentPostDto?> GetOneReportCommentPostFromDb(int idUser, int idComment, int idReason)
    {
        var reportCommentDto = await _context
            .ReportCommentPosts.Where(x => x.IdUser == idUser)
            .Where(x => x.IdComment == idComment)
            .Where(x => x.IdReason == idReason)
            .SingleOrDefaultAsync();
        
        if (reportCommentDto is null)
        {
            return null;
        }
        
        return new ReportCommentPostDto
        {
            IdUser      = reportCommentDto.IdUser,
            IdComment   = reportCommentDto.IdComment,
            IdReason    = reportCommentDto.IdReason,
            IdPost   =  _context.CommentPosts
                    .Where(x => x.IdComment == reportCommentDto.IdComment)
                    .Select(x => x.IdPost)
                    .SingleOrDefault(),
            ReasonName = _context.Reasons
                .Where(x=>x.IdReason ==  idReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportCommentDto.Description,
            Viewed      = reportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    /// <summary>
    ///  Get report comment posts list
    /// </summary>
    /// <returns>List - ReportCommentPostDto</returns>
    public async Task<IEnumerable<ReportCommentPostDto>> GetReportCommentPostsFromDb()
    {
        var reportCommentPost = await _context.ReportCommentPosts
            .Select(e => new ReportCommentPostDto
            {
                IdUser      = e.IdUser,
                IdComment   = e.IdComment,
                IdReason    = e.IdReason,
                IdPost      =  _context.CommentPosts
                    .Where(x => x.IdComment == e.IdComment)
                    .Select(x => x.IdPost)
                    .SingleOrDefault(),
                ReasonName  = _context.Reasons
                    .Where(x=>x.IdReason ==  e.IdReason)
                    .Select(x=>x.ReasonName)
                    .SingleOrDefault(),
                Content = e.Description,
                Viewed      = e.Viewed,
                ReportedAt  = e.ReportedAt
            }).ToListAsync();

        return reportCommentPost;
    }
    
}