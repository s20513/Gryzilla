using System.Security.Claims;
using Gryzilla_App.DTOs.Requests.GroupUserMessage;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.GroupUserMessage;

public class GroupUserMessageRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly GroupUserMessageDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public GroupUserMessageRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new GroupUserMessageDbRepository(_context);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
    }
    
    private async Task AddTestDataWithOneGroup()
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

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "test",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();

        await _context.GroupUsers.AddAsync(new GroupUser
        {
            IdGroup = 1,
            IdUser = 1
        });
        await _context.SaveChangesAsync();
        
        await _context.GroupUserMessages.AddAsync(new Gryzilla_App.Models.GroupUserMessage
        {
            IdUser = 1,
            IdGroup = 1,
            CreatedAt = DateTime.Now,
            Message = "Testowa wiadomość"
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddNewMessageToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();

        var newGroupUserMessageDto = new AddGroupUserMessageDto
        {
            IdUser = 1,
            IdGroup = 1,
            Content = "test"
        };
        
        //Act
        var res = await _repository.AddMessage(newGroupUserMessageDto);
        
        //Assert
        Assert.NotNull(res);

        var message = _context.GroupUserMessages.ToList();
        Assert.True(message.Exists(e => e.IdMessage == res.IdMessage));
    }
    
    [Fact]
    public async Task AddNewGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newGroupUserMessageDto = new AddGroupUserMessageDto()
        {
            IdUser = 1,
            IdGroup = 1,
            Content = "test"
        };
        //Act
        var res = await _repository.AddMessage(newGroupUserMessageDto);
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteMessageToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();

        var idMessage = 1;
        
        //Act
        var res = await _repository.DeleteMessage(idMessage, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);

        var message = _context.GroupUserMessages.ToList();
        Assert.False(message.Exists(e => e.IdMessage == res.IdMessage));
    }
    
    [Fact]
    public async Task DeleteGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idMessage = 1;
        
        //Act
        var res = await _repository.DeleteMessage(idMessage, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyMessageToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idMessage = 2;
        
        var modifyGroupRequestDto = new UpdateGroupUserMessageDto
        {
             IdMessage = 2,
             Content = "testowa wiadomosc1"
        };
        //Act
        var res = await _repository.ModifyMessage(idMessage, modifyGroupRequestDto, _mockClaimsPrincipal.Object);
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyGroupToDb_Returns_GroupDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneGroup();
        
        var idMessage = 1;
        
        var modifyGroupRequestDto = new UpdateGroupUserMessageDto
        {
            IdMessage = 1,
            Content = "testowa wiadomosc1"
        };
        
        //Act
        var res = await _repository.ModifyMessage(idMessage, modifyGroupRequestDto, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var group = await _context.GroupUserMessages
            .SingleOrDefaultAsync(e =>
            e.IdMessage  == idMessage 
            && e.Message == modifyGroupRequestDto.Content);
        
        Assert.NotNull(group);
    }
    
    [Fact]
    public async Task GetMessageFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneGroup();
        int idGroup = 1;
        //Act
        var res = await _repository.GetMessages(idGroup);
        
        //Assert
        Assert.NotNull(res);
        
        var messages = await _context.GroupUserMessages.Select(e => e.IdGroup).ToArrayAsync();
        Assert.Equal(messages, res.Select(e => e.IdGroup));
    }
}