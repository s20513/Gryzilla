using Gryzilla_App.DTOs.Requests.ReportProfileComment;
using Gryzilla_App.DTOs.Responses.ReportProfileComment;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Gryzilla_App.Repositories.Implementations;

public class ReportProfileCommentDbRepository: IReportProfileCommentDbRepository
{
    private readonly GryzillaContext _context;
    
    public ReportProfileCommentDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<ReportProfileCommentResponseDto?> AddReportProfileCommentToDb(NewReportProfileCommentRequestDto newReportProfileCommentRequestDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(e => e.IdUser == newReportProfileCommentRequestDto.IdUser);

        var profileComment = await _context
            .ProfileComments
            .SingleOrDefaultAsync(e => e.IdProfileComment == newReportProfileCommentRequestDto.IdProfileComment);

        var reason = await _context
            .Reasons
            .AnyAsync(e => e.IdReason == newReportProfileCommentRequestDto.IdReason);

        if (user is null || profileComment is null || !reason)
        {
            return null;
        }

        if (profileComment.IdUser == user.IdUser)
        {
            throw new UserCreatorException("You can't report your own comment");
        }

        var newReport = new ReportProfileComment
        {
            IdUser = newReportProfileCommentRequestDto.IdUser,
            IdProfileComment = newReportProfileCommentRequestDto.IdProfileComment,
            IdReason = newReportProfileCommentRequestDto.IdReason,
            Description = newReportProfileCommentRequestDto.Content,
            Viewed = false,
            ReportedAt = DateTime.Now
        };

        await _context.ReportProfileComments.AddAsync(newReport);
        await _context.SaveChangesAsync();

        var profComment = await _context.ProfileComments
            .Include(e => e.IdUserCommentNavigation)
            .Where(x => x.IdProfileComment == newReportProfileCommentRequestDto.IdProfileComment)
            .SingleOrDefaultAsync();
        
        return new ReportProfileCommentResponseDto
        {
            IdProfile = profComment.IdUserComment,
            IdUser = newReportProfileCommentRequestDto.IdUser,
            IdUserReported = profComment.IdUser,
            NickReported  = profComment.IdUserNavigation.Nick,
            IdProfileComment = newReportProfileCommentRequestDto.IdProfileComment,
            IdReason = newReportProfileCommentRequestDto.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== newReportProfileCommentRequestDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = newReportProfileCommentRequestDto.Content,
            Viewed = false,
            ReportedAt = newReport.ReportedAt
        };
    }

    public async Task<ReportProfileCommentResponseDto?> DeleteReportProfileCommentFromDb(ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto)
    {
        var reportProfileComment = await _context
            .ReportProfileComments
            .SingleOrDefaultAsync(e => 
                e.IdUser == reportProfileCommentIdsRequestDto.IdUser
            && e.IdProfileComment == reportProfileCommentIdsRequestDto.IdProfileComment
            && e.IdReason == reportProfileCommentIdsRequestDto.IdReason);
        
        if (reportProfileComment is null)
        {
            return null;
        }

        _context.ReportProfileComments.Remove(reportProfileComment);
        await _context.SaveChangesAsync();
        
        var profComment = await _context.ProfileComments
            .Include(e => e.IdUserCommentNavigation)
            .Where(x => x.IdProfileComment == reportProfileCommentIdsRequestDto.IdProfileComment)
            .SingleOrDefaultAsync();

        return new ReportProfileCommentResponseDto
        {
            IdProfile = profComment.IdUserComment,
            IdUser = reportProfileComment.IdUser,
            IdUserReported = profComment.IdUser,
            NickReported  = profComment.IdUserNavigation.Nick,
            IdProfileComment = reportProfileComment.IdProfileComment,
            IdReason = reportProfileComment.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportProfileComment.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportProfileComment.Description,
            Viewed = reportProfileComment.Viewed,
            ReportedAt = reportProfileComment.ReportedAt
        };
    }

