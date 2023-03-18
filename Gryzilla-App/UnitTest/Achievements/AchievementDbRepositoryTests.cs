using Gryzilla_App;
using Gryzilla_App.DTO.Requests;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Achievements;

public class AchievementDbRepositoryTests
{
   private readonly GryzillaContext _context;
   private readonly AchievementDbRepository _repository;

   public AchievementDbRepositoryTests()
   {
      var options = new DbContextOptions<GryzillaContext>();

      _context = new GryzillaContext(options, true);
      _repository = new AchievementDbRepository(_context);
   }

   private async Task AddTestData()
   {
      await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank()
      {
         Name = "Rank1",
         RankLevel = 1
      });
      await _context.SaveChangesAsync();

      await _context.UserData.AddAsync(new UserDatum
      {
         IdRank = 1,
         Nick = "Nick1",
         Password = "Pass1",
         Email = "email1",
         CreatedAt = DateTime.Today
      });

      await _context.UserData.AddAsync(new UserDatum
      {
         IdRank = 1,
         Nick = "Nick2",
         Password = "Pass2",
         Email = "email2",
         CreatedAt = DateTime.Today
      });
      await _context.SaveChangesAsync();

      await _context.Achievements.AddAsync(new Achievement
      {
         Points = 10,
         Descripion = "Desc1",
         AchievementName = "AchName1"
      });

      await _context.Achievements.AddAsync(new Achievement
      {
         Points = 20,
         Descripion = "Desc2",
         AchievementName = "AchName2"
      });

      await _context.Achievements.AddAsync(new Achievement
      {
         Points = 30,
         Descripion = "Desc3",
         AchievementName = "AchName3"
      });

      await _context.SaveChangesAsync();

      await _context.AchievementUsers.AddAsync(new AchievementUser
      {
         IdUser = 1,
         IdAchievement = 1,
         ReceivedAt = DateTime.Now
      });

      await _context.AchievementUsers.AddAsync(new AchievementUser
      {
         IdUser = 1,
         IdAchievement = 2,
         ReceivedAt = DateTime.Now
      });

      await _context.AchievementUsers.AddAsync(new AchievementUser
      {
         IdUser = 2,
         IdAchievement = 2,
         ReceivedAt = DateTime.Now
      });

