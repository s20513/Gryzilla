using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Friends;

public class FriendsRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly FriendsDbRepository _repository;

    public FriendsRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new FriendsDbRepository(_context);
    }

    private async Task AddTestDataWithManyUser()
    {
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
    public async Task AddNewFriendToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser1 = 1;
        var idUser2 = 2;
        
        //Act
        var res = await _repository.AddNewFriendToDb(idUser1, idUser2);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddNewFriendToDb_Returns_FriendDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyUser();
        var idUser1 = 1;
        var idUser2 = 2;
        
        //Act
        var res = await _repository.AddNewFriendToDb(idUser1, idUser2);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.IdUser.Equals(idUser2));
    }
    
    [Fact]
    public async Task AddUserToDb_Returns_ReferenceException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithManyUser();

        var user = await _context.UserData.Include(x => x.IdUserFriends).FirstAsync();
        var user1 = await _context.UserData.Include(x => x.IdUserFriends).OrderBy(x=>x.IdUser).LastAsync();
        
        user.IdUserFriends.Add(user1);
        await _context.SaveChangesAsync();
        
        var idUser1 = 1;
        var idUser2 = 2;
        
        //Act
        //Assert
        await Assert.ThrowsAsync<ReferenceException>(() => _repository.AddNewFriendToDb(idUser1, idUser2));
    }
        
    [Fact]
    public async Task DeleteFriendFromDb_Returns_FriendDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyUser();
        
        var idUser1 = 1;
        var idUser2 = 2;
        var user = await _context.UserData.Include(x => x.IdUserFriends).FirstAsync();
        var user1 = await _context.UserData.Include(x => x.IdUserFriends).OrderBy(x=>x.IdUser).LastAsync();
        
        user.IdUserFriends.Add(user1);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.DeleteFriendFromDb(idUser1, idUser2);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.IdUser.Equals(idUser2));
    }
    
    [Fact]
    public async Task DeleteFriendFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser1 = 1;
        var idUser2 = 2;
        
        //Act
        var res = await _repository.DeleteFriendFromDb(idUser1, idUser2);
        
        //Assert
        Assert.Null(res);
    }
    
        
    [Fact]
    public async Task GetFriendsFromDb_Returns_FriendDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithManyUser();
        
        var idUser1 = 1;
        var user = await _context.UserData.Include(x => x.IdUserFriends).FirstAsync();
        var user1 = await _context.UserData.Include(x => x.IdUserFriends).OrderBy(x=>x.IdUser).LastAsync();
        
        user.IdUserFriends.Add(user1);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.GetFriendsFromDb(idUser1);
        
        //Assert
        Assert.NotNull(res);
        Assert.NotEmpty(res);
        Assert.True(res.First().IdUser == user1.IdUser);
        
    }
    
    [Fact]
    public async Task GetFriendsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        var idUser1 = 1;
        
        //Act
        var res = await _repository.GetFriendsFromDb(idUser1);
        
        //Assert
        Assert.Null(res);
    }
    
}