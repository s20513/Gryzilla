using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Link;

public class LinkRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly LinkDbRepository _repository;

    public LinkRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        _context = new GryzillaContext(options, true);
        _repository = new LinkDbRepository(_context);
    }
    
    private async Task AddTestDataWithOneUser()
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
            CreatedAt = DateTime.Today,
            DiscordLink = "DiscordLink",
            SteamLink = "SteamLink"
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task DeleteSteamLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkSteam(idUser);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task DeleteLinkSteam_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkSteam(idUser);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteLinkDiscord_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkDiscord(idUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteDiscordLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkDiscord(idUser);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task ModifyLinkDiscord_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var linkDto = new LinkDto
        {
            IdUser = 2,
            Link = "DiscordLink"
        };
        
        //Act
        var res = await _repository.PutLinkDiscord(linkDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyDiscordLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "DiscordLink"
        };
        
        //Act
        var res = await _repository.PutLinkDiscord(linkDto);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.Equals("Link changed"));
    }
    
    
    [Fact]
    public async Task ModifyLinkSteam_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var linkDto = new LinkDto
        {
            IdUser = 2,
            Link = "DiscordLink"
        };
        
        //Act
        var res = await _repository.PutLinkSteam(linkDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifySteamLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "DiscordLink"
        };
        
        //Act
        var res = await _repository.PutLinkSteam(linkDto);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task GetUsersLinkFromDb_Returns_String()
    {
        //Arrange
        var idUser = 1;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        
        //Act
        var res = await _repository.GetUserLinks(idUser);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.DiscordLink.Equals("DiscordLink"));
    }
    
    [Fact]
    public async Task GetUsersLinkFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        var idUser = 1;
        
        
        //Act
        var res = await _repository.GetUserLinks(idUser);
        
        //Assert
        Assert.Null(res);
    }
}