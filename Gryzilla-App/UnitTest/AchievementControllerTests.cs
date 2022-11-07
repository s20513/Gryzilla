using FakeItEasy;
using Gryzilla_App.Controllers;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Achievement;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest;

public class AchievementControllerTests
{
    [Fact]
    public void GetAchievements_Returns_Correct_Number_Of_Achievements()
    {
        //Arrange
        var count = 5;
        var fakeAchievements = A.CollectionOfDummy<AchievementDto>(count).AsEnumerable();
        var repository = A.Fake<IAchievementDbRepository>();
        A.CallTo(() => repository.GetAchievementsFromDb())!.Returns(Task.FromResult(fakeAchievements));
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetAchievements();
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        var returnAchievements = result.Value as IEnumerable<AchievementDto>;
        
        Assert.Equal(count, returnAchievements.Count());

    }
    
    [Fact]
    public void GetAchievements_Returns_Null()
    {
        //Arrange
        var repository = A.Fake<IAchievementDbRepository>();
        A.CallTo(() => repository.GetAchievementsFromDb()).Returns(Task.FromResult<IEnumerable<AchievementDto>?>(null));
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.GetAchievements();
        
        //Assert
        var result = actionResult.Result as NotFoundObjectResult;
        var returnAchievements = result.Value as string;
        
        
        Assert.Equal("Not found any achievements", returnAchievements);

    }

    [Fact]
    public void ModifyAchievement_Returns_Modified_Achievement()
    {
        //Arrange
        //var achievement = A.CollectionOfDummy<AchievementDto>(count).AsEnumerable();
        var id = 5;
        var achievement = A.Dummy<PutAchievementDto>();
        achievement.IdAchievement = 5;
        var repository = A.Fake<IAchievementDbRepository>();
        var modifiedAchievement = A.Dummy<AchievementDto>();
        modifiedAchievement.Description = "test";
        A.CallTo(() => repository.ModifyAchievement(id, achievement)).Returns(Task.FromResult(modifiedAchievement));
        var controller = new AchievementController(repository);
        
        //Act
        var actionResult = controller.ModifyAchievement(id, achievement);
        
        //Assert
        var result = actionResult.Result as OkObjectResult;
        var returnAchievements = result.Value as AchievementDto;
        
        Assert.Equal("test", returnAchievements.Description);
    }
}