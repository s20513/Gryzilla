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
        return Ok("Wszystko super");
    }

=======
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ModifyUser([FromRoute] int id)
    {
        return Ok("usunieto");
    }
>>>>>>> Stashed changes
}