﻿using Gryzilla_App.DTOs.Requests.ArticleComment;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/articles/comments")]
public class CommentArticleController: Controller
{
    private readonly ICommentArticleDbRepository _articleDbRepository;
    
    public CommentArticleController(ICommentArticleDbRepository articleDbRepository)
    {
        _articleDbRepository = articleDbRepository;
    }

    /// <summary>
    /// Create article comment
    /// </summary>
    /// <param name="newArticleCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Article Comment created correctly, returns  ArticleComment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    [HttpPost]
    public async Task<IActionResult> CreateNewArticleComment([FromBody] NewArticleCommentDto newArticleCommentDto)
    {
        var result = await _articleDbRepository.AddCommentToArticle(newArticleCommentDto);
        
        if (result is null)
        {
            return NotFound("User or article not found");
        }
            
        return Ok(result);
    }

    /// <summary>
    /// Modify Article Comment
    /// </summary>
    /// <param name="idComment">Article Comment id</param>
    /// <param name="putArticleCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Article Comment modified correctly, returns  ArticleComment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    /// <returns>Return Status Bad Request - Id from route and Id in body are not some</returns>
    [HttpPut("{idComment:int}")]
    public async Task<IActionResult> ModifyArticleComment(PutArticleCommentDto putArticleCommentDto, int idComment)
    {
        if (putArticleCommentDto.IdComment != idComment)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }
        
        var result = await _articleDbRepository
            .ModifyArticleCommentFromDb(putArticleCommentDto, idComment);

        if (result is null)
        {
            return NotFound("Comment not found");
        }
            
        return Ok(result);
    }

    /// <summary>
    /// Delete achievement
    /// </summary>
    /// <param name="idComment">Article Comment Id</param>
    /// <returns> Return Status Ok - Article Comment deleted correctly, returns Article Comment body</returns>
    /// <returns>Return status Not Found - Article Comment not found</returns>
    [HttpDelete("{idComment:int}")]
    public async Task<IActionResult> DeleteArticleComment(int idComment)
    {
        var result = await _articleDbRepository.DeleteArticleCommentFromDb(idComment);

        if (result is null)
        {
            return NotFound("Comment not found");
        }
            
        return Ok(result);
    }
}