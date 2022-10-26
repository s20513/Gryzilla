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
    
    
    /// <summary>
    /// Create Post Comment
    /// </summary>
    /// <param name="newPostCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Post Comment created correctly, returns  Post Comment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePostComment([FromBody] NewPostCommentDto newPostCommentDto)
    {
        var comment = await _commentPostDbRepository.AddCommentToPost(newPostCommentDto);
        
        if (comment is null)
        {
            return NotFound("Cannot add new comment");
        }
        
        return Ok(comment);
    }
    
    /// <summary>
    /// Modify Post Comment
    /// </summary>
    /// <param name="idComment">Post Comment id</param>
    /// <param name="putPostCommentDto">Dto to store new Comment information</param>
    /// <returns>Return Status Ok - Post Comment modified correctly, returns Post Comment body</returns>
    /// <returns>Return Status Not Found - Comment doesn't exist</returns>
    /// <returns>Return Status Bad Request - Id from route and Id in body are not some</returns>
    [HttpPut("{idComment:int}")]
    public async Task<IActionResult> ModifyPostComment([FromBody] PutPostCommentDto putPostCommentDto, [FromRoute] int idComment)
    {
        if (idComment != putPostCommentDto.IdComment)
        {
            return BadRequest("Id from route and Id in body have to be same");
        }
        
        var comment = await _commentPostDbRepository.ModifyPostCommentFromDb(putPostCommentDto, idComment);
        
        if (comment is null)
        {
            return NotFound("Comment not found");
        }
        return Ok(comment);
    }
    
    /// <summary>
    /// Delete Post Comment
    /// </summary>
    /// <param name="idComment">Post Comment Id</param>
    /// <returns> Return Status Ok - Post Comment deleted correctly, returns Post Comment body</returns>
    /// <returns>Return status Not Found - Post Comment not found</returns>
    [HttpDelete("{idComment:int}")]
    public async Task<IActionResult> DeletePostComment([FromRoute] int idComment)
    {
        var comment = await _commentPostDbRepository.DeleteCommentFromDb(idComment);
        
        if (comment is null)
        {
            return NotFound("Comment not found");
        }
        
        return Ok(comment);
    }
    
}