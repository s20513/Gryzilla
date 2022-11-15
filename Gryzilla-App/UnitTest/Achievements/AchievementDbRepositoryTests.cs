using Gryzilla_App.DTOs.Responses.Achievement;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;
using MockQueryable.Moq;

namespace UnitTest.Achievements;

public class AchievementDbRepositoryTests
{
   /*private readonly Mock<GryzillaContext>? _contextMock;
   private readonly AchievementDbRepository _repository;
   
   public AchievementDbRepositoryTests()
   {
      var achievements = new List<Achievement>
      {
         new()
         {
            IdAchievement = 1,
            Points = 10,
            Descripion = "Desc1",
            AchievementName = "Ach1"
         },
         new()
         {
            IdAchievement = 2,
            Points = 10,
            Descripion = "Desc2",
            AchievementName = "Ach2"
         }
      }.AsQueryable();

      var achievementsMock = new Mock<DbSet<Achievement>>();
      achievementsMock.As<IQueryable<Achievement>>().Setup(x => x.Provider).Returns(achievements.Provider);
      achievementsMock.As<IQueryable<Achievement>>().Setup(x => x.Expression).Returns(achievements.Expression);
      achievementsMock.As<IQueryable<Achievement>>().Setup(x => x.ElementType).Returns(achievements.ElementType);
      achievementsMock.As<IQueryable<Achievement>>().Setup(x => x.GetEnumerator()).Returns(achievements.GetEnumerator());

      _contextMock = new Mock<GryzillaContext>();
      _contextMock.Setup(x => x.Achievements).Returns(achievementsMock.Object);
   
      _repository = new AchievementDbRepository(_contextMock.Object);
   }
   
   [Fact]
   public async void GetAchievementsFromDb_Returns_2_Achievements()
   {
      //Arrange

      //Act
      var result = await _repository.GetAchievementsFromDb();
        
      //Assert
      Assert.NotNull(result);
      Assert.True(result.Count() == 2);
   }*/
   
   
   [Fact]
   public async void GetAchievementsFromDb_Returns_2_Achievements()
   {
      //Arrange
      var achievements = new List<Achievement>
      {
         new Achievement
         {
            IdAchievement = 1,
            AchievementName = "ach1",
            Descripion = "desc1",
            Points = 10
         },
         new Achievement
         {
            IdAchievement = 2,
            AchievementName = "ach2",
            Descripion = "desc2",
            Points = 20
         }
      };

      var mock = achievements.AsQueryable().BuildMockDbSet();
      //var
      

      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(mock.Object);
      //contextMock.Setup(x => x.Achievements.Add(It.IsAny<Achievement>())).Returns((Achievement u) => u);
  
      var repository = new AchievementDbRepository(contextMock.Object);
  
      // Act
      var res = await repository.GetAchievementsFromDb();
      
      //Assert
      Assert.NotNull(res);
   }





}