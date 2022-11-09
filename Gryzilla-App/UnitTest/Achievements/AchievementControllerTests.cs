using FakeItEasy;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Achievement;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest.Achievements;

public class AchievementControllerTests
{
    [Fact]
    public void GetAchievements_Returns_Ok()
    {
        //Arrange
        var count = 5;
        
        IEnumerable<AchievementDto> fakeAchievements = A.CollectionOfDummy<AchievementDto>(count)
                                                        .AsEnumerable()
                                                        .ToList();
        
        var repository = A.Fake<IAchievementDbRepository>();
        A.CallTo(() => repository.GetAchievementsFromDb())!.Returns(Task.FromResult(fakeAchievements));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetAchievements();
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);

        if (result is null) return;
        var resultValue = result.Value as IEnumerable<AchievementDto>;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal(fakeAchievements, resultValue);
    }
    
    [Fact]
    public void GetAchievements_Returns_Not_found()
    {
        //Arrange
        var repository = A.Fake<IAchievementDbRepository>();
        
        A.CallTo(() => repository.GetAchievementsFromDb())
            .Returns(Task.FromResult<IEnumerable<AchievementDto>?>(null));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetAchievements();
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Not found any achievements", resultValue);
    }

    [Fact]
    public void ModifyAchievement_Returns_Ok()
    {
        //Arrange
        var id = 5;
        
        var achievement = A.Dummy<PutAchievementDto>();
        achievement.IdAchievement = id;
        
        var modifiedAchievement = A.Dummy<AchievementDto>();
        modifiedAchievement.Description = "test";
        
        var repository = A.Fake<IAchievementDbRepository>();
        
        A.CallTo(() => repository.ModifyAchievement(id, achievement))!
            .Returns(Task.FromResult(modifiedAchievement));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.ModifyAchievement(id, achievement);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
                
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        Assert.NotNull(resultValue);
        
        if (resultValue is null) return;
        Assert.Equal("test", resultValue.Description);
    }
    
    [Fact]
    public void ModifyAchievement_Returns_Bad_Request()
    {
        //Arrange
        var id = 5;
        var achievement = A.Dummy<PutAchievementDto>();
        achievement.IdAchievement = 6;
        
        var repository = A.Fake<IAchievementDbRepository>();
        var modifiedAchievement = A.Dummy<AchievementDto>();
        modifiedAchievement.Description = "test";
        
        A.CallTo(() => repository.ModifyAchievement(id, achievement))!
            .Returns(Task.FromResult(modifiedAchievement));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.ModifyAchievement(id, achievement);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Id from route and Id in body have to be same", resultValue);
    }
    
    [Fact]
    public void ModifyAchievement_Returns_Not_Found()
    {
        //Arrange
        var id = 5;
        var achievement = A.Dummy<PutAchievementDto>();
        achievement.IdAchievement = id;
        
        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.ModifyAchievement(id, achievement))
            .Returns(Task.FromResult<AchievementDto?>(null));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.ModifyAchievement(id, achievement);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Achievement not found", resultValue);
    }
    
    [Fact]
    public void AddNewAchievement_Returns_Not_Found()
    {
        //Arrange
        var newAchievement = A.Dummy<AddAchievementDto>();
        var returnAchievement = A.Dummy<AchievementDto>();

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.AddNewAchievement(newAchievement))
            .Returns(Task.FromResult(returnAchievement));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.AddNewAchievement(newAchievement);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        
        Assert.Equal(returnAchievement, resultValue);
    }
    
    [Fact]
    public void AddNewAchievement_Returns_Bad_Request()
    {
        //Arrange
        var exceptionMessage = "Achievement with given name already exists!";
        var newAchievement = A.Dummy<AddAchievementDto>();

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.AddNewAchievement(newAchievement))
            .Throws(new SameNameException(exceptionMessage));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.AddNewAchievement(newAchievement);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal(exceptionMessage, resultValue);
    }
    
    [Fact]
    public void DeleteAchievement_Returns_Ok()
    {
        //Arrange
        var id = 5;
        var deletedAchievement = A.Dummy<AchievementDto>();

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.DeleteAchievement(id))!
            .Returns(Task.FromResult(deletedAchievement));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.DeleteAchievement(id);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        
        Assert.Equal(deletedAchievement, resultValue);
    }
    
    [Fact]
    public void DeleteAchievement_Returns_Not_Found()
    {
        //Arrange
        var id = 5;

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.DeleteAchievement(id))
            .Returns(Task.FromResult<AchievementDto?>(null));
        
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.DeleteAchievement(id);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Achievement not found", resultValue);
    }
    
    [Fact]
    public void DeleteAchievement_Returns_Bad_Request()
    {
        //Arrange
        var id = 5;
        var exceptionMessage = "Cannot delete. Some user have this achievement!";

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.DeleteAchievement(id))
            .Throws(new ReferenceException(exceptionMessage));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.DeleteAchievement(id);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal(exceptionMessage, resultValue);
    }
    
    [Fact]
    public void AddNewUserAchievement_Returns_Ok()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;
        
        var achievement = A.Dummy<AchievementDto>();

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.AddNewUserAchievement(idAchievement, idUser))!
            .Returns(Task.FromResult(achievement));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.AddNewUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        
        Assert.Equal(achievement, resultValue);
    }
    
    [Fact]
    public void AddNewUserAchievement_Returns_Not_Found()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.AddNewUserAchievement(idAchievement, idUser))
            .Returns(Task.FromResult<AchievementDto?>(null));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.AddNewUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Cannot find user or achievement", resultValue);
    }
    
    [Fact]
    public void AddNewUserAchievement_Returns_Bad_Request()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;
        var exceptionMessage = "User already has the achievement!";

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.AddNewUserAchievement(idAchievement, idUser))
            .Throws(new ReferenceException(exceptionMessage));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.AddNewUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal(exceptionMessage, resultValue);
    }
    
    [Fact]
    public void DeleteUserAchievement_Returns_Ok()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;
        
        var achievement = A.Dummy<AchievementDto>();

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.DeleteUserAchievement(idAchievement, idUser))!
            .Returns(Task.FromResult(achievement));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.DeleteUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as AchievementDto;
        
        Assert.Equal(achievement, resultValue);
    }
    
    [Fact]
    public void DeleteUserAchievement_Returns_Not_Found()
    {
        //Arrange
        var idAchievement = 1;
        var idUser = 1;

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.DeleteUserAchievement(idAchievement, idUser))
            .Returns(Task.FromResult<AchievementDto?>(null));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.DeleteUserAchievement(idAchievement, idUser);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("Cannot delete achievement from user", resultValue);
    }
    
    [Fact]
    public void GetUserAchievements_Returns_Ok()
    {
        //Arrange
        var idUser = 1;
        var count = 5;
        
        IEnumerable<AchievementDto> fakeAchievements = A.CollectionOfDummy<AchievementDto>(count)
                                                        .AsEnumerable()
                                                        .ToList();

        var repository = A.Fake<IAchievementDbRepository>();
        
        A.CallTo(() => repository.GetUserAchievements(idUser))!
            .Returns(Task.FromResult(fakeAchievements));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetUserAchievements(idUser);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as IEnumerable<AchievementDto>;
        
        Assert.Equal(fakeAchievements, resultValue);
    }
    
    [Fact]
    public void GetUserAchievements_Returns_Not_Found()
    {
        //Arrange
        var idUser = 1;

        var repository = A.Fake<IAchievementDbRepository>();

        A.CallTo(() => repository.GetUserAchievements(idUser))
            .Returns(Task.FromResult<IEnumerable<AchievementDto>?>(null));

        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetUserAchievements(idUser);
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        Assert.NotNull(result);
        
        if (result is null) return;
        var resultValue = result.Value as string;
        
        Assert.Equal("User not found", resultValue);
    }
}