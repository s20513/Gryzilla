using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Achievement;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Achievements;

public class AchievementControllerTests
{
    private readonly AchievementController _achievementController;
    private readonly Mock<IAchievementDbRepository> _achievementRepositoryMock = new();

    private List<AchievementDto> _fakeAchievements = new List<AchievementDto>
    {
        new()
        {
            IdAchievement = 1,
            AchievementName = "Ach1",
            Description = "Desc1",
            Points = 20
        },
        new()
        {
            IdAchievement = 2,
            AchievementName = "Ach2",
            Description = "Desc2",
            Points = 30
        },
        new()
        {
            IdAchievement = 3,
            AchievementName = "Ach3",
            Description = "Desc3",
            Points = 40
        }
    };

    public AchievementControllerTests()
    {
        _achievementController = new AchievementController(_achievementRepositoryMock.Object);
    }
    

    [Fact]
    public async void GetAchievements_Returns_Ok()
    {
        //Arrange
        _achievementRepositoryMock
            .Setup(x => x.GetAchievementsFromDb())
            .ReturnsAsync(_fakeAchievements);
        
        //Act
        var actionResult = await _achievementController.GetAchievements();
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<AchievementDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeAchievements, resultValue);
    }

    [Fact]
    public async void GetAchievements_Returns_Not_found()
    {
        //Arrange
        IEnumerable<AchievementDto>? nullValue = null;
        
        _achievementRepositoryMock
            .Setup(x => x.GetAchievementsFromDb())
            .ReturnsAsync(nullValue);
        
        //Act
        var actionResult = await _achievementController.GetAchievements();
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Not found any achievements", resultValue);
    }

    [Fact]
    public async void ModifyAchievement_Returns_Ok()
    {
        //Arrange
        var id = 10;
        var putAchievementDto = new PutAchievementDto
        {
            IdAchievement = id
        };

        var returnedAchievement = new AchievementDto
        {
            IdAchievement = id
        };

        _achievementRepositoryMock
            .Setup(x => x.ModifyAchievement(id, putAchievementDto))
            .ReturnsAsync(returnedAchievement);
        
        //Act
        var actionResult = await _achievementController.ModifyAchievement(id, putAchievementDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdAchievement);
    }
    
    [Fact]
    public async void ModifyAchievement_Returns_Bad_Request()
    {
        //Arrange
        var id = 10;
        var putAchievementDto = new PutAchievementDto
        {
            IdAchievement = 11
        };

        //Act
        var actionResult = await _achievementController.ModifyAchievement(id, putAchievementDto);
        
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
    public async void ModifyAchievement_Returns_Not_Found()
    {
        //Arrange
        var id = 10;
        var putAchievementDto = new PutAchievementDto
        {
            IdAchievement = id
        };

        AchievementDto? nullValue = null;

        _achievementRepositoryMock
            .Setup(x => x.ModifyAchievement(id, putAchievementDto))
            .ReturnsAsync(nullValue);
        
        //Act
        var actionResult = await _achievementController.ModifyAchievement(id, putAchievementDto);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Achievement not found", resultValue);
    }

    [Fact]
    public async void AddNewAchievement_Returns_Ok()
    {
        //Arrange
        var id = 10;
        var addAchievementDto = new AddAchievementDto();

        var returnedAchievement = new AchievementDto
        {
            IdAchievement = id
        };

        _achievementRepositoryMock
            .Setup(x => x.AddNewAchievement(addAchievementDto))
            .ReturnsAsync(returnedAchievement);
        
        //Act
        var actionResult = await _achievementController.AddNewAchievement(addAchievementDto);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdAchievement);
    }

    [Fact]
    public async void AddNewAchievement_Returns_Bad_Request()
    {
        //Arrange
        var addAchievementDto = new AddAchievementDto();
        var exceptionMessage = "Achievement with given name already exists!";

        _achievementRepositoryMock
            .Setup(x => x.AddNewAchievement(addAchievementDto))
            .ThrowsAsync(new SameNameException(exceptionMessage));
        
        //Act
        var actionResult = await _achievementController.AddNewAchievement(addAchievementDto);
        
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
    public async void DeleteAchievement_Returns_Ok()
    {
        //Arrange
        var id = 10;

        var returnedAchievement = new AchievementDto
        {
            IdAchievement = id
        };

        _achievementRepositoryMock
            .Setup(x => x.DeleteAchievement(id))
            .ReturnsAsync(returnedAchievement);
        
        //Act
        var actionResult = await _achievementController.DeleteAchievement(id);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(id, resultValue.IdAchievement);
    }
    
    [Fact]
    public async void DeleteAchievement_Returns_Not_Found()
    {
        //Arrange
        var id = 10;

        AchievementDto? nullValue = null;

        _achievementRepositoryMock
            .Setup(x => x.DeleteAchievement(id))
            .ReturnsAsync(nullValue);
        
        //Act
        var actionResult = await _achievementController.DeleteAchievement(id);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Achievement not found", resultValue);
    }
    
    [Fact]
    public async void DeleteAchievement_Returns_Bad_Request()
    {
        //Arrange
        var id = 10;
        var exceptionMessage = "Cannot delete. Some user have this achievement!";

        _achievementRepositoryMock
            .Setup(x => x.DeleteAchievement(id))
            .ThrowsAsync(new ReferenceException(exceptionMessage));
        
        //Act
        var actionResult = await _achievementController.DeleteAchievement(id);
        
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
    public async void AddNewUserAchievement_Returns_Ok()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        var returnedAchievement = new AchievementDto
        {
            IdAchievement = idAchievement
        };

        _achievementRepositoryMock
            .Setup(x => x.AddNewUserAchievement(idAchievement, idUser))
            .ReturnsAsync(returnedAchievement);
        
        //Act
        var actionResult = await _achievementController.AddNewUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idAchievement, resultValue.IdAchievement);
    }
    
    [Fact]
    public async void AddNewUserAchievement_Returns_Not_Found()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        AchievementDto? nullvalue = null;

        _achievementRepositoryMock
            .Setup(x => x.AddNewUserAchievement(idAchievement, idUser))
            .ReturnsAsync(nullvalue);
        
        //Act
        var actionResult = await _achievementController.AddNewUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot find user or achievement", resultValue);
    }
    
    [Fact]
    public async void AddNewUserAchievement_Returns_Bad_Request()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;
        var exceptionMessage = "User already has the achievement!";
        

        _achievementRepositoryMock
            .Setup(x => x.AddNewUserAchievement(idAchievement, idUser))
            .ThrowsAsync(new ReferenceException(exceptionMessage));
        
        //Act
        var actionResult = await _achievementController.AddNewUserAchievement(idAchievement, idUser);
        
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
    public async void DeleteUserAchievement_Returns_Ok()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        var returnedAchievement = new AchievementDto
        {
            IdAchievement = idAchievement
        };

        _achievementRepositoryMock
            .Setup(x => x.DeleteUserAchievement(idAchievement, idUser))
            .ReturnsAsync(returnedAchievement);
        
        //Act
        var actionResult = await _achievementController.DeleteUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(idAchievement, resultValue.IdAchievement);
    }
    
    [Fact]
    public async void DeleteUserAchievement_Returns_Not_Found()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        AchievementDto? nullvalue = null;

        _achievementRepositoryMock
            .Setup(x => x.DeleteUserAchievement(idAchievement, idUser))
            .ReturnsAsync(nullvalue);
        
        //Act
        var actionResult = await _achievementController.DeleteUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("Cannot delete achievement from user", resultValue);
    }
    
    [Fact]
    public async void GetUserAchievements_Returns_Ok()
    {
        //Arrange
        var idUser = 1;

        _achievementRepositoryMock
            .Setup(x => x.GetUserAchievements(idUser))
            .ReturnsAsync(_fakeAchievements);
        
        //Act
        var actionResult = await _achievementController.GetUserAchievements(idUser);
        
        //Assert
        var result = actionResult as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as IEnumerable<AchievementDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(_fakeAchievements, resultValue.ToList());
    }
    
    [Fact]
    public async void GetUserAchievements_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;
        IEnumerable<AchievementDto>? nullValue = null;

        _achievementRepositoryMock
            .Setup(x => x.GetUserAchievements(idUser))
            .ReturnsAsync(nullValue);
        
        //Act
        var actionResult = await _achievementController.GetUserAchievements(idUser);
        
        //Assert
        var result = actionResult as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("User not found", resultValue);
    }
}