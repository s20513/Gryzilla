using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.DTOs.Responses.ReportCommentArticle;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class ReportCommentArticleDbRepository:IReportCommentArticleDbRepository
{
    private readonly GryzillaContext _context;

    public ReportCommentArticleDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<ReportCommentArticleDto?> AddReportCommentArticleToDb(NewReportCommentArticleDto newReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == newReportCommentDto.IdUser);

        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(x => x.IdCommentArticle == newReportCommentDto.IdComment);

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

        var newReportCommentPost = new ReportCommentArticle
        {
            IdUser             = newReportCommentDto.IdUser,
            IdCommentArticle   = newReportCommentDto.IdComment,
            IdReason           = newReportCommentDto.IdReason,
            Description        = newReportCommentDto.Content,
            Viewed             = false,
            ReportedAt         = DateTime.Now
        };

        await _context.ReportCommentArticles.AddAsync(newReportCommentPost);
        await _context.SaveChangesAsync();

        return new ReportCommentArticleDto
        {
            IdUser        = newReportCommentDto.IdUser,
            IdUserReported = _context.CommentArticles
                .Where(x=>x.IdCommentArticle == newReportCommentDto.IdComment)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.CommentArticles
            .Include(x=>x.IdUserNavigation)
            .Where(x=>x.IdCommentArticle == newReportCommentDto.IdComment)
            .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdArticle   =  _context.CommentArticles
                .Where(x => x.IdCommentArticle == newReportCommentDto.IdComment)
                .Select(x => x.IdArticle)
                .SingleOrDefault(),
            IdComment   = newReportCommentDto.IdComment,
            IdReason    = newReportCommentDto.IdReason,
            ReasonName  = _context.Reasons
                .Where(x=>x.IdReason == newReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content     = newReportCommentDto.Content,
            Viewed      = false,
            ReportedAt  = DateTime.Now
        };
    }

    public async Task<ReportCommentArticleDto?> DeleteReportCommentArticleFromDb(DeleteReportCommentArticleDto deleteReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == deleteReportCommentDto.IdUser);

        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(x => x.IdCommentArticle == deleteReportCommentDto.IdComment);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == deleteReportCommentDto.IdReason);
        

        if (reason is null || comment is null || user is null)
        {
            return null;
        }
        
        var reportCommentArticle = await _context
            .ReportCommentArticles.Where(x => x.IdUser == deleteReportCommentDto.IdUser)
            .Where(x => x.IdCommentArticle == deleteReportCommentDto.IdComment)
            .Where(x => x.IdReason == deleteReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentArticle is null)
        {
            return null;
        }
        
        _context.ReportCommentArticles.Remove(reportCommentArticle);
        await _context.SaveChangesAsync();
        
        var newReportCommentPost = new ReportCommentArticleDto
        {
            IdUser             = deleteReportCommentDto.IdUser,
            IdUserReported = _context.CommentArticles
                .Where(x=>x.IdCommentArticle == deleteReportCommentDto.IdComment)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.CommentArticles
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdCommentArticle == deleteReportCommentDto.IdComment)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdArticle          =  _context.CommentArticles
                .Where(x => x.IdCommentArticle == deleteReportCommentDto.IdComment)
                .Select(x => x.IdArticle)
                .SingleOrDefault(),
            IdComment          = deleteReportCommentDto.IdComment,
            IdReason           = deleteReportCommentDto.IdReason,
            ReasonName = _context.Reasons
                .Where(x=>x.IdReason ==  deleteReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content        = reportCommentArticle.Description,
            Viewed             = reportCommentArticle.Viewed,
            ReportedAt         = reportCommentArticle.ReportedAt
        };

        return newReportCommentPost;
    }

    public async Task<ReportCommentArticleDto?> UpdateReportCommentArticleFromDb(UpdateReportCommentArticleDto updateReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == updateReportCommentDto.IdUser);

        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(x => x.IdCommentArticle == updateReportCommentDto.IdComment);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == updateReportCommentDto.IdReason);
        

        if (reason is null || comment is null || user is null)
        {
            return null;
        }
        
        var reportCommentDto = await _context
            .ReportCommentArticles.Where(x => x.IdUser == updateReportCommentDto.IdUser)
            .Where(x => x.IdCommentArticle == updateReportCommentDto.IdComment)
            .Where(x => x.IdReason == updateReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }

        reportCommentDto.Description = updateReportCommentDto.Content;
        reportCommentDto.Viewed = updateReportCommentDto.Viewed;
        await _context.SaveChangesAsync();
        
        return new ReportCommentArticleDto
        {
            IdUser      = updateReportCommentDto.IdUser,
            IdUserReported = _context.CommentArticles
                .Where(x=>x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.CommentArticles
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdArticle   =  _context.CommentArticles
                .Where(x => x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x => x.IdArticle)
                .SingleOrDefault(),
            IdComment   = updateReportCommentDto.IdComment,
            IdReason    = updateReportCommentDto.IdReason,
            ReasonName = _context.Reasons
                .Where(x=>x.IdReason ==  updateReportCommentDto.IdReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = updateReportCommentDto.Content,
            Viewed      = updateReportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    public async Task<ReportCommentArticleDto?> GetOneReportCommentArticleFromDb(int idUser, int idComment, int idReason)
    {
        var reportCommentDto = await _context
            .ReportCommentArticles.Where(x => x.IdUser == idUser)
            .Where(x => x.IdCommentArticle == idComment)
            .Where(x => x.IdReason == idReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }

        var article = await _context.CommentArticles
            .Where(x => x.IdCommentArticle == idComment)
            .Select(x => x.IdArticle)
            .SingleOrDefaultAsync();
        
        return new ReportCommentArticleDto
        {
            IdUser      = reportCommentDto.IdUser,
            IdUserReported = _context.CommentArticles
                .Where(x=>x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x=>x.IdUser).SingleOrDefault(),
            NickReported  = _context.CommentArticles
                .Include(x=>x.IdUserNavigation)
                .Where(x=>x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
            IdArticle   =  _context.CommentArticles
                .Where(x => x.IdCommentArticle == reportCommentDto.IdCommentArticle)
                .Select(x => x.IdArticle)
                .SingleOrDefault(),
            IdComment   = reportCommentDto.IdCommentArticle,
            IdReason    = reportCommentDto.IdReason,
            ReasonName = _context.Reasons
                .Where(x=>x.IdReason ==  idReason)
                .Select(x=>x.ReasonName)
                .SingleOrDefault(),
            Content = reportCommentDto.Description,
            Viewed      = reportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    public async Task<IEnumerable<ReportCommentArticleDto>> GetReportCommentArticlesFromDb()
    {
        var reportCommentArticle = await _context.ReportCommentArticles
            .Select(e => new ReportCommentArticleDto
            {
                IdUser      = e.IdUser,
                IdUserReported = _context.CommentArticles
                    .Where(x=>x.IdCommentArticle ==  e.IdCommentArticle)
                    .Select(x=>x.IdUser).SingleOrDefault(),
                NickReported  = _context.CommentArticles
                    .Include(x=>x.IdUserNavigation)
                    .Where(x=>x.IdCommentArticle ==  e.IdCommentArticle)
                    .Select(x=>x.IdUserNavigation.Nick).SingleOrDefault(),
                IdComment   = e.IdCommentArticle,
                IdArticle   =  _context.CommentArticles
                    .Where(x => x.IdCommentArticle == e.IdCommentArticle)
                    .Select(x => x.IdArticle)
                    .SingleOrDefault(),
                IdReason    = e.IdReason,
                ReasonName  = _context.Reasons
                    .Where(x=>x.IdReason ==  e.IdReason)
                    .Select(x=>x.ReasonName)
                    .SingleOrDefault(),
                Content = e.Description,
                Viewed      = e.Viewed,
                ReportedAt  = e.ReportedAt
            }).ToListAsync();

        return reportCommentArticle;
    }
}