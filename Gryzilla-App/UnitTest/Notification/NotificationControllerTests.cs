using Gryzilla_App.Controllers;
using Gryzilla_App.DTOs.Requests.Notification;
using Gryzilla_App.DTOs.Responses.Notification;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Notification;

public class NotificationControllerTests
{
    private readonly NotificationController _notificationController;
    private readonly Mock<INotificationDbRepository> _notificationRepositoryMock = new();

    public NotificationControllerTests()
    {
        _notificationController = new NotificationController(_notificationRepositoryMock.Object);
    }
    
     [Fact]
    public async void CreateNotification_Returns_Ok()
    {
        //Arrange
        var newNotificationDto = new NewNotificationDto();
        var returnedNotification = new NotificationDto();
        
        _notificationRepositoryMock
            .Setup(x => x.AddNotificationFromDb(newNotificationDto))
            .ReturnsAsync(returnedNotification);

        //Act
        var actionResult = await _notificationController.AddNotification(newNotificationDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as NotificationDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedNotification, resultValue);
    }

    [Fact]
    public async void CreateNotification_Returns_Not_Found()
    {
        //Arrange
        var newNotificationDto = new NewNotificationDto();
        NotificationDto? nullValue = null;
        
        _notificationRepositoryMock
            .Setup(x => x.AddNotificationFromDb(newNotificationDto))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _notificationController.AddNotification(newNotificationDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No user with given id found", resultValue);
    }
    
    [Fact]
    public async void ModifyNotification_Returns_Ok()
    {
        //Arrange
        var idNotification = 1;
        var putNotification = new ModifyNotificationDto
        {
            Content = "Content"
        };
        
        var returnedComment = new NotificationDto();
        
        _notificationRepositoryMock
            .Setup(x => x.ModifyNotificationFromDb(idNotification, putNotification))
            .ReturnsAsync(returnedComment);

        //Act
        var actionResult = await _notificationController.ModifyNotification(idNotification, putNotification);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as NotificationDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedComment, resultValue);
    }
    
    [Fact]
    public async void ModifyNotification_Returns_Not_Found()
    {
        //Arrange
        var idNotification = 1;
        var putNotification = new ModifyNotificationDto
        {
            Content = "Content"
        };
        NotificationDto? nullValue = null;
        
        _notificationRepositoryMock
            .Setup(x => x.ModifyNotificationFromDb(idNotification, putNotification))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _notificationController.ModifyNotification(idNotification, putNotification);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No notification with given id found", resultValue);
    }
    
    [Fact]
    public async void DeleteNotification_Returns_Ok()
    {
        //Arrange
        var idNotification = 1;
        var returnedNotification = new NotificationDto();
        
        _notificationRepositoryMock
            .Setup(x => x.DeleteNotificationFromDb(idNotification))
            .ReturnsAsync(returnedNotification);

        //Act
        var actionResult = await _notificationController.DeleteNotification(1);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as NotificationDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(returnedNotification, resultValue);
    }
    
    [Fact]
    public async void DeleteProfileComment_Returns_Not_Found()
    {
        //Arrange
        NotificationDto? nullValue = null;
        var idNotification = 1;
        
        _notificationRepositoryMock
            .Setup(x => x.DeleteNotificationFromDb(idNotification))
            .ReturnsAsync(nullValue);

        //Act
        var actionResult = await _notificationController.DeleteNotification(1);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No notification with given id found", resultValue);
    }
}