      await _context.SaveChangesAsync();
   }

   [Fact]
   public async Task GetAchievementsFromDb_Returns_IEnumerable()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      //Act
      var res = await _repository.GetAchievementsFromDb();

      //Assert
      Assert.NotNull(res);

      var achievements = await _context.Achievements.Select(e => e.IdAchievement).ToListAsync();
      Assert.Equal(achievements, res.Select(e => e.IdAchievement));
   }

   [Fact]
   public async Task GetAchievementsFromDb_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      //Act
      var res = await _repository.GetAchievementsFromDb();

      //Assert
      Assert.Null(res);
   }

   [Fact]
   public async Task ModifyAchievement_Returns_AchievementDto()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var id = 1;

      var putAchievementDto = new PutAchievementDto
      {
         IdAchievement = id,
         AchievementName = "NewAchName",
         Content = "NewDesc",
         Points = 40
      };

      //Act
      var res = await _repository.ModifyAchievement(id, putAchievementDto);

      //Assert
      Assert.NotNull(res);

      var achievement = await _context.Achievements
         .Where(e => e.IdAchievement == id
                     && e.AchievementName == res.AchievementName
                     && e.Descripion == res.Content
                     && e.Points == res.Points)
         .SingleOrDefaultAsync();

      Assert.NotNull(achievement);
   }

   [Fact]
   public async Task ModifyAchievement_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      var id = 1;

      var putAchievementDto = new PutAchievementDto
      {
         IdAchievement = id,
         AchievementName = "NewAchName",
         Content = "NewDesc",
         Points = 40
      };

      //Act
      var res = await _repository.ModifyAchievement(id, putAchievementDto);

      //Assert
      Assert.Null(res);
   }

   [Fact]
   public async Task AddNewAchievement_Returns_AchievementDto()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var addAchievementDto = new AddAchievementDto
      {
         AchievementName = "NewAchName",
         Content = "NewDesc",
         Points = 40
      };

      //Act
      var res = await _repository.AddNewAchievement(addAchievementDto);

      //Assert
      Assert.NotNull(res);

      var achievement = await _context.Achievements
         .Where(e =>
            e.IdAchievement == res.IdAchievement
            && e.AchievementName == res.AchievementName
            && e.Descripion == res.Content
            && e.Points == res.Points)
         .SingleOrDefaultAsync();

      Assert.NotNull(achievement);
   }

   [Fact]
   public async Task AddNewAchievement_Throws_SameNameException()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var addAchievementDto = new AddAchievementDto
      {
         AchievementName = "AchName1",
         Content = "NewDesc",
         Points = 40
      };

      //Act
      //Assert

      await Assert.ThrowsAsync<SameNameException>(() => _repository.AddNewAchievement(addAchievementDto));
   }

   [Fact]
   public async Task DeleteAchievement_Returns_AchievementDto()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var id = 3;

      //Act
      var res = await _repository.DeleteAchievement(id);

      //Assert
      Assert.NotNull(res);

      var achievement = await _context.Achievements
         .Where(e =>
            e.IdAchievement == res.IdAchievement
            && e.AchievementName == res.AchievementName
            && e.Descripion == res.Content
            && e.Points == res.Points)
         .SingleOrDefaultAsync();

      Assert.Null(achievement);
   }

   [Fact]
   public async Task DeleteAchievement_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var id = 4;

      //Act
      var res = await _repository.DeleteAchievement(id);

      //Assert
      Assert.Null(res);
   }

   [Fact]
   public async Task DeleteAchievement_Throws_ReferenceException()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var id = 1;

      //Act

      //Assert
      await Assert.ThrowsAsync<ReferenceException>(() => _repository.DeleteAchievement(id));
   }
   
   [Fact]
   public async Task DeleteUserAchievement_Returns_AchievementDto()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var idAchievement = 1;
      var idUser = 1;

      //Act
      var res = await _repository.DeleteUserAchievement(idAchievement, idUser);

      //Assert
      Assert.NotNull(res);

      var achievementUser = await _context.AchievementUsers
         .Where(e =>
            e.IdAchievement == res.IdAchievement
            && e.IdUser ==     idUser)
         .SingleOrDefaultAsync();

      Assert.Null(achievementUser);
   }
   
   [Fact]
   public async Task DeleteUserAchievement_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var idAchievement = 10;
      var idUser = 10;

      //Act
      var res = await _repository.DeleteUserAchievement(idAchievement, idUser);

      //Assert
      Assert.Null(res);
   }
   
   [Fact]
   public async Task AddNewUserAchievement_Returns_AchievementDto()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var idAchievement = 1;
      var idUser = 2;

      //Act
      var res = await _repository.AddNewUserAchievement(idAchievement, idUser);

      //Assert
      Assert.NotNull(res);

      var achievementUser = await _context.AchievementUsers
         .Where(e =>
            e.IdAchievement == res.IdAchievement
            && e.IdUser ==     idUser)
         .SingleOrDefaultAsync();

      Assert.NotNull(achievementUser);
   }
   
   [Fact]
   public async Task AddNewUserAchievement_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var idAchievement = 10;
      var idUser = 10;

      //Act
      var res = await _repository.AddNewUserAchievement(idAchievement, idUser);

      //Assert
      Assert.Null(res);
   }
   
   [Fact]
   public async Task AddNewUserAchievement_Throws_ReferenceException()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();

      var idAchievement = 1;
      var idUser = 1;

      //Act

      //Assert
      await Assert.ThrowsAsync<ReferenceException>(() => _repository.AddNewUserAchievement(idAchievement, idUser));
   }
   
   [Fact]
   public async Task GetUserAchievements_Returns_IEnumerable()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();
      
      var idUser = 1;

      //Act
      var res = await _repository.GetUserAchievements(idUser);

      //Assert
      Assert.NotNull(res);

      var achievements = await _context.Achievements
         .SelectMany(e => e.AchievementUsers)
         .Where(e => e.IdUser == idUser)
         .Select(e => e.IdAchievement)
         .ToListAsync();
      
      
      Assert.Equal(achievements, res.Select(e => e.IdAchievement));
   }
   
   [Fact]
   public async Task GetUserAchievements_Returns_Null()
   {
      //Arrange
      await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

      await AddTestData();
      
      var idUser = 10;

      //Act
      var res = await _repository.GetUserAchievements(idUser);

      //Assert
      Assert.Null(res);
   }
}


