using Gryzilla_App.DTOs.Requests.ReportUser;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ReportUserDbRepository : IReportUserDbRepository
{
    private readonly GryzillaContext _context;

    public ReportUserDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<ReportUserDto?> AddReportUserToDb(NewReportUserDto newReportUserDto)
    {
        var userReported = await _context
            .UserData
            .SingleOrDefaultAsync(e => e.IdUser == newReportUserDto.IdUserReported);

        var userReporting = await _context
            .UserData
            .SingleOrDefaultAsync(e => e.IdUser == newReportUserDto.IdUser);

        var reason = await _context
            .Reasons
            .AnyAsync(e => e.IdReason == newReportUserDto.IdReason);

        if (userReported is null || userReporting is null || !reason)
        {
            return null;
        }

        if (userReporting.IdUser == userReported.IdUser)
        {
            throw new UserCreatorException("You can't report yourself");
        }

        var reportUser = new ReportUser
        {
            IdReason        = newReportUserDto.IdReason,
            IdUserReported  = newReportUserDto.IdUserReported,
            IdUserReporting = newReportUserDto.IdUser,
            Description     = newReportUserDto.Content,
            Viewed          = false,
            ReportedAt      = DateTime.Now
        };

        await _context.ReportUsers.AddAsync(reportUser);
        await _context.SaveChangesAsync();
        
        return new ReportUserDto
        {
            IdReason        = newReportUserDto.IdReason,
            NickReported     =  _context.UserData.
                Where(x=>x.IdUser == newReportUserDto.IdUserReported).Select(x=>x.Nick).SingleOrDefault(),
            IdUserReported  = newReportUserDto.IdUserReported,
            IdUser = newReportUserDto.IdUser,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== newReportUserDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content     = newReportUserDto.Content,
            Viewed          = false,
            ReportedAt      = DateTime.Now
        };
    }

    public async Task<ReportUserDto?> UpdateReportUserFromDb(ModifyReportUser modifyReportUser)
    {
        var reportUser = await _context
            .ReportUsers
            .SingleOrDefaultAsync(e => 
                e.IdReport == modifyReportUser.IdReport);
        
        if (reportUser is null)
        {
            return null;
        }

        reportUser.Description = modifyReportUser.Content;
        reportUser.Viewed = modifyReportUser.Viewed;

        await _context.SaveChangesAsync();
        
        return new ReportUserDto
        {
            idReport        = modifyReportUser.IdReport,
            IdReason        = modifyReportUser.IdReason,
            IdUserReported  = reportUser.IdUserReported,
            IdUser = reportUser.IdUserReporting,
            NickReported     =  _context.UserData.
                Where(x=>x.IdUser == reportUser.IdUserReported).Select(x=>x.Nick).SingleOrDefault(),
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== modifyReportUser.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content     = reportUser.Description,
            Viewed          = reportUser.Viewed,
            ReportedAt      = reportUser.ReportedAt
        };
    }

    public async Task<IEnumerable<ReportUserDto?>> GetUsersReportsFromDb()
    {
        var reportUsers = await _context
            .ReportUsers
            .Select(e => new ReportUserDto
            {
                idReport        = e.IdReport,
                IdReason        = e.IdReason,
                IdUserReported  = e.IdUserReported,
                IdUser          = e.IdUserReporting,
                NickReported     =  _context.UserData.
                    Where(x=>x.IdUser == e.IdUserReported).Select(x=>x.Nick).SingleOrDefault(),
                ReasonName =  _context.Reasons
                    .Where(x=>x.IdReason== e.IdReason)
                    .Select(x=>x.ReasonName)
                    .SingleOrDefault(),
                Content     = e.Description,
                Viewed          = e.Viewed,
                ReportedAt      = e.ReportedAt
            }).ToListAsync();

        return reportUsers;
    }

    public async Task<ReportUserDto?> GetUserReportFromDb(int idReport)
    {
        var report = await _context
            .ReportUsers
            .Where(x => x.IdReport == idReport)
            .SingleOrDefaultAsync();

        if (report is null)
        {
            return null;
        }

        return new ReportUserDto
        {
            idReport        = idReport,
            IdReason        = report.IdReason,
            IdUserReported  = report.IdUserReported,
            IdUser = report.IdUserReporting,
            NickReported     =  _context.UserData.
                Where(x=>x.IdUser ==report.IdUserReported).Select(x=>x.Nick).SingleOrDefault(),
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== report.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content     = report.Description,
            Viewed          = report.Viewed
        };
    }

    public async Task<ReportUserDto?> DeleteReportUserFromDb(int idReport)
    {
        var report = await _context
            .ReportUsers
            .Where(x => x.IdReport == idReport)
            .SingleOrDefaultAsync();

        if (report is null)
        {
            return null;
        }

        _context.ReportUsers.Remove(report);
        await _context.SaveChangesAsync();
        
        return new ReportUserDto
        {
            idReport        = idReport,
            IdReason        = report.IdReason,
            IdUserReported  = report.IdUserReported,
            IdUser = report.IdUserReporting,
            NickReported     =  _context.UserData.
                Where(x=>x.IdUser ==report.IdUserReported).Select(x=>x.Nick).SingleOrDefault(),
            ReasonName =  _context.Reasons
                            .Where(x=>x.IdReason== report.IdReason)
                            .Select(x=>x.ReasonName)
                            .SingleOrDefault(),
            Content     = report.Description,
            Viewed          = report.Viewed
        };
    }
}