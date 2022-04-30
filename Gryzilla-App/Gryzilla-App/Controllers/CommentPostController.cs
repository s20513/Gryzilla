using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/posts/comment")]
public class CommentPostController : Controller
{
    private readonly ICommentPostDbRepository _commentPostDbRepository;
    
    public CommentPostController(ICommentPostDbRepository commentPostDbRepository)
    {
        _commentPostDbRepository = commentPostDbRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] AddCommentDto addCommentDto)
    {
        var comment = await _commentPostDbRepository.PostNewCommentToDb(addCommentDto);
        if (comment is null)
        {
            return NotFound("Cannot added new comment");
        }
        return Ok(comment);
    }
    
    [HttpPut]
    public async Task<IActionResult> ModifyComment([FromBody] PutCommentDto putCommentDto)
    {
        var comment = await _commentPostDbRepository.ModifyCommentToDb(putCommentDto);
        if (comment is null)
        {
            return NotFound("Cannot modify comment");
        }
        return Ok(comment);
    }
    
    [HttpDelete("{idComment:int}")]
    public async Task<IActionResult> ModifyComment([FromRoute] int idComment)
    {
        var comment = await _commentPostDbRepository.DeleteCommentFromDb(idComment);
        if (comment is null)
        {
            return NotFound("Cannot delete comment");
        }
        return Ok(comment);
    }
}