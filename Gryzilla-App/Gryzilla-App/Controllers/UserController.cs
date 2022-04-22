using Microsoft.AspNetCore.Mvc;

namespace Gryzilla_App.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        return Ok("usunieto");
    }
    
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int idUser)
    {
        return Ok("pokazało");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int id)
    {
        return Ok("zmieniono");
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