using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class PostDbRepository : IPostDbRepository
{
    private readonly GryzillaContext _context;
    public PostDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<PostDto>?> GetPostsFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        var postDtos = new List<PostDto>();
        
        if (allPosts is null) return null;
        
        foreach (var post in allPosts)
        {
            var newPost = await _context.Posts.Where(x => x.IdPost == post.IdPost)
                .SelectMany(x=> x.IdUsers)
                .Select(a => new PostDto
                {
                    likes = _context.Posts.Where(c=>c.IdPost==post.IdPost).SelectMany(b => b.IdUsers).Count(),
                    CreatedAt = a.CreatedAt,
                    Nick = a.Nick,
                    tags = _context.Posts
                        .Where(x=>x.IdPost==post.IdPost)
                        .SelectMany(x => x.IdTags)
                        .Select(x=> new TagDto
                        { 
                          nameTag= x.NameTag
                        }).ToArray()
                }).SingleOrDefaultAsync();
            if (newPost != null) postDtos.Add(newPost);
        }
        return postDtos;
    }

    public async Task<string?> AddNewPostFromDb(AddPostDto addPostDto)
    {
        //sprawdzam czy user istnieje
        var user = await _context.UserData.Where(x => x.IdUser == addPostDto.idUser).SingleOrDefaultAsync();
        if (user is null) return null;
        
        var post = new Post{
            IdUser = addPostDto.idUser,
            Title = addPostDto.title,
            CreatedAt = DateTime.Now,
            Content = addPostDto.content,
            HighLight = false
        };
        await _context.Posts.AddAsync(post);
        foreach (var tag in addPostDto.tags)
        {
            var newTag = await _context.Tags.Where(x => x.NameTag == tag.nameTag).SingleOrDefaultAsync();
            if (newTag is not null){
                post.IdTags.Add(newTag);
            }
        }
        
        await _context.SaveChangesAsync();
        return "Added new post";
    }

    public async Task<string?> DeletePostFromDb(int idPost)
    {
        var post = await _context.Posts.Where(x => x.IdPost == idPost).SingleOrDefaultAsync();
        if (post is null) return null;
        var tags = await _context.Posts.Include(x => x.IdTags).Where(x => x.IdPost == post.IdPost).Select(x=>new Tag()).ToArrayAsync();
        foreach (var tag in tags)
        {
            post.IdTags.Remove(tag);
        }
        //w toku
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return "deleted post";
    }
}