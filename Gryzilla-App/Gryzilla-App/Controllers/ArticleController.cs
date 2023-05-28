using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.Articles;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController: Controller
{
    private readonly IArticleDbRepository _articleDbRepository;

    public ArticleController(IArticleDbRepository articleDbRepository)
    {
        _articleDbRepository = articleDbRepository;
    }

    /// <summary>
    /// Find all Articles from db
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllArticlesFromDb()
    {
        var articles = await _articleDbRepository.GetArticlesFromDb();
        
        if (articles is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Get user articles
    /// </summary>
    /// <param name="idUser">int - User Identifier</param>
    /// <returns>
    /// NotFound - user has no articles
    /// Ok - return user articles
    /// </returns>
    [HttpGet("user/{idUser:int}")]
    public async Task<IActionResult> GetUserArticles([FromRoute] int idUser)
    {
        var articles = await _articleDbRepository.GetUserArticlesFromDb(idUser);

        return Ok(articles);
    }

    /// <summary>
    /// Find all Articles from db sorted by Likes
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by most Likes</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("byLikes/most")]
    public async Task<IActionResult> GetAllArticlesByMostLikesFromDb()
    {
        var articles = await _articleDbRepository.GetArticlesByMostLikesFromDb();
        
        if (articles is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Find all Articles from db sorted by Likes
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by least Likes</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("byLikes/least")]
    public async Task<IActionResult> GetAllArticlesByLeastLikesFromDb()
    {
        var articles = await _articleDbRepository.GetArticlesByLeastLikesFromDb();
        
        if (articles is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Find all Articles from db sorted by date
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by earliest date</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("byDate/earliest")]
    public async Task<IActionResult> GetAllArticlesByEarliestDateFromDb()
    {
        var articles = await _articleDbRepository.GetArticlesByEarliestDateFromDb();
        
        if (articles is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Find all Articles from db sorted by date
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by oldest date</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("byDate/oldest")]
    public async Task<IActionResult> GetAllArticlesByOldestDateFromDb()
    {
        var articles = await _articleDbRepository.GetArticlesByOldestDateFromDb();
        
        if (articles is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(articles);
    }

    /// <summary>
    /// Find all Articles from db
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("qty/{qtyArticles:int}")]
    public async Task<IActionResult> GetQtyAllArticlesFromDb([FromRoute] int qtyArticles)
    {
        ArticleQtyDto? articles;
        try
        {
            articles = await _articleDbRepository.GetQtyArticlesFromDb(qtyArticles);
            if (articles is null)
            {
                return NotFound(new StringMessageDto{ Message = "No articles found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
       
        
        return Ok(articles);
    }
    /// <summary>
    /// Find all Articles from db sorted by Likes
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by most Likes</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("qty/byLikesDesc/{qtyArticles:int}")]
    public async Task<IActionResult> GetQtyArticlesByMostLikesFromDb([FromRoute] int qtyArticles, [FromQuery] DateTime time)
    {
        ArticleQtyDto? articles;
        try
        {
            articles = await _articleDbRepository.GetQtyArticlesByMostLikesFromDb(qtyArticles, time);
            if (articles is null)
            {
                return NotFound(new StringMessageDto{ Message = "No articles found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Find all Articles from db sorted by Comments
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by comments</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("qty/byCommentsDesc/{qtyArticles:int}")]
    public async Task<IActionResult> GetQtyArticlesByCommentsFromDb([FromRoute] int qtyArticles, [FromQuery] DateTime time)
    {
        ArticleQtyDto? articles;
        try
        { 
            articles = await _articleDbRepository.GetQtyArticlesByCommentsFromDb(qtyArticles, time);
            if (articles is null)
            {
                return NotFound(new StringMessageDto{ Message = "No articles found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Find all Articles from db sorted by date
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by earliest date</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("qty/byDateDesc/{qtyArticles:int}")]
    public async Task<IActionResult> GetQtyArticlesByEarliestDateFromDb([FromRoute] int qtyArticles, [FromQuery] DateTime time)
    {
        ArticleQtyDto? articles;
        try
        { 
            articles = await _articleDbRepository.GetQtyArticlesByEarliestDateFromDb(qtyArticles, time);
        
            if (articles is null)
            {
                return NotFound(new StringMessageDto{ Message = "No articles found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
        
        return Ok(articles);
    }
    /// <summary>
    /// Get top articles
    /// </summary>
    /// <returns>
    /// Return articles list
    /// </returns>
    [HttpGet("top")]
    public async Task<IActionResult> GetTopArticles()
    {
        IEnumerable<ArticleDto>? posts = await _articleDbRepository.GetTopArticles();

        if (posts is null)
        {
            return NotFound(new StringMessageDto{ Message = "No articles found" });
        }
        
        return Ok(posts);
    }
    /// <summary>
    /// Find all Articles from db sorted by date
    /// </summary>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by oldest date</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("qty/byDateAsc/{qtyArticles:int}")]
    public async Task<IActionResult> GetQtyArticlesByOldestDateFromDb([FromRoute] int qtyArticles, [FromQuery] DateTime time)
    {
        ArticleQtyDto? articles;
        try
        { 
            articles = await _articleDbRepository.GetQtyArticlesByOldestDateFromDb(qtyArticles, time);
        
            if (articles is null)
            {
                return NotFound(new StringMessageDto{ Message = "No articles found" });
            }
        }
        catch (WrongNumberException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message });
        }
        
        return Ok(articles);
    }

    /// <summary>
    /// Find all Articles from db sorted by date
    /// </summary>
    /// <param name="idArticle">Article id</param>
    /// <returns>Return Status OK - if any Article exists, return Articles sorted by oldest date</returns>
    /// <returns>Return Status Not Found - no Articles in db</returns>
    [HttpGet("{idArticle:int}")]
    public async Task<IActionResult> GetArticleFromDb(int idArticle)
    {
        var article = await _articleDbRepository.GetArticleFromDb(idArticle);
        
        if (article is null)
        {
            return NotFound(new StringMessageDto{ Message = "Article not found" });
        }
        
        return Ok(article);
    }

    /// <summary>
    /// Create new Article
    /// </summary>
    /// <param name="newArticleRequestDto">Dto with new Article data</param>
    /// <returns>Return Status OK - Article added to Db</returns>
    /// <returns>Return Status Not Found - User (creator) not found</returns>
    [Authorize(Roles = "Admin, Redactor")]
    [HttpPost]
    public async Task<IActionResult> AddNewArticleToDb([FromBody] NewArticleRequestDto newArticleRequestDto)
    {
        var result = await _articleDbRepository.AddNewArticleToDb(newArticleRequestDto);
        
        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "User not found" });
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Modify Article
    /// </summary>
    /// <param name="putArticleRequestDto">Dto with Article new data</param>
    /// <param name="idArticle">Article id</param>
    /// <returns>Return Status OK - Article modified</returns>
    /// <returns>Return Status Not Found - Article not found</returns>
    /// <returns>Return Status Bad Request - Id from route and Id in body were not same</returns>
    [Authorize(Roles = "Admin, Redactor")]
    [HttpPut("{idArticle:int}")]
    public async Task<IActionResult> ModifyArticleFromDb(
        [FromBody] PutArticleRequestDto putArticleRequestDto, 
        [FromRoute] int idArticle)
    {
        if (putArticleRequestDto.IdArticle != idArticle)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same" });
        }
        
        var result = await _articleDbRepository.ModifyArticleFromDb(putArticleRequestDto, idArticle);
        
        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Article not found" });
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Delete Article
    /// </summary>
    /// <param name="idArticle">Article id</param>
    /// <returns>Return Status OK - Article deleted</returns>
    /// <returns>Return Status Not Found - Article not found</returns>
    [Authorize(Roles = "Admin, Moderator, Redactor")]
    [HttpDelete("{idArticle:int}")]
    public async Task<IActionResult> DeleteArticleFromDb([FromRoute] int idArticle)
    {
        var result = await _articleDbRepository.DeleteArticleFromDb(idArticle);
        
        if (result is null)
        {
            return NotFound(new StringMessageDto{ Message = "Article not found" });
        }
        
        return Ok(result);
    }
    
 
}