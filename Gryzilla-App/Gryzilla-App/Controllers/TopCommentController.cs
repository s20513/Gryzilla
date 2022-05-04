using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;


[ApiController]
[Route("api/top/comments")]
public class TopCommentController : Controller
{
    private readonly ITopCommentDbRepository _commentDbRepository;

    public TopCommentController(ITopCommentDbRepository commentDbRepository)
    {
        _commentDbRepository = commentDbRepository;
    }
    
    //zostawiam na przyszlosc
    
}