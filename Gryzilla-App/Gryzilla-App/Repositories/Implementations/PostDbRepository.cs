using System.Xml;
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

    public async Task<IEnumerable<PostDto>?> GetTableSort(Post[] allPosts)
    {
        var postDtos = new List<PostDto>();
        foreach (var post in allPosts)
        {
            var newPost = await _context.Posts.Where(x => x.IdPost == post.IdPost)
                .Include(x => x.IdUserNavigation)
                .Select(a => new PostDto
                {
                    Likes = _context.Posts.Where(c => c.IdPost == post.IdPost).SelectMany(b => b.IdUsers).Count(),
                    CreatedAt = a.CreatedAt,
                    Content = a.Content,
                    Title = a.Title,
                    Nick = a.IdUserNavigation.Nick,
                    Tags = _context.Posts.Where(x => x.IdPost == post.IdPost).SelectMany(x => x.IdTags)
                        .Select(x => new TagDto {nameTag = x.NameTag}).ToArray()
                }).SingleOrDefaultAsync();

            if (newPost != null) postDtos.Add(newPost);
        }

        return postDtos;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        if (allPosts is null) return null;
        var postDtos = await GetTableSort(allPosts);
        return postDtos;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByLikesFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        if (allPosts is null) return null;
        var postDtos = await GetTableSort(allPosts);
        if (postDtos != null)
        {
            postDtos = postDtos.OrderByDescending(order => order.Likes).ToList();
            return postDtos;
        }

        return null;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByLikesLeastFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        if (allPosts is null) return null;
        var postDtos = await GetTableSort(allPosts);
        if (postDtos != null)
        {
            postDtos = postDtos.OrderBy(order => order.Likes).ToList();
            return postDtos;
        }

        return null;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByDateFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        if (allPosts is null) return null;
        var postDtos = await GetTableSort(allPosts);
        if (postDtos != null)
        {
            postDtos = postDtos.OrderByDescending(order => order.CreatedAt).ToList();
            return postDtos;
        }

        return null;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByDateOldestFromDb()
    {
        var allPosts = await _context.Posts.ToArrayAsync();
        if (allPosts is null) return null;
        var postDtos = await GetTableSort(allPosts);
        if (postDtos != null)
        {
            postDtos = postDtos.OrderBy(order => order.CreatedAt).ToList();
            return postDtos;
        }

        return null;
    }

    public async Task<string?> AddNewPostFromDb(AddPostDto addPostDto)
    {
        //sprawdzam czy user istnieje
        var user = await _context.UserData.Where(x => x.IdUser == addPostDto.idUser).SingleOrDefaultAsync();
        if (user is null) return null;

        var post = new Post
        {
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
            if (newTag is not null)
            {
                post.IdTags.Add(newTag);
            }
        }

        await _context.SaveChangesAsync();
        return "Added new post";
    }

    public async Task<string?> DeletePostFromDb(int idPost)
    {
        var post = await _context.Posts.Where(x => x.IdPost == idPost).Include(x => x.IdTags).Include(x => x.IdUsers)
            .SingleOrDefaultAsync();
        if (post is null) return null;
        var tags = await _context.Posts.Where(x => x.IdPost == idPost).SelectMany(x => x.IdTags).ToArrayAsync();
        foreach (var tag in tags)
        {
            post.IdTags.Remove(tag);
        }

        var likes = await _context.Posts.Where(c => c.IdPost == idPost).SelectMany(x => x.IdUsers).ToArrayAsync();
        foreach (var like in likes)
        {
            post.IdUsers.Remove(like);
        }

        var comments = await _context.CommentPosts.Where(x => x.IdPost == idPost).ToArrayAsync();
        foreach (var comment in comments)
        {
            _context.CommentPosts.Remove(comment);
        }

        var reports = await _context.ReportPosts.Where(x => x.IdPost == idPost).ToArrayAsync();

        foreach (var report in reports)
        {
            _context.ReportPosts.Remove(report);
        }

        await _context.SaveChangesAsync();
        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();
        return "deleted post";
    }

    public async Task<string?> DeleteTagFromPost(int idPost, int idTag)
    {
        var post = await _context.Posts.Where(x => x.IdPost == idPost).Include(x => x.IdTags).SingleOrDefaultAsync();
        if (post is null) return null;
        var tagFromPost = await _context.Posts.Where(x => x.IdPost == idPost).SelectMany(x => x.IdTags)
            .Where(x => x.IdTag == idTag).SingleOrDefaultAsync();
        if (tagFromPost is null) return null;
        post.IdTags.Remove(tagFromPost);
        await _context.SaveChangesAsync();
        return "removed tag from post";
    }

    public async Task<string?> ModifyPostFromDb(PutPostDto putPostDto)
    {
        var post = await _context.Posts.Where(x => x.IdPost == putPostDto.idPost).Include(x => x.IdTags)
            .SingleOrDefaultAsync();
        if (post is null) return null;
        post.Title = putPostDto.title;
        post.Content = putPostDto.content;
        if (putPostDto.tags.Length > 0)
        {
            foreach (var tag in post.IdTags)
            {
                post.IdTags.Remove(tag);
            }
            foreach (var tag in putPostDto.tags)
            {
                var newTag = await _context.Tags.Where(x => x.IdTag == tag.idTag).SingleOrDefaultAsync();
                if(newTag is not null)
                    post.IdTags.Add(newTag);
            }

            await _context.SaveChangesAsync();
            return "modified post";
        }
        else
        {
            await _context.SaveChangesAsync();
            return null;
        }
    }
}


