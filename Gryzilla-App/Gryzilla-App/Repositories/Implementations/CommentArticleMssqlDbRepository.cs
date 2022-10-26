using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class CommentArticleMssqlDbRepository:ICommentArticleDbRepository
{
    private readonly GryzillaContext _context;

    public CommentArticleMssqlDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<ArticleCommentDto?> AddCommentToArticle(NewArticleCommentDto newArticleCommentDto)
    {
        int            id;
        UserDatum?     user;
        Article?       article;
        CommentArticle articleComment;
        
        user = await _context
                .UserData
                .SingleOrDefaultAsync(e => e.IdUser == newArticleCommentDto.IdUser);
        
        if (user is null)
        {
            return null;
        }
        
        article = await _context
                .Articles
                .SingleOrDefaultAsync(e => e.IdArticle == newArticleCommentDto.IdArticle);

        if (article is null)
        {
            return null;
        }

        articleComment = new CommentArticle
        {
            IdUser =             newArticleCommentDto.IdUser,
            IdArticle =          newArticleCommentDto.IdArticle,
            DescriptionArticle = newArticleCommentDto.Description
        };

        await _context.CommentArticles.AddAsync(articleComment);
        await _context.SaveChangesAsync();

        id = _context.CommentArticles.Max(e => e.IdCommentArticle);

        return new ArticleCommentDto
        {
            IdComment =   id,
            Nick =        user.Nick,
            IdUser =      user.IdUser,
            IdArticle =   newArticleCommentDto.IdArticle,
            Description = newArticleCommentDto.Description
        };
    }
    
    public async Task<ArticleCommentDto?> ModifyArticleCommentFromDb(PutArticleCommentDto putArticleCommentDto, int idComment)
    {
        string nick;
        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(e => 
                e.IdCommentArticle == putArticleCommentDto.IdComment && 
                e.IdUser ==           putArticleCommentDto.IdUser &&
                e.IdArticle ==        putArticleCommentDto.IdArticle);

        if (comment is null)
        {
            return null;
        }

        comment.DescriptionArticle = putArticleCommentDto.Description;
        await _context.SaveChangesAsync();
        
        nick = await _context
            .UserData
            .Where(x => x.IdUser == putArticleCommentDto.IdUser)
            .Select(x => x.Nick)
            .SingleAsync();

        return new ArticleCommentDto
        {
            Nick =        nick,
            IdComment =   comment.IdCommentArticle,
            IdUser =      putArticleCommentDto.IdUser,
            IdArticle = putArticleCommentDto.IdArticle,
            Description = putArticleCommentDto.Description
        };
    }

    public async Task<ArticleCommentDto?> DeleteArticleCommentFromDb(int idComment)
    {
        string nick;
        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(e => e.IdCommentArticle == idComment);
        
        if (comment is null)
        {
            return null;
        }

        _context.CommentArticles.Remove(comment);
        await _context.SaveChangesAsync();

        nick = await _context
            .UserData
            .Where(x => x.IdUser == comment.IdUser)
            .Select(x => x.Nick)
            .SingleAsync();
        
        return new ArticleCommentDto
        {
            Nick = nick,
            IdComment =   idComment,
            IdUser =      comment.IdUser,
            IdArticle = comment.IdArticle,
            Description = comment.DescriptionArticle
        };
    }
    
}