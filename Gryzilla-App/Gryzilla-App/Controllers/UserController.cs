using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserDbRepository _userDbRepository;
    public UserController(IUserDbRepository userDbRepository)
    {
        _userDbRepository = userDbRepository;
    }
    
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int idUser)
    {
        var user = await _userDbRepository.GetUserFromDb(idUser);
        if (user is null)
          return NotFound(null);
        
        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int id)
    {
        return Ok("modify");
    }

    [HttpPost("{idUser:int}")]
    public async Task<IActionResult> PostNewUser([FromRoute] int idUser)
    {
        return Ok("add-pppppp");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
       
        return Ok("delete");
    }

}