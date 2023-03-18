using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.User;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTest.User;

public class UserRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly UserDbRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        var configuration = new ConfigurationManager();
        configuration["JWT:Key"] = "3Zd75xqq5HtGPsjbVanrxgFPZG5FOnNDXmGXW0F7Q7Wng9vT3bIjOSj5M9KyZweyFCzufpUt7JZw0cdF";
        //to trzeba jakoś lepiej otentegować
        
        _context = new GryzillaContext(options, true);
        _repository = new UserDbRepository(_context, new ConfigurationManager());
    }

    private async Task AddTestDataWithOneUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Moderator",
            RankLevel = 2
        });
        await _context.SaveChangesAsync();

        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "Admin",
            RankLevel = 3
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
        {
            Name = "User",
            RankLevel = 4
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
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithManyUser()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank
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
        await _context.SaveChangesAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task DeleteUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteUserFromDb(idUser);
        
        //Assert
        Assert.NotNull(res);

        var users = _context.UserData.ToList();
        Assert.False(users.Exists(e => e.IdUser== res.IdUser));
    }
    
    [Fact]
    public async Task DeleteUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteUserFromDb(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        //Act
        var res = await _repository.ModifyUserFromDb(idUser, putUserFromDb);
        
        //Assert
        Assert.NotNull(res);
        var users = _context.UserData.ToList();
        Assert.True(users.Exists(e => e.Nick == res.Nick));
    }
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var idUser = 1;

        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Nick2",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.ModifyUserFromDb(idUser, putUserFromDb));
    }
    
    [Fact]
    public async Task ModifyUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        var idUser = 1;
        
        var putUserFromDb = new PutUserDto
        {
            IdUser = 1,
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        var res = await _repository.ModifyUserFromDb(idUser, putUserFromDb);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddUserToDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        var addUserToDb = new AddUserDto
        {
            Nick = "Poziomka",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        //Act
        var res = await _repository.AddUserToDb(addUserToDb);
        
        //Assert
        Assert.NotNull(res);
        var users = _context.UserData.ToList();
        Assert.True(users.Exists(e => e.Nick == res.Nick));
    }
    
    [Fact]
    public async Task AddUserToDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var addUserToDb = new AddUserDto
        {
            Nick = "Nick1",
            Email = "email@gmail.com",
            Password = "2222",
            PhoneNumber = "14141515"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddUserToDb(addUserToDb));
    }
    
    [Fact]
    public async Task GetUserFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        //Act
        var res = await _repository.GetUserFromDb(idUser);
        
        //Assert
        Assert.NotNull(res);
        
        var users = _context.UserData.ToList();
        Assert.Single(users);
        
        var user = users.SingleOrDefault(e => e.IdUser == res.IdUser);
        Assert.NotNull(user);
    }
    
    [Fact]
    public async Task GetUserFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        var idUser = 1;

        //Act
        var res = await _repository.GetUserFromDb(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetUsersFromDb_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;

        //Act
        var res = await _repository.GetUsersFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var users = await _context.UserData.Select(e => e.IdUser).ToListAsync();
        Assert.Equal(users, res.Select(e => e.IdUser));
    }
    
    [Fact]
    public async Task GetUsersFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        //Act
        var res = await _repository.GetUsersFromDb();
        
        //Assert
        Assert.Empty(res);
    }
    
    [Fact]
    public async Task ChangeUserRank_Returns_UserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var userRank = new UserRank
        {
            IdUser = 1,
            IdRank = 2
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.NotNull(res);

        var userRankId = _context.UserData
            .Where(e => e.IdUser == userRank.IdUser)
            .Select(e => e.IdRank)
            .SingleOrDefault();
        
        Assert.True(userRankId == userRank.IdRank);
    }
    
    [Fact]
    public async Task ChangeUserRank_WithNoExistingRankWithGivenId_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var userRank = new UserRank
        {
            IdUser = 1,
            IdRank = 20
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ChangeUserRank_WithNoExistingUserWithGivenId_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var userRank = new UserRank
        {
            IdUser = 100,
            IdRank = 2
        };

        //Act
        var res = await _repository.ChangeUserRank(userRank);
        
        //Assert
        Assert.Null(res);
    }
}