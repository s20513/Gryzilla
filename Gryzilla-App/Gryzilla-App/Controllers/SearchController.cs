using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
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
        UsersQtyDto users;
        
        try
        {
            users = await _searchDbRepository.GetUsersByNameFromDb(qtyUsers, time, nickName);
            if (users == null)
            {
                return NotFound("No users found"); 
            }
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(users);
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
        PostQtySearchDto? posts;
        
        try
        {
            posts = await _searchDbRepository.GetPostByWordFromDb(qtyPosts, time, word);

            if (posts == null)
            {
                return NotFound("No posts found"); 
            }
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(posts);
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
        ArticleQtySearchDto? articles;
        
        try
        {
            articles = await _searchDbRepository.GetArticleByWordFromDb(qtyArticles, time, word);

            if (articles == null)
            {
                return NotFound("No articles found"); 
            }
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok(articles);
    }
}