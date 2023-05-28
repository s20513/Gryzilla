using System.Security.Claims;
using Gryzilla_App.DTOs.Responses.LikesArticle;
using Gryzilla_App.DTOs.Responses.LikesPost;
using Gryzilla_App.Helpers;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class LikesPostDbRepository : ILikesPostDbRepository
{
    private readonly GryzillaContext _context;

    public LikesPostDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    private async Task<Post?> GetPost(int idPost)
    {
        var post = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .Include(x=>x.IdUsers)
            .SingleOrDefaultAsync();

        return post;
    }
    private async Task<UserDatum?> GetUser(int idUser)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();
        
        return user;
    }

    private async Task<UserDatum?> GetPostLike(int idPost, int idUser)
    {
        var postLike = await _context
            .Posts
            .Where(x => x.IdPost   == idPost)
            .SelectMany(x => x.IdUsers)
            .Where(x=>x.IdUser == idUser)
            .SingleOrDefaultAsync();
        
        return postLike;
    }
    
    public async Task<string?> AddLikeToPost(int idUser, int idPost)
    {
        var user = await GetUser(idUser);
        var post = await GetPost(idPost);
        
        if (post is null || user is null)
        {
            return "Post or user doesn't exist";
        }
        
        var postLike = await GetPostLike(idPost, idUser);

        if (postLike is not null)
        {
            return "Like has been assigned";
        }
        
        post.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        return "Added like";
    }
    
    public async Task<string?> DeleteLikeFromPost(int idUser, int idPost, ClaimsPrincipal userClaims)
    {
        var user = await GetUser(idUser);
        var post = await GetPost(idPost);
        
        if (post is null || user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, idUser))
        {
            return "Post or user doesn't exist";
        }
        
        var postLike = await GetPostLike(idPost, idUser);
        
        if (postLike is null)
        {
            return "Like has not been assigned";
        }

        post.IdUsers.Remove(user);
        await _context.SaveChangesAsync();
        
        return "Deleted like";
    }

    public async Task<LikesPostDto?> ExistLike(int idUser, int idPost)
    {
        var user = await GetUser(idUser);
        var post = await GetPost(idPost);
        
        if (post is null || user is null)
        {
            return null;
        }
        
        var postLike = await GetPostLike(idPost, idUser);
        
        return new LikesPostDto
        {
            liked = postLike is not null ? true : false
        };
    }
}