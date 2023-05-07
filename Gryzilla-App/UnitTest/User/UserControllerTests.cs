using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.User;

public class UserControllerTests
{
    private readonly UserController _usersController;
    private readonly Mock<IUserDbRepository> _userRepositoryMock = new();
    
    
    public UserControllerTests()
    {
        _usersController = new UserController(_userRepositoryMock.Object);
    }
    
    
    [Fact]
    public async void GetUser_Returns_Ok()
    {
        //Arrange
        var idUser= 1;
        var user = new UserDto();
        
        _userRepositoryMock.Setup(x => x.GetUserFromDb(idUser)).ReturnsAsync(user);

        //Act
        var actionResult = await _usersController.GetUser(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(user, resultValue);
    }
    
    [Fact]
    public async void GetUser_Not_Found()
    {
        //Arrange
        var idUser = 1;
        UserDto? nullValue = null;
        
        _userRepositoryMock.Setup(x => x.GetUserFromDb(idUser)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.GetUser(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
    
    [Fact]
    public async void GetUsers_Returns_Ok()
    {
        //Arrange
        var users = new UserDto[5];
        
        _userRepositoryMock.Setup(x => x.GetUsersFromDb()).ReturnsAsync(users);

        //Act
        var actionResult = await _usersController.GetUsers();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(users, resultValue);
    }
    
    [Fact]
    public async void GetUsers_Returns_Not_Found()
    {
        //Arrange
        IEnumerable<UserDto>? nullValue = null;
        
        _userRepositoryMock.Setup(x => x.GetUsersFromDb())!.ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.GetUsers();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Users don't exist", resultValue.Message);
    }
    
     [Fact]
    public async void ModifyUser_Returns_Ok()
    {
        //Arrange
        var id = 5;
        var putUserDto = new PutUserDto
        {
            IdUser = id
        };

        var returnedUser = new UserDto
        {
            IdUser = id
        };
        
        _userRepositoryMock.Setup(x => x.ModifyUserFromDb(id, putUserDto)).ReturnsAsync(returnedUser);

        //Act
        var actionResult = await _usersController.ModifyUser(id, putUserDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdUser);
    }
    
    [Fact]
    public async void ModifyUser_Returns_Not_Found()
    {
        //Arrange
        var id = 5;
        var putUserDto = new PutUserDto
        {
            IdUser = id
        };

        UserDto? nullValue = null;
        
        _userRepositoryMock.Setup(x => x.ModifyUserFromDb(id, putUserDto)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.ModifyUser(id, putUserDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyUser_Returns_SameNameException_Bad_Request()
    {
        //Arrange
        var id = 5;
        var putUserDto = new PutUserDto
        {
            IdUser = id
        };
        var exceptionMessage = "Nick with given name already exists!";

        _userRepositoryMock
            .Setup(x => x.ModifyUserFromDb(id, putUserDto))
            .ThrowsAsync(new SameNameException(exceptionMessage));

        //Act
        var actionResult = await _usersController.ModifyUser(id, putUserDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue.Message);
    }
    [Fact]
    public async void ModifyUser_Returns_Bad_Request()
    {
        //Arrange
        var id = 5;
        var putUserDto = new PutUserDto
        {
            IdUser = 6
        };

        //Act
        var actionResult = await _usersController.ModifyUser(id, putUserDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue.Message);
    }
    
    
    [Fact]
    public async void CreateNewUser_Returns_Ok()
    {
        //Arrange
        var addUserDto = new AddUserDto();
        var user = new UserDto();
        
        _userRepositoryMock
            .Setup(x => x.AddUserToDb(addUserDto))
            .ReturnsAsync(user);
        
        //Act
        var actionResult = await _usersController.PostNewUser(addUserDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(user, resultValue);
    }
    
    
    [Fact]
    public async void CreateNewUser_Returns_Not_Found()
    {
        //Arrange
        var addUserDto = new AddUserDto();
        UserDto? nullValue = null;
        
        _userRepositoryMock
            .Setup(x => x.AddUserToDb(addUserDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.PostNewUser(addUserDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
    
    [Fact]
    public async void CreateNewUser_Returns_Bad_Request()
    {
        //Arrange
        var addUserDto = new AddUserDto();
        
        _userRepositoryMock
            .Setup(x => x.AddUserToDb(addUserDto))
            .Throws(new SameNameException("Nick with given name already exists!"));

        //Act
        var actionResult = await _usersController.PostNewUser(addUserDto);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Nick with given name already exists!", resultValue.Message);
    }
    
    [Fact]
    public async void DeleteUser_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var user = new UserDto();
        
        _userRepositoryMock.Setup(x => x.DeleteUserFromDb(idUser)).ReturnsAsync(user);

        //Act
        var actionResult = await _usersController.DeleteUser(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(user, resultValue);
    }
    
    [Fact]
    public async void DeleteUser_Not_Found()
    {
        //Arrange
        var idUser = 1;
        UserDto? nullValue = null;
        
        _userRepositoryMock.Setup(x => x.DeleteUserFromDb(idUser)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.DeleteUser(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue.Message);
    }
    
    [Fact]
    public async void ChangeUserRank_Returns_Ok()
    {
        //Arrange
        var userRank = new UserRank();
        var userDto = new UserDto();

        _userRepositoryMock.Setup(x => x.ChangeUserRank(userRank)).ReturnsAsync(userDto);

        //Act
        var actionResult = await _usersController.ChangeUserRank(userRank);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(userDto, resultValue);
    }
    
    [Fact]
    public async void ChangeUserRank_Not_Found()
    {
        //Arrange
        var userRank = new UserRank();
        UserDto? userDto = null;
        
        _userRepositoryMock.Setup(x => x.ChangeUserRank(userRank)).ReturnsAsync(userDto);

        //Act
        var actionResult = await _usersController.ChangeUserRank(userRank);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Rank or user does not exists!", resultValue.Message);
    }
    
    [Fact]
    public async void SetUserPhoto_Returns_Ok()
    {
        //Arrange
        var file = new Mock<IFormFile>();
        var idUser = 1;
        var userDto = new UserDto();

        _userRepositoryMock.Setup(x => x.SetUserPhoto(file.Object, idUser)).ReturnsAsync(userDto);

        //Act
        var actionResult = await _usersController.SetUserPhoto(file.Object, idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(userDto, resultValue);
    }
    
    [Fact]
    public async void GetUserPhoto_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var userPhotoResponseDto = new UserPhotoResponseDto();

        _userRepositoryMock.Setup(x => x.GetUserPhoto(idUser)).ReturnsAsync(userPhotoResponseDto);

        //Act
        var actionResult = await _usersController.GetUserPhoto(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserPhotoResponseDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(userPhotoResponseDto, resultValue);
    }
    
    [Fact]
    public async void ChangeUserPassword_WithCheckingOldPassword_Returns_NotFound()
    {
        //Arrange
        var idUser = 1;
        var changePasswordDto = new ChangePasswordDto();
        bool? nullValue = null;

        _userRepositoryMock.Setup(x => x.ChangePassword(changePasswordDto, idUser)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _usersController.ChangeUserPassword(changePasswordDto, idUser);
        
        //Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }
    
    [Fact]
    public async void ChangeUserPassword_WithCheckingOldPassword_Returns_BadRequest()
    {
        //Arrange
        var idUser = 1;
        var changePasswordDto = new ChangePasswordDto();
        var res = false;

        _userRepositoryMock.Setup(x => x.ChangePassword(changePasswordDto, idUser)).ReturnsAsync(res);

        //Act
        var actionResult = await _usersController.ChangeUserPassword(changePasswordDto, idUser);
        
        //Assert
        Assert.IsType<BadRequestResult>(actionResult);
    }
    
    [Fact]
    public async void ChangeUserPassword_WithCheckingOldPassword_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var changePasswordDto = new ChangePasswordDto();
        var res = true;

        _userRepositoryMock.Setup(x => x.ChangePassword(changePasswordDto, idUser)).ReturnsAsync(res);

        //Act
        var actionResult = await _usersController.ChangeUserPassword(changePasswordDto, idUser);
        
        //Assert
        Assert.IsType<OkResult>(actionResult);
    }
    
    [Fact]
    public async void ChangeUserPassword_WithoutCheckingOldPassword_Returns_NotFound()
    {
        //Arrange
        var idUser = 1;
        var changePasswordShortDto = new ChangePasswordShortDto();
        var res = false;

        _userRepositoryMock.Setup(x => x.ChangePassword(changePasswordShortDto, idUser)).ReturnsAsync(res);

        //Act
        var actionResult = await _usersController.ChangeUserPassword(changePasswordShortDto, idUser);
        
        //Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }
    
    [Fact]
    public async void ChangeUserPassword_WithoutCheckingOldPassword_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var changePasswordShortDto = new ChangePasswordShortDto();
        var res = true;

        _userRepositoryMock.Setup(x => x.ChangePassword(changePasswordShortDto, idUser)).ReturnsAsync(res);

        //Act
        var actionResult = await _usersController.ChangeUserPassword(changePasswordShortDto, idUser);
        
        //Assert
        Assert.IsType<OkResult>(actionResult);
    }

}