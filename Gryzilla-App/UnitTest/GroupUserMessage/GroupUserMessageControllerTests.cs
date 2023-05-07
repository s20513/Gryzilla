using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.DTOs.Responses.GroupUserMessageDto;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.GroupUserMessage;

public class GroupUserMessageControllerTests
{
    private readonly GroupUserMessageController _groupsController;
    private readonly Mock<IGroupUserMessageDbRepository> _groupRepositoryMock = new();

    public GroupUserMessageControllerTests()
    {
        _groupsController = new GroupUserMessageController(_groupRepositoryMock.Object);
    }
    
    [Fact]
    public async void GetMessages_Returns_Ok()
    {
        //Arrange
        int idGroup = 1;
        var messages = new GroupUserMessageDto[5];
        _groupRepositoryMock.Setup(x => x.GetMessages(idGroup)).ReturnsAsync(messages);

        //Act
        var actionResult = await _groupsController.GetMessages(idGroup);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupUserMessageDto[];
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(messages, resultValue);
    }
    
    [Fact]
    public async void GetMessages_Returns_Not_Found()
    {
        //Arrange
        int idGroup = 1;
        var messages = Array.Empty<GroupUserMessageDto>();
        
        _groupRepositoryMock.Setup(x => x.GetMessages(idGroup)).ReturnsAsync(messages);

        //Act
        var actionResult = await _groupsController.GetMessages(idGroup);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no messages for this group", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyMessage_Returns_Ok()
    {
        //Arrange
        var idMessage = 1;
        var messageRequestDto = new UpdateGroupUserMessageDto
        {
            IdMessage = 1,
            Content   = "Nowa testowa"
        };
        
        var message = new GroupUserMessageDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyMessage(idMessage, messageRequestDto)).ReturnsAsync(message);

        //Act
        var actionResult = await _groupsController.ModifyMessage(idMessage, messageRequestDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupUserMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue);
    }
    
    [Fact]
    public async void ModifyMessage_Returns_Not_Found()
    {
        //Arrange
        var idMessage = 1;
        var messageRequestDto = new UpdateGroupUserMessageDto
        {
            IdMessage = 1,
            Content   = "Nowa testowa"
        };
        
        GroupUserMessageDto? nullValue = null;
        
        _groupRepositoryMock.Setup(x => x.ModifyMessage(idMessage, messageRequestDto)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.ModifyMessage(idMessage, messageRequestDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no message with given id", resultValue.Message);
    }
    
    [Fact]
    public async void ModifyMessage_With_Different_Ids_Returns_Bad_Request()
    {
        //Arrange
        var idMessage = 1;
        var messageRequestDto = new UpdateGroupUserMessageDto
        {
            IdMessage = 2,
            Content   = "Nowa testowa"
        };
        var message = new GroupUserMessageDto();
        
        _groupRepositoryMock.Setup(x => x.ModifyMessage(idMessage, messageRequestDto)).ReturnsAsync(message);

        //Act
        var actionResult = await _groupsController.ModifyMessage(idMessage, messageRequestDto);

        
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
    public async void DeleteMessage_Returns_Ok()
    {
        //Arrange
        var idMessage = 1;
        
        var message = new GroupUserMessageDto();
        
        _groupRepositoryMock.Setup(x => x.DeleteMessage(idMessage)).ReturnsAsync(message);

        //Act
        var actionResult = await _groupsController.DeleteMessage(idMessage);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupUserMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue);
    }
    
    [Fact]
    public async void DeleteMessage_Not_Found()
    {
        //Arrange
        var idMessage = 1;
        GroupUserMessageDto? nullValue = null;
        
        _groupRepositoryMock.Setup(x => x.DeleteMessage(idMessage)).ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.DeleteMessage(idMessage);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("There is no message with given id", resultValue.Message);
    }
    
    [Fact]
    public async void CreateMessage_Returns_Ok()
    {
        //Arrange
        var newGroupUserMessageDto = new AddGroupUserMessageDto();
        var message = new GroupUserMessageDto();
        
        _groupRepositoryMock
            .Setup(x => x.AddMessage(newGroupUserMessageDto))
            .ReturnsAsync(message);

        //Act
        var actionResult = await _groupsController.CreateNewMessage(newGroupUserMessageDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as GroupUserMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue);
    }
    
    [Fact]
    public async void CreateMessage_Returns_Not_Found()
    {
        //Arrange
        var newGroupUserMessageDto = new AddGroupUserMessageDto();
        GroupUserMessageDto? nullValue = null;
        
        _groupRepositoryMock
            .Setup(x => x.AddMessage(newGroupUserMessageDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _groupsController.CreateNewMessage(newGroupUserMessageDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Wrong userId or groupId", resultValue.Message);
    }

}