using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Helpers;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class PostDbRepository : IPostDbRepository
{
    private readonly GryzillaContext _context;

    public PostDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    private async Task<Tag> AddNewTag(string tagName)
    {
        var newTag = new Tag
        {
            NameTag = tagName
        };

        _context.Tags.Add(newTag);
        await _context.SaveChangesAsync();
        
        return newTag;
    }
    
    private async Task<IEnumerable<PostDto>?> GetTableSort(Post[] allPosts)
    {
        var postDto = new List<PostDto>();
        
        foreach (var post in allPosts)
        {
            var newPost = await _context
                .Posts
                .Where(x => x.IdPost == post.IdPost)
                .Include(x => x.IdUserNavigation)
                .Select(a => new PostDto
                {
                    idPost = post.IdPost,
                    Likes  = _context
                            .Posts
                            .Where(c => c.IdPost == post.IdPost)
                            .SelectMany(b => b.IdUsers)
                            .Count(),
                    CommentsNumber      = _context
                                    .Posts
                                    .Where(c => c.IdPost == post.IdPost)
                                    .SelectMany(b => b.CommentPosts)
                                    .Count(),
                    CommentsDtos  = GetCommentPost(post.IdPost),
                    CreatedAt     = a.CreatedAt,
                    Content       = a.Content,
                    Nick          = a.IdUserNavigation.Nick,
                    Tags          = _context
                        .Posts
                        .Where(x => x.IdPost == post.IdPost)
                        .SelectMany(x => x.IdTags)
                        .Select(x => x.NameTag)
                        .ToArray()
                }).SingleOrDefaultAsync();

            if (newPost != null)
                postDto.Add(newPost);
        }

        return postDto;
    }

    private List<PostCommentDto> GetCommentPost(int idPost)
    {
        var postComment =  _context
            .CommentPosts
            .Where(x => x.IdPost == idPost)
            .Include(c => c.IdUserNavigation)
            .Select(x => new PostCommentDto
            {
                Content = x.DescriptionPost,
                IdComment = x.IdComment,
                IdPost = x.IdPost,
                IdUser = x.IdUser,
                Nick  =x.IdUserNavigation.Nick,
                CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(x.CreatedAt)
            })
            .ToList();
        List <PostCommentDto> list = postComment.Take(2).ToList();
        return list;
    }
    public async Task<IEnumerable<PostDto>?> GetPostsFromDb()
    {
        var allPosts = await _context
            .Posts
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }
        
        var postDto = await GetTableSort(allPosts);
        
        return postDto;
    }

    public async Task<PostQtyDto?> GetQtyPostsFromDb(int qtyPosts)
    {
        bool next = false;
        
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allPosts = await _context
            .Posts
            .OrderBy(x => x.IdPost)
            .Skip(qtyPosts-5)
            .Take(5)
            .ToArrayAsync();
        
        if (allPosts.Length == 0)
        {
            return null;
        }

        var nextPost = await _context
            .Posts
            .CountAsync();

        if (qtyPosts < nextPost)
        {
            next = true;
        }
        
        var postDto = await GetTableSort(allPosts);
        
        return new PostQtyDto()
        {
            Posts = postDto,
            IsNext = next
        };
    }
    public async Task<IEnumerable<PostDto>?> GetTopPosts()
    {
        var allPosts = await _context
            .Posts
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);

        postDtos = postDtos
            .OrderByDescending(order => order.Likes)
            .Skip(0)
            .Take(3)
            .ToList();

        return postDtos;
    }
    
    public async Task<PostQtyDto?> GetQtyPostsByLikesFromDb(int qtyPosts, DateTime time)
    {
        var next = false;
        
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allPosts = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);

        postDtos = postDtos
            .OrderByDescending(order => order.Likes)
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();
        
        var nextPost = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyPosts < nextPost)
        {
            next = true;
        }
        
        return new PostQtyDto()
        {
            Posts = postDtos,
            IsNext = next
        };
    }

    public async Task<PostQtyDto?> GetQtyPostsByCommentsFromDb(int qtyPosts, DateTime time)
    {
        var next = false;
        
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var allPosts = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);

        postDtos = postDtos
            .OrderByDescending(order => order.CommentsNumber)
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();
        
        var nextPost = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyPosts < nextPost)
        {
            next = true;
        }
        
        return new PostQtyDto()
        {
            Posts = postDtos,
            IsNext = next
        };
    }

    public async Task<PostQtyDto?> GetQtyPostsByDateFromDb(int qtyPosts, DateTime time)
    {
        var next = false;
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        var allPosts = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .OrderByDescending(e => e.CreatedAt)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);

        postDtos = postDtos
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();
        
        var nextPost = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyPosts < nextPost)
        {
            next = true;
        }
        
        return new PostQtyDto()
        {
            Posts = postDtos,
            IsNext = next
        };
    }

    public async Task<PostQtyDto?> GetQtyPostsByDateOldestFromDb(int qtyPosts, DateTime time)
    {
        bool next = false;
        if (qtyPosts < 5)
        {
            throw new WrongNumberException("Wrong Number! Please insert number greater than 4");
        }
        
        var allPosts = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .OrderBy(e => e.CreatedAt)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);
        postDtos = postDtos
            .Skip(qtyPosts - 5)
            .Take(5)
            .ToList();
        
        var nextPost = await _context
            .Posts
            .Where(x=>x.CreatedAt < time)
            .CountAsync();

        if (qtyPosts < nextPost)
        {
            next = true;
        }
        
        return new PostQtyDto()
        {
            Posts = postDtos,
            IsNext = next
        };
        
    }
    public async Task<IEnumerable<PostDto>?> GetPostsByLikesFromDb()
    {
        var allPosts = await _context
            .Posts
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }

        var postDtos = await GetTableSort(allPosts);

        postDtos = postDtos.OrderByDescending(order => order.Likes).ToList();
        return postDtos;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByCommentsFromDb()
    {
        var allPosts = await _context
            .Posts
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }
       
        var postDto = await GetTableSort(allPosts);
        
        postDto = postDto.OrderByDescending(order => order.CommentsNumber).ToList();
        return postDto;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByDateFromDb()
    {
        var allPosts = await _context
            .Posts
            .OrderByDescending(e => e.CreatedAt)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }
        
        var postDto = await GetTableSort(allPosts);
        
        return postDto;
    }

    public async Task<IEnumerable<PostDto>?> GetPostsByDateOldestFromDb()
    {
        var allPosts = await _context
            .Posts
            .OrderBy(e => e.CreatedAt)
            .ToArrayAsync();

        if (allPosts.Length == 0)
        {
            return null;
        }
        
        var postDto = await GetTableSort(allPosts);
        
        return postDto;
    }

    public async Task<NewPostDto?> AddNewPostToDb(AddPostDto addPostDto)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == addPostDto.IdUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var post = new Post
        {
            IdUser    = addPostDto.IdUser,
            CreatedAt = DateTime.Now,
            Content   = addPostDto.Content,
            HighLight = false
        };
        
        await _context.Posts.AddAsync(post);
        
        foreach (var tag in addPostDto.Tags)
        {
            var newTag = await _context
                .Tags
                .Where(x => x.NameTag == tag)
                .SingleOrDefaultAsync();
            
            if (newTag is not null)
            {
                post.IdTags.Add(newTag);
            }
            else
            {
                post.IdTags.Add(await AddNewTag(tag));
            }
        }

        await _context.SaveChangesAsync();
        
        var idNewPost = await _context
            .Posts
            .Select(x => x.IdPost)
            .OrderByDescending(x => x)
            .FirstAsync();
        
        return new NewPostDto()
        {
            IdPost    = idNewPost,
            Nick      = user.Nick,
            CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(post.CreatedAt),
            Content   = post.Content,
            IdUser    = post.IdUser,
            Tags      = await _context
                .Posts
                .Where(x => x.IdPost == idNewPost)
                .SelectMany(x => x.IdTags)
                .Select(x => x.NameTag).ToArrayAsync(),
            Comments = 0,
            Likes    = 0
            
        };
    }

    public async Task<DeletePostDto?> DeletePostFromDb(int idPost)
    {
        var post = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .Include(x => x.IdTags)
            .Include(x => x.IdUsers)
            .SingleOrDefaultAsync();

        if (post is null)
        {
            return null;
        }
        
        var tags = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .SelectMany(x => x.IdTags)
            .ToArrayAsync();
        
        foreach (var tag in tags)
        {
            post.IdTags.Remove(tag);
        }

        var likes = await _context
            .Posts
            .Where(c => c.IdPost == idPost)
            .SelectMany(x => x.IdUsers)
            .ToArrayAsync();
        
        foreach (var like in likes)
        {
            post.IdUsers.Remove(like);
        }

        var comments = await _context
            .CommentPosts
            .Where(x => x.IdPost == idPost)
            .ToArrayAsync();
        
        foreach (var comment in comments)
        {
            _context.CommentPosts.Remove(comment);
        }

        var reports = await _context
            .ReportPosts
            .Where(x => x.IdPost == idPost)
            .ToArrayAsync();

        foreach (var report in reports)
        {
            _context.ReportPosts.Remove(report);
            await _context.SaveChangesAsync();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        
        return new DeletePostDto
        {
            DeletedAt = DateTimeConverter.GetDateTimeToStringWithFormat(DateTime.Now),
            Content   = post.Content,
            IdPost    = post.IdPost,
            IdUser    = post.IdUser,
        };
    }

    public async Task<DeleteTagDto?> DeleteTagFromPost(int idPost, int idTag)
    {
        var post = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .Include(x => x.IdTags)
            .SingleOrDefaultAsync();

        if (post is null)
        {
            return null;
        }
        
        var tagFromPost = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .SelectMany(x => x.IdTags)
            .Where(x => x.IdTag == idTag)
            .SingleOrDefaultAsync();

        if (tagFromPost is null)
        {
            return null;
        }
        
        post.IdTags.Remove(tagFromPost);
        await _context.SaveChangesAsync();
        
        return new DeleteTagDto
        {
            IdTag   = idTag,
            NameTag = tagFromPost.NameTag
        };
    }
    

    public async Task<ModifyPostDto?> ModifyPostFromDb(PutPostDto putPostDto, int idPost)
    {
        var post = await _context
            .Posts
            .Where(x => x.IdPost == putPostDto.IdPost)
            .Include(x => x.IdTags)
            .SingleOrDefaultAsync();

        if (post is null)
        {
            return null;
        }
        
        post.Content = putPostDto.Content;
        
        if (putPostDto.Tags.Length > 0)
        {
            foreach (var tag in post.IdTags)
            {
                post.IdTags.Remove(tag);
            }
            
            foreach (var tag in putPostDto.Tags)
            {
                var currentTag = await _context
                    .Tags
                    .Where(x => x.NameTag == tag)
                    .SingleOrDefaultAsync();

                if (currentTag is not null)
                {
                    post.IdTags.Add(currentTag);
                }
                else
                {
                    post.IdTags.Add(await AddNewTag(tag));
                }
            }
            
            await _context.SaveChangesAsync();
            
            return new ModifyPostDto
            {
                CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(post.CreatedAt),
                Content   = post.Content,
                IdPost    = post.IdPost,
                IdUser    = post.IdUser,
                Tags      = await _context
                    .Posts
                    .Where(x=>x.IdPost==idPost)
                    .SelectMany(x=>x.IdTags)
                    .Select(x=>x.NameTag).ToArrayAsync()
            };
        }

        await _context.SaveChangesAsync();
        return new ModifyPostDto
        {
            CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(post.CreatedAt),
            Content   = post.Content,
            IdPost    = post.IdPost,
            IdUser    = post.IdUser,
            Tags = Array.Empty<string>()
        };
    }

    public async Task<OnePostDto?> GetOnePostFromDb(int idPost)
    {
        var post = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .SingleOrDefaultAsync();

        if (post is null)
        {
            return null;
        }
        
        var newPost = await _context
            .Posts
            .Where(x => x.IdPost == idPost)
            .Include(x => x.IdUserNavigation)
            .Select(a => new OnePostDto
            {
                idPost = post.IdPost,
                Likes = _context
                    .Posts
                    .Where(c => c.IdPost == post.IdPost)
                    .SelectMany(b => b.IdUsers)
                    .Count(),
                CommentsNumber = _context
                    .Posts
                    .Where(c => c.IdPost == post.IdPost)
                    .SelectMany(b => b.CommentPosts)
                    .Count(),
                Comments = _context
                    .CommentPosts
                    .Where(c => c.IdPost == post.IdPost)
                    .Include(b=>b.IdUserNavigation)
                    .Select(x=> new PostCommentDto
                    {
                        IdPost      = idPost,
                        IdComment   = x.IdComment,
                        IdUser      = x.IdUser,
                        Nick        = x.IdUserNavigation.Nick,
                        Content = x.DescriptionPost
                    }).ToArray(),
                
                CreatedAt = DateTimeConverter.GetDateTimeToStringWithFormat(a.CreatedAt),
                Content   = a.Content,
                Nick      = a.IdUserNavigation.Nick,
                Tags      = _context
                    .Posts
                    .Where(x => x.IdPost == post.IdPost)
                    .SelectMany(x => x.IdTags)
                    .Select(x => x.NameTag)
                    .ToArray()
                
            }).SingleOrDefaultAsync();
        return newPost;
    }

    public async Task<IEnumerable<PostDto>?> GetUserPostsFromDb(int idUser)
    {
        var userPosts = await _context
            .Posts
            .Where(e => e.IdUser == idUser)
            .ToArrayAsync();

        if (userPosts.Length == 0)
        {
            return null;
        }
        
        var postDtos = await GetTableSort(userPosts);
        
        return postDtos;
    }
}


