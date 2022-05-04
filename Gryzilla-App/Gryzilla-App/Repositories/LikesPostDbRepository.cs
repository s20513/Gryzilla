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
    public async Task<string?> AddLikeToPost(int idUser, int idPost)
    {
        var post = await _context.Posts.Where(x => x.IdPost == idPost).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == idUser).SingleOrDefaultAsync();
        if (post is null || user is null) return null;
        var postLike = await _context.Posts.Where(x => x.IdPost == idPost).SelectMany(x => x.IdUsers)
            .Where(x=>x.IdUser==idUser).SingleOrDefaultAsync();
        if (postLike is not null) return null;
        post.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        return "added new like";
    }
    public async Task<string?> DeleteLikeToPost(int idUser, int idPost)
    {
        var post = await _context.Posts.Where(x => x.IdPost == idPost).Include(x=>x.IdUsers).SingleOrDefaultAsync();
        var user = await _context.UserData.Where(x => x.IdUser == idUser).SingleOrDefaultAsync();
        if (post is null || user is null) return null;
        var postLike = await _context.Posts.Where(x => x.IdPost == idPost).SelectMany(x => x.IdUsers)
            .Where(x=>x.IdUser==idUser).SingleOrDefaultAsync();
        if (postLike is null) return null;
        post.IdUsers.Remove(user);
        await _context.SaveChangesAsync();
        return "deleted like";
    }
}