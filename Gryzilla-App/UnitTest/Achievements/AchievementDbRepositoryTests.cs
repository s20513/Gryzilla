using Gryzilla_App.DTOs.Responses.Achievement;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;
using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
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
         new()
         {
            IdAchievement = 1,
            AchievementName = "ach1",
            Descripion = "desc1",
            Points = 10
         },
         new()
         {
            IdAchievement = 2,
            AchievementName = "ach2",
            Descripion = "desc2",
            Points = 20
         }
      };

      var achievementDbSetMock = achievements.AsQueryable().BuildMockDbSet();
      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(achievementDbSetMock.Object);
      //contextMock.Setup(x => x.Achievements.Add(It.IsAny<Achievement>())).Returns((Achievement u) => u);

      var repository = new AchievementDbRepository(contextMock.Object);

      // Act
      var res = await repository.GetAchievementsFromDb();

      //Assert
      Assert.NotNull(res);

      if (res is null) return;
      Assert.True(res.Any());
   }

   [Fact]
   public async void GetAchievementsFromDb_Returns_Null()
   {
      //Arrange
      var achievements = new List<Achievement>();

      var achievementDbSetMock = achievements.AsQueryable().BuildMockDbSet();
      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(achievementDbSetMock.Object);

      var repository = new AchievementDbRepository(contextMock.Object);

      // Act
      var res = await repository.GetAchievementsFromDb();

      //Assert
      Assert.Null(res);
   }

   [Fact]
   public async void ModifyAchievement_Returns_Modified_Achievement()
   {
      //Arrange
      var achievements = new List<Achievement>
      {
         new()
         {
            IdAchievement = 1,
            AchievementName = "ach1",
            Descripion = "desc1",
            Points = 10
         },
         new()
         {
            IdAchievement = 2,
            AchievementName = "ach2",
            Descripion = "desc2",
            Points = 20
         }
      };

      var id = 1;
      var newPoints = 15;
      var newName = "ach1mod";
      var newDesc = "desc1mod";

      var putAchievementDto = new PutAchievementDto
      {
         IdAchievement = id,
         Points = newPoints,
         AchievementName = newName,
         Description = newDesc
      };

      var achievementDbSetMock = achievements.AsQueryable().BuildMockDbSet();
      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(achievementDbSetMock.Object);

      var repository = new AchievementDbRepository(contextMock.Object);

      // Act
      var res = await repository.ModifyAchievement(id, putAchievementDto);

      //Assert
      Assert.NotNull(res);

      if (res is null) return;
      Assert.Equal(newName, res.AchievementName);
      Assert.Equal(newDesc, res.Description);
      Assert.Equal(newPoints, res.Points);
   }
   
   [Fact]
   public async void ModifyAchievement_Returns_Null()
   {
      //Arrange
      var achievements = new List<Achievement>
      {
         new()
         {
            IdAchievement = 1,
            AchievementName = "ach1",
            Descripion = "desc1",
            Points = 10
         },
         new()
         {
            IdAchievement = 2,
            AchievementName = "ach2",
            Descripion = "desc2",
            Points = 20
         }
      };

      var id = 10;

      var putAchievementDto = new PutAchievementDto
      {
         IdAchievement = id
      };

      var achievementDbSetMock = achievements.AsQueryable().BuildMockDbSet();
      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(achievementDbSetMock.Object);

      var repository = new AchievementDbRepository(contextMock.Object);

      // Act
      var res = await repository.ModifyAchievement(id, putAchievementDto);

      //Assert
      Assert.Null(res);
   }
   



   /*[Fact]
   public async void AddNewAchievement_Returns_New_Achievement()
   {
      //Arrange
      var achievements = new List<Achievement>()
      {
         new()
         {
            IdAchievement = 1,
            AchievementName = "ach1",
            Descripion = "desc1",
            Points = 10
         },
         new()
         {
            IdAchievement = 2,
            AchievementName = "ach2",
            Descripion = "desc2",
            Points = 20
         }
      };
      
      achievements.Add(new Achievement());
      
      var newName = "newAch";
      var newDesc = "newDesc";
      var newPoints = 50;
      
      var newAchievementDto = new AddAchievementDto
      {
         AchievementName = newName,
         Description = newDesc,
         Points = newPoints
      };


      var achievementDbSetMock = achievements.AsQueryable().BuildMockDbSet();
      achievementDbSetMock.Object.Add(new Achievement());
      var t = achievementDbSetMock.Object;
      
      var contextMock = new Mock<GryzillaContext>();
      contextMock.Setup(x => x.Achievements).Returns(achievementDbSetMock.Object);
      
      
      
      /*contextMock.Setup(x => x.Achievements.Where(e => e.AchievementName == newName).Select(e => e.IdAchievement).First())
         .Returns(3);
      
      contextMock.Setup(x => x.Achievements.FindAsync(1)).ReturnsAsync((object[] ids) =>
      {
         var id = (Guid)ids[0];
         return achievements.FirstOrDefault(x => x.IdAchievement == 1);
      });#1#
      
      /*contextMock.Setup(x => x.Achievements.Select(e => e.IdAchievement)).Returns((object[] ids) =>
      {
         var id = (Guid)ids[0];
         return 3;
      });#1#
      
      
      
      
      /*contextMock.Setup(x => x.Achievements
         .Where(e => e.AchievementName == newName))
         .Returns((object[] ids) => new Achievement
      {
         IdAchievement = 3
      });#1#
      
      
      
      
      
      
      
      
      //contextMock.Setup(x => x.Achievements.Add(It.IsAny<Achievement>())).Returns((Achievement u) => u);

      var repository = new AchievementDbRepository(contextMock.Object);

      // Act
      var res = await repository.AddNewAchievement(newAchievementDto);

      //Assert
      Assert.NotNull(res);
      
      Assert.Equal(newName, res.AchievementName);
      Assert.Equal(newDesc, res.Description);
      Assert.Equal(newPoints, res.Points);
   }*/
}

