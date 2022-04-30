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
    
    
}