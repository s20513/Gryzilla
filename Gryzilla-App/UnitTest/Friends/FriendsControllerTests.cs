using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Friends;
using Gryzilla_App.DTOs.Responses.PostComment;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Friends;

public class FriendsControllerTests
{
    private readonly FriendsController _friendsController;
    private readonly Mock<IFriendsDbRepository> _friendsRepositoryMock = new();

    public FriendsControllerTests()
    {
        _friendsController = new FriendsController(_friendsRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetFriends_Returns_Ok()
    {
        //Arrange
        var id = 1;
        IEnumerable<FriendDto> fakeFriends = new List<FriendDto>();
        
        _friendsRepositoryMock
            .Setup(x => x.GetFriendsFromDb(id))
            .ReturnsAsync(fakeFriends);

        //Act
        var actionResult = await _friendsController.GetFriends(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<FriendDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(fakeFriends, resultValue);
    }
    
    [Fact]
    public async void GetFriends_Returns_Not_Found()
    {
        //Arrange
        var id = 1;
        IEnumerable<FriendDto>? nullValue = null;
        
        _friendsRepositoryMock
            .Setup(x => x.GetFriendsFromDb(id))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _friendsController.GetFriends(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User does not exists!", resultValue);
    }

    [Fact]
    public async void DeleteFriend_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 2; 
        var friendDto = new FriendDto();
        
        _friendsRepositoryMock
            .Setup(x => x.DeleteFriendFromDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.DeleteFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FriendDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(friendDto, resultValue);
    }
    
    [Fact]
    public async void DeleteFriend_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 2; 
        FriendDto? friendDto = null;
        
        _friendsRepositoryMock
            .Setup(x => x.DeleteFriendFromDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.DeleteFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("One of the users does not exists!", resultValue);
    }
    
    [Fact]
    public async void DeleteFriend_Returns_Bad_Request()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 1; 
        FriendDto? friendDto = null;
        
        _friendsRepositoryMock
            .Setup(x => x.DeleteFriendFromDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.DeleteFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Ids must have different values!", resultValue);
    }
    
    [Fact]
    public async void AddNewFriend_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 2; 
        var friendDto = new FriendDto();
        
        _friendsRepositoryMock
            .Setup(x => x.AddNewFriendToDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.AddNewFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FriendDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(friendDto, resultValue);
    }
    
    [Fact]
    public async void AddNewFriend_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 2; 
        FriendDto? friendDto = null;
        
        _friendsRepositoryMock
            .Setup(x => x.AddNewFriendToDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.AddNewFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("One of the users does not exists!", resultValue);
    }
    
    [Fact]
    public async void AddNewFriend_One_Of_The_Users_Does_Not_Exists_Returns_Bad_Request()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 2;

        _friendsRepositoryMock
            .Setup(x => x.AddNewFriendToDb(idUser, idUserFriend))
            .Throws(new ReferenceException("User1 is already user2s friend!"));

        //Act
        var actionResult = await _friendsController.AddNewFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User1 is already user2s friend!", resultValue);
    }
    
    [Fact]
    public async void AddNewFriend_With_2_Same_Ids_Returns_Bad_Request()
    {
        //Arrange
        var idUser = 1;
        var idUserFriend = 1;
        FriendDto? friendDto = null;

        _friendsRepositoryMock
            .Setup(x => x.AddNewFriendToDb(idUser, idUserFriend))
            .ReturnsAsync(friendDto);

        //Act
        var actionResult = await _friendsController.AddNewFriend(idUser, idUserFriend);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Ids must have different values!", resultValue);
    }
    
}