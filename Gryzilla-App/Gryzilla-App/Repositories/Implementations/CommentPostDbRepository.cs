using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class  CommentPostDbRepository : ICommentPostDbRepository
{
    private readonly GryzillaContext _context;

    public CommentPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    public async Task<PostCommentDto?> AddCommentToPost(NewPostCommentDto newPostCommentDto)
    {
        int          idComment;
        Post?        post;
        UserDatum?   user;
        CommentPost? newCommentPost;

        post = await _context
            .Posts
            .Where(x => x.IdPost == newPostCommentDto.IdPost)
            .SingleOrDefaultAsync();
        
        if (post is null)
        {
            return null;
        }
        
        user = await _context
            .UserData
            .Where(x => x.IdUser == newPostCommentDto.IdUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }
        
        newCommentPost = new CommentPost
        {
            IdUser          = newPostCommentDto.IdUser,
            IdPost          = newPostCommentDto.IdPost,
            DescriptionPost = newPostCommentDto.Content,
            CreatedAt       = DateTime.Now,
        };
        
        await _context.CommentPosts.AddAsync(newCommentPost);
        await _context.SaveChangesAsync();

        idComment = _context.CommentPosts.Max(x => x.IdComment);
        
        return new PostCommentDto
        {
            Nick        = user.Nick,
            IdUser      = user.IdUser,
            IdComment   = idComment,
            IdPost      = newPostCommentDto.IdPost,
            Content     = newCommentPost.DescriptionPost,
            CreatedAt   = newCommentPost.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }

    public async Task<PostCommentDto?> ModifyPostCommentFromDb(PutPostCommentDto putPostCommentDto, int idComment)
    {
        var commentPost = await _context
            .CommentPosts
            .Where(x => 
                x.IdComment == idComment &&
                x.IdPost    == putPostCommentDto.IdPost)
            .SingleOrDefaultAsync();
        
        if (commentPost is null)
        {
            return null;
        }

        commentPost.DescriptionPost = putPostCommentDto.Content;
        await _context.SaveChangesAsync();
        
        var user = await _context
            .UserData
            .Where(x => x.IdUser == commentPost.IdUser)
            .SingleAsync();
        
        return new PostCommentDto
        {
            Nick        = user.Nick,
            IdComment   = idComment,
            IdPost      = putPostCommentDto.IdPost,
            IdUser      = commentPost.IdUser,
            Content     = putPostCommentDto.Content,
            CreatedAt   = commentPost.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }

    public async Task<PostCommentDto?> DeleteCommentFromDb(int idComment)
    {
        var commentPost = await _context
            .CommentPosts
            .Where(x => x.IdComment == idComment)
            .SingleOrDefaultAsync();
        
        if (commentPost is null)
        {
            return null;
        }
        
        _context.CommentPosts.Remove(commentPost);
        await _context.SaveChangesAsync();
        
        var user = await _context
            .UserData
            .Where(x => x.IdUser == commentPost.IdUser)
            .SingleAsync();
        
        return new PostCommentDto
        {
            Nick        = user.Nick,
            IdPost      = commentPost.IdPost,
            IdUser      = commentPost.IdUser,
            IdComment   = idComment,
            Content     = commentPost.DescriptionPost,
            CreatedAt   = commentPost.CreatedAt,
            base64PhotoData = Convert.ToBase64String(user.Photo ?? Array.Empty<byte>()), 
            Type = user.PhotoType,
        };
    }
    
    public async Task<GetPostCommentDto?> GetPostCommentsFromDb(int idPost)
    {
        var posts = await _context.Posts.SingleOrDefaultAsync(x => x.IdPost == idPost);

        if (posts is null)
        {
            return null;
        }
        
        var comments = await _context
            .CommentPosts
            .Where(x => x.IdPost == idPost).
            Select(x=> new PostCommentDto
            {
                IdComment = x.IdComment,
                IdPost = idPost,
                Content = x.DescriptionPost,
                IdUser = x.IdUser,
                Nick    = _context
                    .UserData
                    .Where(u=>u.IdUser == x.IdUser)
                    .Select(u=>u.Nick)
                    .SingleOrDefault(),
                CreatedAt = x.CreatedAt,
                base64PhotoData = Convert.ToBase64String(x.IdUserNavigation.Photo ?? Array.Empty<byte>()), 
                Type = x.IdUserNavigation.PhotoType,
            }).ToArrayAsync();

        return new GetPostCommentDto
        {
            Comments = comments
        };
    }
}