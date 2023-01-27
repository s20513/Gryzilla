﻿using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
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
            .SingleOrDefaultAsync(x => x.IdCommentArticle == newReportCommentDto.IdCommentArticle);

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
            IdCommentArticle   = newReportCommentDto.IdCommentArticle,
            IdReason           = newReportCommentDto.IdReason,
            Description        = newReportCommentDto.Description,
            Viewed             = false,
            ReportedAt         = DateTime.Now
        };

        await _context.ReportCommentArticles.AddAsync(newReportCommentPost);
        await _context.SaveChangesAsync();

        return new ReportCommentArticleDto
        {
            IdUser      = newReportCommentDto.IdUser,
            IdComment   = newReportCommentDto.IdCommentArticle,
            IdReason    = newReportCommentDto.IdReason,
            Description = newReportCommentDto.Description,
            Viewed      = false,
            ReportedAt  = DateTime.Now
        };
    }

    public async Task<ReportCommentArticleDto?> DeleteReportCommentArticleToDb(DefaultReportCommentArticleDto deleteReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == deleteReportCommentDto.IdUser);

        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(x => x.IdCommentArticle == deleteReportCommentDto.IdCommentArticle);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == deleteReportCommentDto.IdReason);
        

        if (reason is null || comment is null || user is null)
        {
            return null;
        }
        
        var reportCommentDto = await _context
            .ReportCommentArticles.Where(x => x.IdUser == deleteReportCommentDto.IdUser)
            .Where(x => x.IdCommentArticle == deleteReportCommentDto.IdCommentArticle)
            .Where(x => x.IdReason == deleteReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }
        
        _context.ReportCommentArticles.Remove(reportCommentDto);
        await _context.SaveChangesAsync();
        
        var newReportCommentPost = new ReportCommentArticleDto
        {
            IdUser             = deleteReportCommentDto.IdUser,
            IdComment          = deleteReportCommentDto.IdCommentArticle,
            IdReason           = deleteReportCommentDto.IdReason,
            Description        = reportCommentDto.Description,
            Viewed             = reportCommentDto.Viewed,
            ReportedAt         = reportCommentDto.ReportedAt
        };

        return newReportCommentPost;
    }

    public async Task<ReportCommentArticleDto?> UpdateReportCommentArticleToDb(UpdateReportCommentArticleDto updateReportCommentDto)
    {
        var user = await _context
            .UserData
            .SingleOrDefaultAsync(x => x.IdUser == updateReportCommentDto.IdUser);

        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(x => x.IdCommentArticle == updateReportCommentDto.IdCommentArticle);

        var reason = await _context
            .Reasons
            .SingleOrDefaultAsync(x => x.IdReason == updateReportCommentDto.IdReason);
        

        if (reason is null || comment is null || user is null)
        {
            return null;
        }
        
        var reportCommentDto = await _context
            .ReportCommentArticles.Where(x => x.IdUser == updateReportCommentDto.IdUser)
            .Where(x => x.IdCommentArticle == updateReportCommentDto.IdCommentArticle)
            .Where(x => x.IdReason == updateReportCommentDto.IdReason)
            .SingleOrDefaultAsync();

        if (reportCommentDto is null)
        {
            return null;
        }

        reportCommentDto.Description = updateReportCommentDto.Description;
        reportCommentDto.Viewed = updateReportCommentDto.Viewed;
        await _context.SaveChangesAsync();
        
        return new ReportCommentArticleDto
        {
            IdUser      = updateReportCommentDto.IdUser,
            IdComment   = updateReportCommentDto.IdCommentArticle,
            IdReason    = updateReportCommentDto.IdReason,
            Description = updateReportCommentDto.Description,
            Viewed      = updateReportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    public async Task<ReportCommentArticleDto?> GetOneReportCommentArticleToDb(int idUser, int idComment, int idReason)
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

        
        return new ReportCommentArticleDto
        {
            IdUser      = reportCommentDto.IdUser,
            IdComment   = reportCommentDto.IdCommentArticle,
            IdReason    = reportCommentDto.IdReason,
            Description = reportCommentDto.Description,
            Viewed      = reportCommentDto.Viewed,
            ReportedAt  = reportCommentDto.ReportedAt
        };
    }

    public async Task<IEnumerable<ReportCommentArticleDto?>> GetReportCommentArticlesToDb()
    {
        var reportCommentArticle = await _context.ReportCommentArticles
            .Select(e => new ReportCommentArticleDto
            {
                IdUser      = e.IdUser,
                IdComment   = e.IdCommentArticle,
                IdReason    = e.IdReason,
                Description = e.Description,
                Viewed      = e.Viewed,
                ReportedAt  = e.ReportedAt
            }).ToListAsync();

        return reportCommentArticle;
    }
}