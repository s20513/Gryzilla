using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Reason;
using Gryzilla_App.DTOs.Responses;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Reason;

public class ReasonControllerTests
{
    private readonly ReasonController _reasonController;
    private readonly Mock<IReasonDbRepository> _reasonRepositoryMock = new();

    public ReasonControllerTests()
    {
        _reasonController = new ReasonController(_reasonRepositoryMock.Object);
    }

    [Fact]
    public async void GetReasons_Returns_Ok()
    {
        //Arrange
        var reasons = new List<FullTagDto>();

        _reasonRepositoryMock.Setup(e => e.GetReasonsFromDb()).ReturnsAsync(reasons);
        
        //Act
        var actionResult = await _reasonController.GetReasons();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as List<FullTagDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reasons, resultValue);
    }
    
    [Fact]
    public async void GetReason_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var reason = new FullTagDto
        {
            Id = id
        };

        _reasonRepositoryMock.Setup(e => e.GetReasonFromDb(id)).ReturnsAsync(reason);
        
        //Act
        var actionResult = await _reasonController.GetReason(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reason, resultValue);
    }
    
    [Fact]
    public async void GetReason_Returns_NotFound()
    {
        //Arrange
        var id = 1;
        FullTagDto? reason = null;

        _reasonRepositoryMock.Setup(e => e.GetReasonFromDb(id)).ReturnsAsync(reason);
        
        //Act
        var actionResult = await _reasonController.GetReason(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No reason with given id found", resultValue.Message);
    }
    
    [Fact]
    public async void AddReason_Returns_Ok()
    {
        //Arrange
        var newReason = new NewReasonDto();
        var reason = new FullTagDto();

        _reasonRepositoryMock.Setup(e => e.AddReasonToDb(newReason)).ReturnsAsync(reason);
        
        //Act
        var actionResult = await _reasonController.AddReason(newReason);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reason, resultValue);
    }
    
    [Fact]
    public async void AddReason_Returns_BadRequest()
    {
        //Arrange
        var newReason = new NewReasonDto();
        var message = "Reason with given name already exists!";

        _reasonRepositoryMock
            .Setup(e => e.AddReasonToDb(newReason))
            .ThrowsAsync(new SameNameException("Reason with given name already exists!"));
        
        //Act
        var actionResult = await _reasonController.AddReason(newReason);
        
        //Assert
        var result = actionResult as BadRequestObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(message, resultValue.Message);
    }
    
    [Fact]
    public async void DeleteReason_Returns_Ok()
    {
        //Arrange
        var id = 1;
        var reason = new FullTagDto
        {
            Id = id
        };

        _reasonRepositoryMock.Setup(e => e.DeleteReasonFromDb(id)).ReturnsAsync(reason);
        
        //Act
        var actionResult = await _reasonController.DeleteReason(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as FullTagDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(reason, resultValue);
    }
    
    [Fact]
    public async void DeleteReason_Returns_NotFound()
    {
        //Arrange
        var id = 1;
        FullTagDto? reason = null;

        _reasonRepositoryMock.Setup(e => e.DeleteReasonFromDb(id)).ReturnsAsync(reason);
        
        //Act
        var actionResult = await _reasonController.DeleteReason(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as StringMessageDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("No reason with given id found", resultValue.Message);
    }

}