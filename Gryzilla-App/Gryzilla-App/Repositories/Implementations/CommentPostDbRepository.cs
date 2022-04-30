using System.Xml;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class CommentPostDbRepository : ICommentPostDbRepository
{
    private readonly GryzillaContext _context;

    public CommentPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    public async Task<string?> PostNewCommentToDb(AddCommentDto addCommentDto)
    {
        var post = await _context.Posts.Where(x => x.IdPost == addCommentDto.IdPost).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == addCommentDto.IdUser).SingleOrDefaultAsync();
        if (user is null || post is null) return null;
        var newPost = await _context.CommentPosts.AddAsync(new CommentPost
        {
            IdUser = addCommentDto.IdUser,
            IdPost = addCommentDto.IdPost,
            DescriptionPost = addCommentDto.DescriptionPost
        });
        await _context.SaveChangesAsync();
        return "added new comment";
    }

    public async Task<string?> ModifyCommentToDb(PutCommentDto modifyCommentDto)
    {
        var commentPost = await _context.CommentPosts.Where(x => x.IdComment == modifyCommentDto.IdComment).SingleOrDefaultAsync();
        if (commentPost is null ) return null;
        commentPost.DescriptionPost = modifyCommentDto.DescriptionPost;
        await _context.SaveChangesAsync();
        return "modified comment";
    }

    public async Task<string?> DeleteCommentFromDb(int idComment)
    {
        var commentPost = await _context.CommentPosts.Where(x => x.IdComment == idComment).SingleOrDefaultAsync();
        if (commentPost is null) return null;
        _context.CommentPosts.Remove(commentPost);
        await _context.SaveChangesAsync();
        return "removed comment";
    }
}