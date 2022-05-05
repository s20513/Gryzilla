using Gryzilla_App.DTO.Requests.User;
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
        
        if(user == null)
            return NotFound();

        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var user = await _userDbRepository.GetUsersFromDb();
        
        if(!user.Any())
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{idUser:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int idUser, [FromBody] PutUserDto putUserDto)
    {
       var user = await _userDbRepository.ModifyUserFromDb(idUser, putUserDto);
       if (user == null)
           return BadRequest();
       
       return Ok(user);
    }

    [HttpPost("{idUser:int}")]
    public async Task<IActionResult> PostNewUser([FromRoute] int idUser){
        return Ok("add new user");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
       
        return Ok("delete");
    }

}