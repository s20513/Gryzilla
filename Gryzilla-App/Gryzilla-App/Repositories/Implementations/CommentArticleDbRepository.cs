using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.DTOs.Responses.ArticleComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class CommentArticleDbRepository:ICommentArticleDbRepository
{
    private readonly GryzillaContext _context;

    public CommentArticleDbRepository(GryzillaContext context)
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
            IdUser             = newArticleCommentDto.IdUser,
            IdArticle          = newArticleCommentDto.IdArticle,
            DescriptionArticle = newArticleCommentDto.Content,
            CreatedAt          = DateTime.Now
        };

        await _context.CommentArticles.AddAsync(articleComment);
        await _context.SaveChangesAsync();

        id = _context.CommentArticles.Max(e => e.IdCommentArticle);

        return new ArticleCommentDto
        {
            IdComment   = id,
            Nick        = user.Nick,
            IdUser      = user.IdUser,
            IdArticle   = newArticleCommentDto.IdArticle,
            Content     = newArticleCommentDto.Content,
            CreatedAt   = articleComment.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }
    
    public async Task<ArticleCommentDto?> ModifyArticleCommentFromDb(PutArticleCommentDto putArticleCommentDto, int idComment)
    {
        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(e => 
                e.IdCommentArticle == putArticleCommentDto.IdComment &&
                e.IdArticle        == putArticleCommentDto.IdArticle);

        if (comment is null)
        {
            return null;
        }

        comment.DescriptionArticle = putArticleCommentDto.Content;
        await _context.SaveChangesAsync();
        
        var user = await _context
            .UserData
            .Where(x => x.IdUser == comment.IdUser)
            .SingleAsync();

        return new ArticleCommentDto
        {
            Nick        = user.Nick,
            IdComment   = comment.IdCommentArticle,
            IdUser      = comment.IdUser,
            IdArticle   = putArticleCommentDto.IdArticle,
            Content     = putArticleCommentDto.Content,
            CreatedAt    = comment.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }

    public async Task<ArticleCommentDto?> DeleteArticleCommentFromDb(int idComment)
    {
        var comment = await _context
            .CommentArticles
            .SingleOrDefaultAsync(e => e.IdCommentArticle == idComment);
        
        if (comment is null)
        {
            return null;
        }

        _context.CommentArticles.Remove(comment);
        await _context.SaveChangesAsync();

        var user = await _context
            .UserData
            .Where(x => x.IdUser == comment.IdUser)
            .SingleAsync();
        
        return new ArticleCommentDto
        {
            Nick        = user.Nick,
            IdComment   = idComment,
            IdUser      = comment.IdUser,
            IdArticle   = comment.IdArticle,
            Content     = comment.DescriptionArticle,
            CreatedAt = comment.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }
    public async Task<GetArticleCommentDto> GetArticleCommentsFromDb(int idArticle)
    {
        var article = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == idArticle);

        if (article is null)
        {
            return null;
        }

        var comments = await _context
            .CommentArticles
            .Where(x => x.IdArticle == idArticle).
            Select(x=> new ArticleCommentDto
            {
                IdComment = x.IdCommentArticle,
                IdUser  = x.IdUser,
                IdArticle = idArticle,
                Content = x.DescriptionArticle,
                Nick    = _context
                    .UserData
                    .Where(u=>u.IdUser == x.IdUser)
                    .Select(u=>u.Nick)
                    .SingleOrDefault(),
                CreatedAt = x.CreatedAt,
                base64PhotoData = Convert.ToBase64String(x.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                Type = x.IdUserNavigation.PhotoType,
            }).ToArrayAsync();

        return new GetArticleCommentDto
        {
            Comments = comments
        };
    }
}