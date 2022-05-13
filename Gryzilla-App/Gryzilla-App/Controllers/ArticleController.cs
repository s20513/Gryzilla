﻿using Gryzilla_App.DTOs.Requests.Article;
using Gryzilla_App.Repositories.Interfaces;
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

    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await _articleDbRepository.GetArticlesFromDb();
        if (articles is null)
            return NotFound("No articles found");
        return Ok(articles);
    }

    [HttpGet("/articles/bylikes/most")]
    public async Task<IActionResult> GetArticlesByMostLikes()
    {
        var articles = await _articleDbRepository.GetArticlesByMostLikesFromDb();
        if (articles is null)
            return NotFound("No articles found");
        return Ok(articles);
    }
    
    [HttpGet("/articles/bylikes/least")]
    public async Task<IActionResult> GetArticlesByLeastLikes()
    {
        var articles = await _articleDbRepository.GetArticlesByLeastLikesFromDb();
        if (articles is null)
            return NotFound("No articles found");
        return Ok(articles);
    }
    
    [HttpGet("/articles/bydate/least")]
    public async Task<IActionResult> GetArticlesByEarliestDate()
    {
        var articles = await _articleDbRepository.GetArticlesByEarliestDateFromDb();
        if (articles is null)
            return NotFound("No articles found");
        return Ok(articles);
    }
    
    [HttpGet("/articles/bydate/oldest")]
    public async Task<IActionResult> GetArticlesByOldestDate()
    {
        var articles = await _articleDbRepository.GetArticlesByOldestDateFromDb();
        if (articles is null)
            return NotFound("No articles found");
        return Ok(articles);
    }

    [HttpGet("{idArticle:int}")]
    public async Task<IActionResult> GetArticle([FromRoute]int idArticle)
    {
        var article = await _articleDbRepository.GetArticleFromDb(idArticle);
        if (article is null)
            return NotFound("Article not found");
        return Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewArticle([FromBody] NewArticleRequestDto newArticleRequestDto)
    {
        var result = await _articleDbRepository.AddNewArticleToDb(newArticleRequestDto);
        if (result is null)
            return NotFound("User not found");
        return Ok(result);
    }

    [HttpPut("{idArticle:int}")]
    public async Task<IActionResult> ModifyArticle([FromBody] PutArticleRequestDto putArticleRequestDto,
        [FromRoute] int idArticle)
    {
        var result = await _articleDbRepository.ModifyArticleFromDb(putArticleRequestDto, idArticle);
        if (result is null)
            return NotFound("Article not found");
        return Ok(result);
    }

    [HttpDelete("{idArticle:int}")]
    public async Task<IActionResult> DeleteArticle([FromRoute] int idArticle)
    {
        var result = await _articleDbRepository.DeleteArticleFromDb(idArticle);
        if (result is null)
            return NotFound("Article not found");
        return Ok(result);
    }

}