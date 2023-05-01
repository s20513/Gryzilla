using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : Controller 
{
    private readonly ISearchDbRepository _searchDbRepository;
    public SearchController(ISearchDbRepository searchDbRepository)
    {
        _searchDbRepository = searchDbRepository;
    }
    
    /// <summary>
    /// Get users by name
    /// </summary>
    /// <returns>
    /// Return users list
    /// </returns>
    [HttpGet("getUsersByName/{qtyUsers:int}")]
    public async Task<IActionResult> GetUsers([FromRoute] int qtyUsers, [FromQuery] DateTime time, [FromQuery] string nickName)
    {
        var user = 
            await _searchDbRepository.GetUsersByNameFromDb(qtyUsers, time, nickName);

        if (user == null)
        {
            return NotFound("No users found"); 
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Get posts by word
    /// </summary>
    /// <returns>
    /// Return posts list
    /// </returns>
    [HttpGet("getPostsByWord/{qtyPosts:int}")]
    public async Task<IActionResult> GetPosts([FromRoute] int qtyPosts, [FromQuery] DateTime time, [FromQuery] string word)
    {
        var user = 
            await _searchDbRepository.GetPostByWordFromDb(qtyPosts, time, word);

        if (user == null)
        {
            return NotFound("No posts found"); 
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Get articles by word
    /// </summary>
    /// <returns>
    /// Return articles list
    /// </returns>
    [HttpGet("getArticlesByWord/{qtyArticles:int}")]
    public async Task<IActionResult> GetArticles([FromRoute] int qtyArticles, [FromQuery] DateTime time, [FromQuery] string word)
    {
        var user = 
            await _searchDbRepository.GetArticleByWordFromDb(qtyArticles, time, word);

        if (user == null)
        {
            return NotFound("No articles found"); 
        }
        
        return Ok(user);
    }
}