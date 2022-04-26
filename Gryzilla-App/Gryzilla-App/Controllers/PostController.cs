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
    
    [HttpDelete("{idPost:int}")]
    public async Task<IActionResult> AddPosts([FromRoute] int idPost)
    {
        //w toku
        var posts = await _postsDbRepository.DeletePostFromDb(idPost);
        if (posts is null)
        {
            return NotFound("Cannot add new post");
        }
        return Ok(posts);
    }
}