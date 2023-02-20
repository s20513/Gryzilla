using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.BlockedUser;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.BlockedUser;

public class BlockedUserDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly BlockedUserDbRepository _repository;

    public BlockedUserDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();

        _context = new GryzillaContext(options, true);
        _repository = new BlockedUserDbRepository(_context);
    }

    private async Task AddTestDataWithBlockedAndPreviousBlocked()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "User",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Admin",
            RankLevel = 4
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 2,
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
        
        await _context.BlockedUsers.AddAsync(new Gryzilla_App.BlockedUser
        {
            IdUser = 1,
            IdUserBlocked = 2,
            Comment = "test1"
        });
        await _context.SaveChangesAsync();

        var blockedUser = await _context.BlockedUsers.SingleAsync();
        _context.BlockedUsers.Remove(blockedUser);
        await _context.SaveChangesAsync();
        
        await _context.BlockedUsers.AddAsync(new Gryzilla_App.BlockedUser
        {
            IdUser = 1,
            IdUserBlocked = 2,
            Comment = "test2"
        });
        await _context.SaveChangesAsync();
    }

    private async Task AddTestDataWithNoBlocking()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });
        await _context.SaveChangesAsync();
        
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Admin",
            RankLevel = 4
        });
        await _context.SaveChangesAsync();

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 2,
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
    }
    
    [Fact]
    public async Task GetBlockedUsers_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithBlockedAndPreviousBlocked();

        //Act
        var res = await _repository.GetBlockedUsers();

        //Assert
        Assert.NotNull(res);

        var blockedUsersIds = await _context.BlockedUsers.Select(e => e.IdUserBlocked).ToListAsync();
        Assert.Equal(blockedUsersIds, res.Select(e => e.IdUserBlocked));
    }

    [Fact]
    public async Task BlockUser_WithBlockingUserNotExisting_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var blockedUserRequestDto = new BlockedUserRequestDto
        {
            IdUserBlocking = 10,
            IdUserBlocked = 2,
            Comment = "Test"
        };
        
        //Act
        var res = await _repository.BlockUser(blockedUserRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task BlockUser_WithBlockedUserNotExisting_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var blockedUserRequestDto = new BlockedUserRequestDto
        {
            IdUserBlocking = 1,
            IdUserBlocked = 20,
            Comment = "Test"
        };
        
        //Act
        var res = await _repository.BlockUser(blockedUserRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task BlockUser_WithBlockedUserAsAdmin_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var blockedUserRequestDto = new BlockedUserRequestDto
        {
            IdUserBlocking = 2,
            IdUserBlocked = 1,
            Comment = "Test"
        };
        
        //Act
        var res = await _repository.BlockUser(blockedUserRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task BlockUser_Returns_BlockedUserDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var blockedUserRequestDto = new BlockedUserRequestDto
        {
            IdUserBlocking = 1,
            IdUserBlocked = 2,
            Comment = "Test"
        };
        
        //Act
        var res = await _repository.BlockUser(blockedUserRequestDto);
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task UnlockUser_WithBlockedUserNotExisting_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var idUser = 1;
        
        //Act
        var res = await _repository.UnlockUser(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task UnlockUser_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithBlockedAndPreviousBlocked();

        var idUser = 2;
        
        //Act
        var res = await _repository.UnlockUser(idUser);
        
        //Assert
        Assert.NotNull(res);
    }
    
    [Fact]
    public async Task GetUserBlockingHistory_Returns_UserBlockingHistoryDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithBlockedAndPreviousBlocked();

        var idUser = 2;

        //Act
        var res = await _repository.GetUserBlockingHistory(idUser);

        //Assert
        Assert.NotNull(res);

        var history = await _context.BlockedUsers
            .TemporalAll()
            .Where(e => e.IdUserBlocked == idUser)
            .Select(e => e.IdUser)
            .ToListAsync();
        
        Assert.Equal(history, res.History.Select(e => e.IdUserBlocking));
    }
    
    [Fact]
    public async Task GetUserBlockingHistory_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 2;

        //Act
        var res = await _repository.GetUserBlockingHistory(idUser);

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task CheckIfUserIsBlocked_WithUserNotExisting_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var idUser = 20;

        //Act
        var res = await _repository.CheckIfUserIsBlocked(idUser);

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task CheckIfUserIsBlocked_Returns_Bool()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithNoBlocking();

        var idUser = 2;

        //Act
        var res = await _repository.CheckIfUserIsBlocked(idUser);

        //Assert
        Assert.NotNull(res);
        Assert.False(res);
    }
    
}