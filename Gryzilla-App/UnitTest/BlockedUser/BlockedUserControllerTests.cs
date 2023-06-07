using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.BlockedUser;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.BlockedUser;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.BlockedUser;

public class BlockedUserControllerTests
{
    private readonly BlockedUserController _blockedUserController;
    private readonly Mock<IBlockedUserDbRepository> _blockedUserRepositoryMock = new();

    public BlockedUserControllerTests()
    {
        _blockedUserController = new BlockedUserController(_blockedUserRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetBlockedUsers_Returns_Ok()
    {
        //Arrange
        var blockedUsersDtos = new List<BlockedUserDto>();
        
        _blockedUserRepositoryMock
            .Setup(x => x.GetBlockedUsers())
            .ReturnsAsync(blockedUsersDtos);

        //Act
        var actionResult = await _blockedUserController.GetBlockedUsers();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as List<BlockedUserDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(blockedUsersDtos, resultValue);
    }

    [Fact]
    public async void BlockUser_Returns_Ok()
    {
        //Arrange
        var blockedUserRequestDto = new BlockedUserRequestDto();
        var blockedUsersDto = new BlockedUserDto();
        
        _blockedUserRepositoryMock
            .Setup(x => x.BlockUser(blockedUserRequestDto))
            .ReturnsAsync(blockedUsersDto);

        //Act
        var actionResult = await _blockedUserController.BlockUser(blockedUserRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as BlockedUserDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(blockedUsersDto, resultValue);
    }
    
    [Fact]
    public async void BlockUser_Returns_NotFound()
    {
        //Arrange
        var blockedUserRequestDto = new BlockedUserRequestDto();
        BlockedUserDto? nullValue = null;
        
        _blockedUserRepositoryMock
            .Setup(x => x.BlockUser(blockedUserRequestDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _blockedUserController.BlockUser(blockedUserRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("One of the users does not exist", resultValue.Message);
    }

    [Fact]
    public async void UnlockUser_Returns_Ok()
    {
        //Arrange
        const int idUser = 1;
        const string info = "User unlocked";
        
        _blockedUserRepositoryMock
            .Setup(x => x.UnlockUser(idUser))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _blockedUserController.UnlockUser(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(info, resultValue.Message);
    }
    
    [Fact]
    public async void UnlockUser_Returns_NotFound()
    {
        //Arrange
        const int idUser = 1;
        const string? info = null;
        
        _blockedUserRepositoryMock
            .Setup(x => x.UnlockUser(idUser))
            .ReturnsAsync(info);

        //Act
        var actionResult = await _blockedUserController.UnlockUser(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Users does not exist", resultValue.Message);
    }
    
    [Fact]
    public async void GetUserBlockingHistory_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var userBlockingHistory = new UserBlockingHistoryDto();

        _blockedUserRepositoryMock
            .Setup(x => x.GetUserBlockingHistory(idUser))
            .ReturnsAsync(userBlockingHistory);

        //Act
        var actionResult = await _blockedUserController.GetUserBlockingHistory(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as UserBlockingHistoryDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(userBlockingHistory, resultValue);
    }
    
    [Fact]
    public async void GetUserBlockingHistory_Returns_NotFound()
    {
        //Arrange
        var idUser = 1;
        UserBlockingHistoryDto? userBlockingHistory = null;

        _blockedUserRepositoryMock
            .Setup(x => x.GetUserBlockingHistory(idUser))
            .ReturnsAsync(userBlockingHistory);

        //Act
        var actionResult = await _blockedUserController.GetUserBlockingHistory(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Users does not exist", resultValue.Message);
    }
    
    [Fact]
    public async void CheckIfUserIsBlocked_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var blocked = true;

        _blockedUserRepositoryMock
            .Setup(x => x.CheckIfUserIsBlocked(idUser))
            .ReturnsAsync(blocked);

        //Act
        var actionResult = await _blockedUserController.CheckIfUserIsBlocked(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as BoolMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(blocked, resultValue.Message);
    }
    
    [Fact]
    public async void CheckIfUserIsBlocked_Returns_NotFound()
    {
        //Arrange
        var idUser = 1;
        bool? blocked = null;

        _blockedUserRepositoryMock
            .Setup(x => x.CheckIfUserIsBlocked(idUser))
            .ReturnsAsync(blocked);

        //Act
        var actionResult = await _blockedUserController.CheckIfUserIsBlocked(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Users does not exist", resultValue.Message);
    }
}