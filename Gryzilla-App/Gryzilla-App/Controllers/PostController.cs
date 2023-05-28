using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Helpers;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : Controller
{
    private readonly IPostDbRepository _postsDbRepository;
    public PostController(IPostDbRepository postsDbRepository)
    {
        _postsDbRepository = postsDbRepository;
    }
    /// <summary>
    /// Get all posts
    /// </summary>
    /// <returns>
    /// Return post list
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postsDbRepository.GetPostsFromDb();
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        
        return Ok(posts);
    }
    
    /// <summary>
    /// Get user posts
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <returns>
    /// NotFound - user has no posts
    /// Ok - return user posts
    /// </returns>
    [HttpGet("user/{idUser:int}")]
    public async Task<IActionResult> GetUserPost([FromRoute] int idUser)
    {
        var posts = await _postsDbRepository.GetUserPostsFromDb(idUser);
        
        return Ok(posts);
    }
    
    /// <summary>
    /// Get top posts 
    /// </summary>
    /// <returns>
    /// Return post list
    /// </returns>
    [HttpGet("top")]
    public async Task<IActionResult> GetTopPosts()
    {
        IEnumerable<PostDto>? posts = await _postsDbRepository
            .GetTopPosts();

        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        
        return Ok(posts);
    }
    
    /// <summary>
    /// Get posts 
    /// </summary>
    /// <returns>
    /// Return post list
    /// </returns>
    [HttpGet("qty/{qtyPosts:int}")]
    public async Task<IActionResult> GetQtyPosts(int qtyPosts)
    {
        PostQtyDto? posts;
        
        try
        {
           posts = await _postsDbRepository
               .GetQtyPostsFromDb(qtyPosts);

            if (posts is null)
            {
                return NotFound(new StringMessageDto { Message = "No posts found" });
            }
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }

        return Ok(posts);
    }
    /// <summary>
    /// Return posts with the least likes
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("qty/byCommentsDesc/{qtyPosts:int}")]
    public async Task<IActionResult> GetPostsByComments([FromRoute] int qtyPosts, [FromQuery] DateTime time)
    {
        PostQtyDto? posts;
        
        try
        {
            posts = await _postsDbRepository.GetQtyPostsByCommentsFromDb(qtyPosts, time);
            if (posts == null)
            {
                return NotFound(new StringMessageDto { Message = "No posts found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }

        return Ok(posts);
    }
    /// <summary>
    /// Return posts with the most likes
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("qty/byLikesDesc/{qtyPosts:int}")]
    public async Task<IActionResult> GetPostsByLikesMost([FromRoute] int qtyPosts, [FromQuery] DateTime time)
    {
        PostQtyDto? posts;
        try
        {
            posts = await _postsDbRepository.GetQtyPostsByLikesFromDb(qtyPosts, time);
            if (posts == null) 
            {
                return NotFound(new StringMessageDto { Message = "No posts found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
       
        return Ok(posts);
    }
    /// <summary>
    /// Return posts from the latest date
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("qty/byDateDesc/{qtyPosts:int}")]
    public async Task<IActionResult> GetPostsByDates([FromRoute] int qtyPosts, [FromQuery] DateTime time)
    {
        PostQtyDto? posts;
        try
        {
            posts = await _postsDbRepository.GetQtyPostsByDateFromDb(qtyPosts, time);
            if (posts == null)
            {
                return NotFound(new StringMessageDto { Message = "No posts found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }

        return Ok(posts);
    }

    /// <summary>
    /// Return posts from the oldest date
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("qty/byDateAsc/{qtyPosts:int}")]
    public async Task<IActionResult> GetPostsByDatesOldest([FromRoute] int qtyPosts, [FromQuery] DateTime time)
    {
        PostQtyDto? posts;
        try
        {
            posts = await _postsDbRepository.GetQtyPostsByDateOldestFromDb(qtyPosts, time);

            if (posts == null)
            {
                return NotFound(new StringMessageDto { Message = "No posts found" });
            }
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        
        return Ok(posts);
    }
    
    /// <summary>
    /// Return posts with the least likes
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("byCommentsDesc")]
    public async Task<IActionResult> GetPostsByComments()
    {
        var posts = await _postsDbRepository.GetPostsByCommentsFromDb();
        if (posts.IsNullOrEmpty())
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        return Ok(posts);
    }
    /// <summary>
    /// Return posts with the most likes
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("byLikesDesc")]
    public async Task<IActionResult> GetPostsByLikesMost()
    {
        var posts = await _postsDbRepository.GetPostsByLikesFromDb();
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        return Ok(posts);
    }
    /// <summary>
    /// Return posts from the latest date
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("byDateDesc")]
    public async Task<IActionResult> GetPostsByDates()
    {
        var posts = await _postsDbRepository.GetPostsByDateFromDb();
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        return Ok(posts);
    }
    /// <summary>
    /// Return posts from the oldest date
    /// </summary>
    /// <returns>
    /// NotFound - if any post doesn't exist
    /// Ok - return list of posts
    /// </returns>
    [HttpGet("byDateAsc")]
    public async Task<IActionResult> GetPostsByDatesOldest()
    {
        var posts = await _postsDbRepository.GetPostsByDateOldestFromDb();
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "No posts found" });
        }
        return Ok(posts);
    }
    /// <summary>
    /// Get post by Id
    /// </summary>
    /// <param name="idPost">int - Post Identifier</param>
    /// <returns>
    /// NotFound - if Post doesn't exist
    /// Ok - return post
    /// </returns>
    [HttpGet("{idPost:int}")]
    public async Task<IActionResult> GetOnePost([FromRoute] int idPost)
    {
        var posts = await _postsDbRepository.GetOnePostFromDb(idPost);
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "Post with this Id doesn't exist"});
        }
        return Ok(posts);
    }
    /// <summary>
    /// Add new Post
    /// </summary>
    /// <param name="addPostDto">Body to store information about new post</param>
    /// <returns>
    /// NotFound - Cannot add new post
    /// Ok - return body of post if added successfully
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> AddPost([FromBody] AddPostDto addPostDto)
    {
        var posts = await _postsDbRepository.AddNewPostToDb(addPostDto);
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "Cannot add new post" });
        }
        return Ok(posts);
    }
    /// <summary>
    /// Modify Post
    /// </summary>
    /// <param name="putPostDto">Dto to store new information about post</param>
    /// <param name="idPost">int - Post Identifier</param>
    /// <returns>
    /// BadRequest - if Id from route and Id in body have to be same 
    /// NotFound - if post doesn't exist
    /// Ok - Return post if modified successfully
    /// </returns>
    [HttpPut("{idPost:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyPost([FromBody] PutPostDto putPostDto, [FromRoute] int idPost)
    {
        if (putPostDto.IdPost != idPost)
        {
            return BadRequest(new StringMessageDto { Message = "Id from route and Id in body have to be same"}); 
        }
        
        var posts = await _postsDbRepository.ModifyPostFromDb(putPostDto, idPost);
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message = "Cannot modify the post" });
        }
        
        return Ok(posts);
    }
    /// <summary>
    /// Delete post with tags
    /// </summary>
    /// <param name="idPost">int - Post Identifier</param>
    /// <returns>
    /// NotFound - if post doesn't exist or cannot deleted
    /// Ok - return Post - if deleted successfully
    /// </returns>
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    [HttpDelete("{idPost:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int idPost)
    {
        var posts = await _postsDbRepository.DeletePostFromDb(idPost);
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message ="Cannot delete new post"});
        }
        
        return Ok(posts);
    }
    
    /// <summary>
    ///  Delete tag from Post
    /// </summary>
    /// <param name="idPost">int - Post Identifier</param>
    /// <param name="idTag">int - Tag Identifier</param>
    /// <returns>
    /// NotFound - if tag has not been assigned or cannot delete tag from post
    /// OK - return Post
    /// </returns>
    [HttpDelete("tag/{idPost:int}/{idTag:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteTagFromPost([FromRoute] int idPost,[FromRoute] int idTag)
    {
        var posts = await _postsDbRepository.DeleteTagFromPost(idPost,idTag);
        
        if (posts is null)
        {
            return NotFound(new StringMessageDto { Message ="Cannot delete tag"});
        }
        return Ok(posts);
    }
    
    
    
}