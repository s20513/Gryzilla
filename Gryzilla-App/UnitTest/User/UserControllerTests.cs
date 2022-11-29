using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.DTOs.Responses.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Users don't exist", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(exceptionMessage, resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Id from route and Id in body have to be same", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Nick with given name already exists!", resultValue);
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
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User doesn't exist", resultValue);
    }

}