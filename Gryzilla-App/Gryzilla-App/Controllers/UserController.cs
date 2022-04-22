using Gryzilla_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly GryzillaContext _context;
    public UserController(GryzillaContext context)
    {
        _context = context;
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        // var user = await _context.UserData.Where(x => x.IdUser == 1).SingleOrDefaultAsync();
        // var user1 = await _context.UserData.Where(x => x.IdUser == 2).SingleOrDefaultAsync();
        //
        //
        // user.IdUserFriends.Add(user1);
        // user.IdUsers.Add(user);
        // user1.IdUserFriends.Add(user);
        // user1.IdUsers.Add(user1);
        // await _context.SaveChangesAsync();
        return Ok("dodano");
    }
    
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int idUser)
    {
        return Ok("pokazało");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int id)
    {
        return Ok("wspolna");
    }

    [HttpPost("{idUser:int}")]
    public async Task<IActionResult> GetPost([FromRoute] int idUser)
    {
        
        return Ok("dodano");
    }
    
    public async Task<IActionResult> GetArticle([FromRoute] int idUser)
    {
        return Ok("dodano");
    }

}