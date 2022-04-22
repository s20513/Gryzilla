using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/friends")]
public class FriendsController : Controller
{
    private readonly IFriendsDbRepository _friendsDbRepository;
    public FriendsController(IFriendsDbRepository friendsDbRepository)
    {
        _friendsDbRepository = friendsDbRepository;
    }

    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetFriends([FromRoute] int idUser)
    {
        return Ok("dodano");
    }
}