    public async Task<ReportProfileCommentResponseDto?> UpdateReportProfileCommentFromDb(UpdateReportProfileCommentRequestDto updateReportProfileCommentRequestDto)
    {
        var reportProfileComment = await _context
            .ReportProfileComments
            .SingleOrDefaultAsync(e => 
                e.IdUser == updateReportProfileCommentRequestDto.IdUser
                && e.IdProfileComment == updateReportProfileCommentRequestDto.IdProfileComment
                && e.IdReason == updateReportProfileCommentRequestDto.IdReason);
        
        if (reportProfileComment is null)
        {
            return null;
        }

        reportProfileComment.Description = updateReportProfileCommentRequestDto.Content;
        reportProfileComment.Viewed = updateReportProfileCommentRequestDto.Viewed;
        await _context.SaveChangesAsync();
        
        var profComment = await _context.ProfileComments
            .Include(e => e.IdUserCommentNavigation)
            .Where(x => x.IdProfileComment == updateReportProfileCommentRequestDto.IdProfileComment)
            .SingleOrDefaultAsync();

        return new ReportProfileCommentResponseDto
        {
            IdProfile = profComment.IdUserComment,
            IdUser = reportProfileComment.IdUser,
            IdUserReported = profComment.IdUser,
            NickReported  = profComment.IdUserNavigation.Nick,
            IdProfileComment = reportProfileComment.IdProfileComment,
            IdReason = reportProfileComment.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportProfileComment.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportProfileComment.Description,
            Viewed = reportProfileComment.Viewed,
            ReportedAt = reportProfileComment.ReportedAt
        };
    }

    public async Task<ReportProfileCommentResponseDto?> GetReportProfileCommentFromDb(ReportProfileCommentIdsRequestDto reportProfileCommentIdsRequestDto)
    {
        var reportProfileComment = await _context
            .ReportProfileComments
            .SingleOrDefaultAsync(e => 
                e.IdUser == reportProfileCommentIdsRequestDto.IdUser
                && e.IdProfileComment == reportProfileCommentIdsRequestDto.IdProfileComment
                && e.IdReason == reportProfileCommentIdsRequestDto.IdReason);
        
        if (reportProfileComment is null)
        {
            return null;
        }
        
        var profComment = await _context.ProfileComments
            .Include(e => e.IdUserCommentNavigation)
            .Where(x => x.IdProfileComment == reportProfileCommentIdsRequestDto.IdProfileComment)
            .SingleOrDefaultAsync();
        
        return new ReportProfileCommentResponseDto
        {
            IdProfile = profComment.IdUserComment,
            IdUser = reportProfileComment.IdUser,
            IdUserReported = profComment.IdUser,
            NickReported  = profComment.IdUserNavigation.Nick,
            IdProfileComment = reportProfileComment.IdProfileComment,
            IdReason = reportProfileComment.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportProfileComment.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportProfileComment.Description,
            Viewed = reportProfileComment.Viewed,
            ReportedAt = reportProfileComment.ReportedAt
        };
    }

    public async Task<IEnumerable<ReportProfileCommentResponseDto?>> GetReportProfileCommentsFromDb()
    {
        var reportProfileComments = await _context
            .ReportProfileComments
            .Select(e => new ReportProfileCommentResponseDto
            {
                IdProfile = e.IdProfileCommentNavigation.IdUserComment,
                IdUser = e.IdUser,
                IdUserReported = e.IdProfileCommentNavigation.IdUser,
                NickReported  = e.IdProfileCommentNavigation.IdUserNavigation.Nick,
                IdProfileComment= e.IdProfileComment,
                IdReason = e.IdReason,
                ReasonName =  _context.Reasons
                    .Where(x=>x.IdReason== e.IdReason)
                    .Select(x=>x.ReasonName)
                    .SingleOrDefault(),
                Content = e.Description,
                Viewed = e.Viewed,
                ReportedAt = e.ReportedAt
            }).ToListAsync();

        return reportProfileComments.OrderByDescending(x => x.ReportedAt);
    }
}