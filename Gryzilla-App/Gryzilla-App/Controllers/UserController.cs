using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    /// <summary>
    /// Find user by id
    /// </summary>
    /// <param name="idUser"> User Identifier</param>
    /// <returns>Return Status OK - if user exists, return user body, otherwise return status Not Found</returns>
    [HttpGet("{idUser:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int idUser)
    {
        var user = 
            await _userDbRepository.GetUserFromDb(idUser);

        if (user == null)
        {
            return NotFound(new StringMessageDto{ Message = "User doesn't exist"}); 
        }
        
        return Ok(user);
    }
    
    /// <summary>
    /// Find all users from db
    /// </summary>
    /// <returns>Return Ok - return list of users, NotFound - if there are no users</returns>
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var user = 
            await _userDbRepository.GetUsersFromDb();

        if (user is null)
        {
            return NotFound(new StringMessageDto{ Message = "Users don't exist"});
        }

        return Ok(user);
    }
    /// <summary>
    ///  Modify user 
    /// </summary>
    /// <param name="idUser">Int - User Identifier</param>
    /// <param name="putUserDto">Dto to store new user information</param>
    /// <returns>Return Status Ok - information about user modified correctly, return user body, Not Found - User doesn't exist</returns>
    [HttpPut("{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ModifyUser([FromRoute] int idUser, [FromBody] PutUserDto putUserDto)
    {
        if (idUser != putUserDto.IdUser)
        {
            return BadRequest(new StringMessageDto{ Message = "Id from route and Id in body have to be same"});
        }
        try
        {
            var user = await _userDbRepository.ModifyUserFromDb(idUser, putUserDto, User);
        
            if (user == null)
            { 
                return NotFound(new StringMessageDto{ Message = "User doesn't exist"});
            }
        
            return Ok(user);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
    }

    /// <summary>
    ///  Add new user TO DO
    /// </summary>
    /// <param name="addUserDto">Dto - information about new user</param>
    /// <returns> Return Status Ok - New user added correctly, return user body</returns>
    [HttpPost("register")]
    public async Task<IActionResult> PostNewUser([FromBody] AddUserDto addUserDto){

        try
        {
            var user = await _userDbRepository.AddUserToDb(addUserDto);
        
            if (user == null)
            { 
                return NotFound(new StringMessageDto{ Message = "User doesn't exist"});
            }
        
            return Ok(user);
        }
        catch (SameNameException e)
        {
            return BadRequest(new StringMessageDto{ Message = e.Message});
        }
    }
    
    /// <summary>
    ///  Delete a user using the User Identifier TO DO
    /// </summary>
    /// <param name="idUser"> int - User Identifier </param>
    /// <returns> Return Status OK - User deleted correctly - return user body. Return Status Not Found - if user doesn't exist</returns>
    [HttpDelete("{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> DeleteUser([FromRoute] int idUser)
    {
        var user = await _userDbRepository.DeleteUserFromDb(idUser, User);
        
        if (user == null)
        { 
            return NotFound(new StringMessageDto{ Message = "User doesn't exist"});
        }
        
        return Ok(user);
    }
    
    /// <summary>
    ///  Creates token for User
    /// </summary>
    /// <param name="loginRequestDto"> LoginRequestDto - Nick and password given by user </param>
    /// <returns> Return Status OK - New token created. Return Status Forbid - given credentials are wrong</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var res = await _userDbRepository.Login(loginRequestDto);
        
        if (res == null)
        { 
            return Forbid();
        }
        
        return Ok(res);
    }
    
    /// <summary>
    ///  Refresh users token
    /// </summary>
    /// <param name="refreshToken"> RefreshTokenDto - refresh token </param>
    /// <returns> Return Status OK - New token created. Return Status Forbid - token expired</returns>
    [HttpPost("refreshToken")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshToken)
    {
        var res = await _userDbRepository.RefreshToken(refreshToken);
        
        if (res == null)
        { 
            return Forbid("Token expired");
        }
        
        return Ok(res);
    }
    
    /// <summary>
    /// Changes user rank
    /// </summary>
    /// <param name="userRank"> UserRank - user and new rank id </param>
    /// <returns> Return Status OK - User has new rank</returns>
    /// <returns> Return Status NotFound - Rank or user does not exists</returns>
    [HttpPost("rank")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRank([FromBody] UserRank userRank)
    {
        var res = await _userDbRepository.ChangeUserRank(userRank);
        
        if (res == null)
        { 
            return NotFound(new StringMessageDto{ Message = "Rank or user does not exists!"});
        }
        
        return Ok(res);
    }
    
    /// <summary>
    /// Sets user photo
    /// </summary>
    /// <param name="file"> IFormFile - photo file </param>
    /// <param name="idUser"> int - user id </param>
    /// <returns> Return Status OK - UseDto - data of the user</returns>
    [HttpPost("photo/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> SetUserPhoto([FromForm] IFormFile file,[FromRoute] int idUser)
    {
        var res = await _userDbRepository.SetUserPhoto(file, idUser, User);
        
        return Ok(res);
    }
    
    /// <summary>
    /// Gets user photo
    /// </summary>
    /// <param name="idUser"> int - user id </param>
    /// <returns> Return Status OK - UseDto - data of the user</returns>
    [HttpGet("photo/{idUser:int}")]
    public async Task<IActionResult> GetUserPhoto([FromRoute] int idUser)
    {
        var res = await _userDbRepository.GetUserPhoto(idUser);
        
        return Ok(res);
    }

    /// <summary>
    /// ChangeUserPassword
    /// </summary>
    /// <param name="changePasswordDto"> ChangePasswordDto - old and new password </param>
    /// <param name="idUser"> int - user id </param>
    /// <returns> Return Status NotFound - User was not found</returns>
    /// <returns> Return Status BadRequest - Wrong old password</returns>
    /// <returns> Return Status OK - new password was set</returns>
    [HttpPut("password/{idUser:int}")]
    [Authorize(Roles = "Admin, User, Moderator, Redactor")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordDto changePasswordDto, [FromRoute] int idUser)
    {
        var res = await _userDbRepository.ChangePassword(changePasswordDto, idUser, User);

        return res switch
        {
            null => NotFound(),
            false => BadRequest(),
            _ => Ok()
        };
    }
    
    /// <summary>
    /// ChangeUserPassword
    /// </summary>
    /// <param name="changePasswordShortDto"> ChangePasswordDto - new password </param>
    /// <param name="idUser"> int - user id </param>
    /// <returns> Return Status NotFound - User was not found</returns>
    /// <returns> Return Status OK - new password was set</returns>
    [HttpPut("password/admin/{idUser:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordShortDto changePasswordShortDto, [FromRoute] int idUser)
    {
        var res = await _userDbRepository.ChangePassword(changePasswordShortDto, idUser);

        if (res)
        {
            return Ok();
        }

        return NotFound();
    }
    
    
    [HttpGet("exist")]
    public async Task<IActionResult> NickExist([FromQuery] CheckNickDto checkNickDto)
    {
        var res = await _userDbRepository.ExistUserNick(checkNickDto.Nick);

       return Ok(res);

    }
}