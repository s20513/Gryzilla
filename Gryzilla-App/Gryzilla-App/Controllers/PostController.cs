using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postsDbRepository.GetPostsFromDb();
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    
    [HttpGet("/bylikes/least")]
    public async Task<IActionResult> GetPostsByLikes()
    {
        var posts = await _postsDbRepository.GetPostsByLikesLeastFromDb();
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    [HttpGet("/bylikes/most")]
    public async Task<IActionResult> GetPostsByLikesLeast()
    {
        var posts = await _postsDbRepository.GetPostsByLikesFromDb();
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    [HttpGet("/dates/lates")]
    public async Task<IActionResult> GetPostsByDates()
    {
        var posts = await _postsDbRepository.GetPostsByDateFromDb();
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    
    [HttpGet("/dates/oldest")]
    public async Task<IActionResult> GetPostsByDatesOldest()
    {
        var posts = await _postsDbRepository.GetPostsByDateOldestFromDb();
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    
    [HttpGet("{idPost:int}")]
    public async Task<IActionResult> GetOnePost([FromRoute] int idPost)
    {
        var posts = await _postsDbRepository.GetOnePostFromDb(idPost);
        if (posts is null)
        {
            return NotFound("No posts found");
        }
        return Ok(posts);
    }
    [HttpPost]
    public async Task<IActionResult> AddPosts([FromBody] AddPostDto addPostDto)
    {
        var posts = await _postsDbRepository.AddNewPostFromDb(addPostDto);
        if (posts is null)
        {
            return NotFound("Cannot add new post");
        }
        return Ok(posts);
    }
    
    [HttpPut]
    public async Task<IActionResult> AddPosts([FromBody] PutPostDto putPostDto)
    {
        var posts = await _postsDbRepository.ModifyPostFromDb(putPostDto);
        if (posts is null)
        {
            return NotFound("Cannot modify post");
        }
        return Ok(posts);
    }
    [HttpDelete("{idPost:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int idPost)
    {
        
        var posts = await _postsDbRepository.DeletePostFromDb(idPost);
        if (posts is null)
        {
            return NotFound("Cannot delete new post");
        }
        return Ok(posts);
    }
    
    [HttpDelete("/tag/{idPost:int}/{idTag:int}")]
    public async Task<IActionResult> DeleteTagFromPost([FromRoute] int idPost,[FromRoute] int idTag)
    {
        
        var posts = await _postsDbRepository.DeleteTagFromPost(idPost,idTag);
        if (posts is null)
        {
            return NotFound("Cannot delete tag");
        }
        return Ok(posts);
    }
}