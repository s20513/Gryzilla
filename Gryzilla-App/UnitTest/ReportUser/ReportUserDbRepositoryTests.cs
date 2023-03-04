using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ReportUser;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.ReportUser;

public class ReportUserDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReportUserDbRepository _repository;
    
    public ReportUserDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReportUserDbRepository(_context);
    }
    
    private async Task AddTestDataToDb()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
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
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick3",
            Password = "Pass3",
            Email = "email3",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        await _context.Reasons.AddAsync(new Gryzilla_App.Reason
        {
            ReasonName = "Test2"
        });
        await _context.SaveChangesAsync();

        await _context.ReportUsers.AddAsync(new Gryzilla_App.ReportUser
        {
            IdUserReported = 1,
            IdUserReporting = 2,
            IdReason = 1,
            Description = "test",
            ReportedAt = DateTime.Now,
            Viewed = false
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddReportUserToDb_Returns_ReportUserResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportUser = new NewReportUserDto
        {
            IdUserReported = 1,
            IdUserReporting = 3,
            IdReason = 1,
            Description = "test"
        };
        
        //Act
        var res = await _repository.AddReportUserToDb(newReportUser);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportUsers.AnyAsync(e => 
            e.IdReport == 2);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task AddReportUserToDb_Returns_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportUser = new NewReportUserDto
        {
            IdUserReported = 1,
            IdUserReporting = 4,
            IdReason = 1,
            Description = "test"
        };
        
        //Act
        var res = await _repository.AddReportUserToDb(newReportUser);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddReportUserToDb_Returns_Throws_UserCreatorException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportUser = new NewReportUserDto
        {
            IdUserReported = 1,
            IdUserReporting = 1,
            IdReason = 1,
            Description = "test"
        };
        
        //Act

        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.AddReportUserToDb(newReportUser));
    }
    
    [Fact]
    public async Task DeleteReportUserFromDb_Returns_ReportUserResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportId = 1;
        
        //Act
        var res = await _repository.DeleteReportUserFromDb(reportId);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportUsers.AnyAsync(e => 
            e.IdReport == 1);
        
        Assert.False(report);
    }
    
    [Fact]
    public async Task DeleteReportUserFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportId = 2;
        
        //Act
        var res = await _repository.DeleteReportUserFromDb(reportId);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task UpdateReportArticleFromDb_Returns_ReportArticleResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReport = new ModifyReportUser
        {
            IdReport = 1,
            Description = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportUserFromDb(updateReport);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context
            .ReportUsers
            .AnyAsync(e => 
                e.IdReport       == updateReport.IdReport
                && e.Description == updateReport.Description
                && e.Viewed      == updateReport.Viewed);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task UpdateReportUserFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReport = new ModifyReportUser
        {
            IdReport = 2,
            Description = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportUserFromDb(updateReport);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportUserFromDb_Returns_ReportUserResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportId = 1;
        
        //Act
        var res = await _repository.GetUserReportFromDb(reportId);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportUsers.AnyAsync(e => 
            e.IdReport == reportId);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task GetReportUserFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportId = 2;
        
        //Act
        var res = await _repository.GetUserReportFromDb(reportId);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportUsersFromDb_Returns_ListOfReportUsers()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        //Act
        var res = await _repository.GetUsersReportsFromDb();
        
        //Assert
        Assert.NotNull(res);

        var reportsNum = await _context.ReportUsers.CountAsync();
        
        Assert.True(res.Count() == reportsNum);
    }
}