using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.DTOs.Responses.Group;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetUsers([FromRoute] int qtyUsers, [FromQuery] DateTime time, [FromQuery] string word)
    {
        UsersQtyDto? users;
        
        try
        {
            users = await _searchDbRepository.GetUsersByNameFromDb(qtyUsers, time, word);
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        return Ok(users);
    }
    
    /// <summary>
    /// Get users by name
    /// </summary>
    /// <returns>
    /// Return users list
    /// </returns>
    [HttpGet("getUsersByName")]
    [Authorize(Roles = "Admin, Moderator")]
    public async Task<IActionResult> GetUsers([FromQuery] string nick)
    {
        var users = await _searchDbRepository.GetUsersByName(nick);
        
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

        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
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

        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Get posts by tag
    /// </summary>
    /// <returns>
    /// Return post list
    /// </returns>
    [HttpGet("getPostsByTag/{qtyPosts:int}")]
    public async Task<IActionResult> GetPostsByTag([FromRoute] int qtyPosts, [FromQuery] DateTime time, [FromQuery] string word)
    {
        PostQtyDto? posts;
        
        try
        {
            posts = await _searchDbRepository
                .GetPostsByTagFromDb(qtyPosts, time, word);

        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }

        return Ok(posts);
    }
    
    /// <summary>
    /// Get articles by tag
    /// </summary>
    /// <returns>
    /// Return post list
    /// </returns>
    [HttpGet("getArticlesByTag/{qtyArticles:int}")]
    public async Task<IActionResult> GetArticlesByTag([FromRoute] int qtyArticles, [FromQuery] DateTime time, [FromQuery] string word)
    {
        ArticleQtyDto? articles;
        
        try
        {
            articles = await _searchDbRepository
                .GetArticlesByTagFromDb(qtyArticles, time, word);
        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }

        return Ok(articles);
    }
    
    
    /// <summary>
    /// Get articles by word
    /// </summary>
    /// <returns>
    /// Return articles list
    /// </returns>
    [HttpGet("getGroupsByWord/{qtyGroups:int}")]
    public async Task<IActionResult> GetGroups([FromRoute] int qtyGroups, [FromQuery] DateTime time, [FromQuery] string word)
    {
        GroupsQtySearchDto? articles;
        
        try
        {
            articles = await _searchDbRepository.GetGroupsByWordFromDb(qtyGroups, time, word);

        } 
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
        
        return Ok(articles);
    }
}