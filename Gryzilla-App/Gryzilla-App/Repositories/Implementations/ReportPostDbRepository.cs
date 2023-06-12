using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.DTOs.Responses.ReportPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ReportPostDbRepository: IReportPostDbRepository
{
    private readonly GryzillaContext _context;
    
    public ReportPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<ReportPostResponseDto?> AddReportPostToDb(NewReportPostRequestDto newReportPostRequestDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(e => e.IdUser == newReportPostRequestDto.IdUser);

        var post = await _context
            .Posts
            .SingleOrDefaultAsync(e => e.IdPost == newReportPostRequestDto.IdPost);

        var reason = await _context
            .Reasons
            .AnyAsync(e => e.IdReason == newReportPostRequestDto.IdReason);

        if (user is null || post is null || !reason)
        {
            return null;
        }
        
        var reporttPostExists = await _context.ReportPosts
            .AnyAsync(e => e.IdReason == newReportPostRequestDto.IdReason
                           && e.IdUser == newReportPostRequestDto.IdUser
                           && e.IdPost == newReportPostRequestDto.IdPost);

        if (post.IdUser == user.IdUser || reporttPostExists)
        {
            throw new UserCreatorException("You can't report your own post");
        }

        var newReportPost = new ReportPost
        {
            IdUser = newReportPostRequestDto.IdUser,
            IdPost = newReportPostRequestDto.IdPost,
            IdReason = newReportPostRequestDto.IdReason,
            Description = newReportPostRequestDto.Content,
            Viewed = false,
            ReportedAt = DateTime.Now
        };

        await _context.ReportPosts.AddAsync(newReportPost);
        await _context.SaveChangesAsync();
        
        return new ReportPostResponseDto
        {
            IdUser = newReportPostRequestDto.IdUser,
            IdUserReported = _context.Posts
                .Where(x=>x.IdPost ==  newReportPostRequestDto.IdPost)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.Posts
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdPost ==  newReportPostRequestDto.IdPost)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdPost = newReportPostRequestDto.IdPost,
            IdReason = newReportPostRequestDto.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== newReportPostRequestDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = newReportPostRequestDto.Content,
            Viewed = false,
            ReportedAt = newReportPost.ReportedAt
        };
    }

    public async Task<ReportPostResponseDto?> DeleteReportPostFromDb(ReportPostIdsRequestDto reportPostIdsRequestDto)
    {
        var reportPost = await _context
            .ReportPosts
            .SingleOrDefaultAsync(e => 
                e.IdUser == reportPostIdsRequestDto.IdUser
            && e.IdPost == reportPostIdsRequestDto.IdPost
            && e.IdReason == reportPostIdsRequestDto.IdReason);
        
        if (reportPost is null)
        {
            return null;
        }

        _context.ReportPosts.Remove(reportPost);
        await _context.SaveChangesAsync();

        return new ReportPostResponseDto
        {
            IdUser = reportPost.IdUser,
            IdUserReported = _context.Posts
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.Posts
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdPost = reportPost.IdPost,
            IdReason = reportPost.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportPost.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportPost.Description,
            Viewed = reportPost.Viewed,
            ReportedAt = reportPost.ReportedAt
        };
    }

    public async Task<ReportPostResponseDto?> UpdateReportPostFromDb(UpdateReportPostRequestDto updateReportPostRequestDto)
    {
        var reportPost = await _context
            .ReportPosts
            .SingleOrDefaultAsync(e => 
                e.IdUser == updateReportPostRequestDto.IdUser
                && e.IdPost == updateReportPostRequestDto.IdPost
                && e.IdReason == updateReportPostRequestDto.IdReason);
        
        if (reportPost is null)
        {
            return null;
        }

        reportPost.Description = updateReportPostRequestDto.Content;
        reportPost.Viewed = updateReportPostRequestDto.Viewed;
        await _context.SaveChangesAsync();

        return new ReportPostResponseDto
        {
            IdUser = reportPost.IdUser,
            IdUserReported = _context.Posts
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.Posts
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdPost = reportPost.IdPost,
            IdReason = reportPost.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportPost.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportPost.Description,
            Viewed = reportPost.Viewed,
            ReportedAt = reportPost.ReportedAt
        };
    }

    public async Task<ReportPostResponseDto?> GetReportPostFromDb(ReportPostIdsRequestDto updateReportPostRequestDto)
    {
        var reportPost = await _context
            .ReportPosts
            .SingleOrDefaultAsync(e => 
                e.IdUser == updateReportPostRequestDto.IdUser
                && e.IdPost == updateReportPostRequestDto.IdPost
                && e.IdReason == updateReportPostRequestDto.IdReason);
        
        if (reportPost is null)
        {
            return null;
        }
        
        return new ReportPostResponseDto
        {
            IdUser = reportPost.IdUser,
            IdUserReported = _context.Posts
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.Posts
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdPost ==  reportPost.IdPost)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdPost = reportPost.IdPost,
            IdReason = reportPost.IdReason,
            ReasonName =  _context.Reasons
                .Where(x=>x.IdReason== reportPost.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportPost.Description,
            Viewed = reportPost.Viewed,
            ReportedAt = reportPost.ReportedAt
        };
    }

    public async Task<IEnumerable<ReportPostResponseDto?>> GetReportPostsFromDb()
    {
        var reportPosts = await _context
            .ReportPosts
            .Select(e => new ReportPostResponseDto
            {
                IdUser = e.IdUser,
                IdUserReported = _context.Posts
                    .Where(x=>x.IdPost ==  e.IdPost)
                    .Select(x=>x.IdUser).SingleOrDefault(),
                NickReported  = _context.Posts
                    .Include(x=>x.IdUserNavigation)
                    .Where(x=>x.IdPost ==  e.IdPost)
                    .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
                IdPost = e.IdPost,
                IdReason = e.IdReason,
                ReasonName =  _context.Reasons
                    .Where(x=>x.IdReason== e.IdReason)
                    .Select(x=>x.ReasonName)
                    .SingleOrDefault(),
                Content = e.Description,
                Viewed = e.Viewed,
                ReportedAt = e.ReportedAt
            }).ToListAsync();

        return reportPosts.OrderByDescending(x=>x.ReportedAt);
    }
}