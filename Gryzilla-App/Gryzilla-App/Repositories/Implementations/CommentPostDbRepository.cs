using System.Xml;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Posts;
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
    public async Task<CommentDto?> PostNewCommentToDb(AddCommentDto addCommentDto)
    {
        var post = await _context.Posts.Where(x => x.IdPost == addCommentDto.IdPost).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == addCommentDto.IdUser).SingleOrDefaultAsync();
        if (user is null || post is null) return null;
        var newComment = new CommentPost
        {
            IdUser = addCommentDto.IdUser,
            IdPost = addCommentDto.IdPost,
            DescriptionPost = addCommentDto.DescriptionPost
        };
        await _context.CommentPosts.AddAsync(newComment);
        await _context.SaveChangesAsync();
        return new CommentDto
        {
            Nick = user.Nick,
            idComment = await _context.CommentPosts.Select(x => x.IdComment).OrderByDescending(x => x).FirstAsync(),
            Description = newComment.DescriptionPost
        };
    }

    public async Task<ModifyCommentDto?> ModifyCommentToDb(PutCommentDto modifyCommentDto, int idComment)
    {
        var commentPost = await _context.CommentPosts.Where(x => x.IdComment == idComment).SingleOrDefaultAsync();
        if (commentPost is null ) return null;
        commentPost.DescriptionPost = modifyCommentDto.DescriptionPost;
        await _context.SaveChangesAsync();
        return new ModifyCommentDto
        {
            idComment = idComment,
            DescriptionPost = modifyCommentDto.DescriptionPost,
        };
    }

    public async Task<DeleteCommentDto?> DeleteCommentFromDb(int idComment)
    {
        var commentPost = await _context.CommentPosts.Where(x => x.IdComment == idComment).SingleOrDefaultAsync();
        if (commentPost is null) return null;
        _context.CommentPosts.Remove(commentPost);
        await _context.SaveChangesAsync();
        return new DeleteCommentDto
        {
            idComment = idComment,
            DescriptionPost = commentPost.DescriptionPost
        };
    }
}