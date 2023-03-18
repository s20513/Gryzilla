using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Reason;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Reason;

public class ReasonsRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReasonDbRepository _repository;

    public ReasonsRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReasonDbRepository(_context);
    }

    private async Task AddTestDataToDb()
    {
        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "Test1"
        });

        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "Test2"
        });
        
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetReasonsFromDb_Returns_ListOfFullTagDtos()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        //Act
        var res = await _repository.GetReasonsFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var reasonIds = await _context.Reasons
            .Select(e => e.IdReason).ToListAsync();
        
        Assert.Equal(reasonIds, res.Select(e => e.Id));
    }
    
    [Fact]
    public async Task GetReasonFromDb_Returns_FullTagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 1;
        
        //Act
        var res = await _repository.GetReasonFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var reason = await _context.Reasons.SingleOrDefaultAsync(e => e.IdReason == id);
        Assert.NotNull(reason);

        if (res is null) return;
        Assert.Equal(res.Id, id);
    }
    
    [Fact]
    public async Task GetReasonFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 10;
        
        //Act
        var res = await _repository.GetReasonFromDb(id);
        
        //Assert
        Assert.Null(res);
        
        var reason = await _context.Reasons.SingleOrDefaultAsync(e => e.IdReason == id);
        Assert.Null(reason);
    }
    
    [Fact]
    public async Task AddReasonToDb_Returns_FullTagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReasonDto = new NewReasonDto
        {
            Name = "NewReason"
        };
        
        //Act
        var res = await _repository.AddReasonToDb(newReasonDto);
        
        //Assert
        Assert.NotNull(res);
        
        var reason = await _context.Reasons.SingleOrDefaultAsync(e => e.IdReason == res.Id);
        Assert.NotNull(reason);

        if (reason is null) return;
        Assert.Equal(reason.ReasonName, newReasonDto.Name);
    }
    
    [Fact]
    public async Task AddReasonToDb_Throws_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReasonDto = new NewReasonDto
        {
            Name = "Test1"
        };
        
        //Act
        
        //Assert

        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddReasonToDb(newReasonDto));
    }
    
    [Fact]
    public async Task DeleteReasonFromDb_Returns_FullTagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteReasonFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var reason = await _context.Reasons.SingleOrDefaultAsync(e => e.IdReason == id);
        Assert.Null(reason);
    }
    
    [Fact]
    public async Task DeleteReasonFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 10;
        
        //Act
        var res = await _repository.DeleteReasonFromDb(id);
        
        //Assert
        Assert.Null(res);
    }
}