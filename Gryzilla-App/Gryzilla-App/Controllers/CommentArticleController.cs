using Gryzilla_App.DTOs.Requests.ArticleComment;
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

    [HttpPost]
    public async Task<IActionResult> CreateNewArticleComment([FromBody] NewArticleCommentDto newArticleCommentDto)
    {
        var result = await _articleDbRepository.AddCommentToArticle(newArticleCommentDto);
        if (result is null)
            return NotFound("User or article not found");
        return Ok(result);
    }

    [HttpPut("{idComment:int}")]
    public async Task<IActionResult> ModifyArticleComment(PutArticleCommentDto putArticleCommentDto, int idComment)
    {
        var result = await _articleDbRepository.ModifyArticleCommentFromDb(putArticleCommentDto, idComment);
        if (result is null)
            return NotFound("Comment not found");
        return Ok(result);
    }

    [HttpDelete("{idComment:int}")]
    public async Task<IActionResult> DeleteArticleComment(int idComment)
    {
        var result = await _articleDbRepository.DeleteArticleCommentFromDb(idComment);
        if (result is null)
            return NotFound("Comment not found");
        return Ok(result);
    }
}