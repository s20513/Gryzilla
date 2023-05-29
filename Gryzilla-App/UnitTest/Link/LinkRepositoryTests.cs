using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.Link;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.Link;

public class LinkRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly LinkDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public LinkRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        _context = new GryzillaContext(options, true);
        _repository = new LinkDbRepository(_context);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
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
    public async Task DeleteEpicLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkEpic(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task DeleteLinkEpic_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkEpic(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeletePsLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkPs(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task DeleteLinkPs_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkPs(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteXboxLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkXbox(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task DeleteLinkXbox_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkXbox(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteSteamLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkSteam(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link removed"));
    }
    
    [Fact]
    public async Task DeleteLinkSteam_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        
        //Act
        var res = await _repository.DeleteLinkSteam(idUser, _mockClaimsPrincipal.Object);
        
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
        var res = await _repository.DeleteLinkDiscord(idUser, _mockClaimsPrincipal.Object);
        
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
        var res = await _repository.DeleteLinkDiscord(idUser, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link removed"));
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
        var res = await _repository.PutLinkDiscord(linkDto, _mockClaimsPrincipal.Object);
        
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
            Link = "https://discord.gg/QTpcZBbx"
        };
        
        //Act
        var res = await _repository.PutLinkDiscord(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task ModifyDiscordLinkFromDb_Returns_Invalid()
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
        var res = await _repository.PutLinkDiscord(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Cannot change the link. Invalid link!"));
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
        var res = await _repository.PutLinkSteam(linkDto, _mockClaimsPrincipal.Object);
        
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
            Link = "https://steamcommunity.com/profiles/76561198204817567"
        };
        
        //Act
        var res = await _repository.PutLinkSteam(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task ModifySteamLinkFromDb_Returns_Invalid()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "https://steamcommunity.com/76561198204817567"
        };
        
        //Act
        var res = await _repository.PutLinkSteam(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Cannot change the link. Invalid link!"));
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
        Assert.True(res.DiscordLink != null && res != null && res.DiscordLink.Equals("DiscordLink"));
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
    
    [Fact]
    public async Task ModifyLinkEpic_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var linkDto = new LinkDto
        {
            IdUser = 2,
            Link = "EpicLink"
        };
        
        //Act
        var res = await _repository.PutLinkEpic(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyEpicLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "https://launcher.store.epicgames.com/u/0e11160b58cd413cab6dc5c80105aaec"
        };
        
        //Act
        var res = await _repository.PutLinkEpic(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task ModifyEpicLinkFromDb_Returns_Invalid()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "EpicLink"
        };
        
        //Act
        var res = await _repository.PutLinkEpic(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Cannot change the link. Invalid link!"));
    }
    
    
    [Fact]
    public async Task ModifyXboxLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "https://account.xbox.com/pl-pl/Profile?xr=mebarnav&rtc=1&csrf=eNX57RWU3bPJbnFgQUCMtkUZTq0Mtvg"
        };
        
        //Act
        var res = await _repository.PutLinkXbox(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task ModifyXboxLinkFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        var linkDto = new LinkDto
        {
            IdUser = 2,
            Link = "https://account.xbox.com/pl-pl/Profile?xr=mebarnav&rtc=1&csrf=eNX57RWU3bPJbnFgQUCMtkUZTq0Mtvg"
        };
        //Act
        var res = await _repository.PutLinkXbox(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyXboxLinkFromDb_Returns_Invalid()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "https://account.xbox.com/pl-pl/xr=mebarnav&rtc=1&csrf=eNX57RWU3bPJbnFgQUCMtkUZTq0Mtvg"
        };
        
        //Act
        var res = await _repository.PutLinkXbox(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Cannot change the link. Invalid link!"));
    }

    [Fact]
    public async Task ModifyPsLinkFromDb_Returns_String()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneUser();
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "PsLink"
        };
        
        //Act
        var res = await _repository.PutLinkPs(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res != null && res.Equals("Link changed"));
    }
    
    [Fact]
    public async Task ModifyPsLinkFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        var linkDto = new LinkDto
        {
            IdUser = 1,
            Link = "PsLink"
        };
        //Act
        var res = await _repository.PutLinkPs(linkDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
}