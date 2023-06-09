using Gryzilla_App.DTOs.Requests.ReportProfileComment;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.ReportProfileComment;

public class ReportProfileCommentDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReportProfileCommentDbRepository _repository;
    
    public ReportProfileCommentDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReportProfileCommentDbRepository(_context);
    }

    private async Task AddTestDataToDb()
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
        
        await _context.ProfileComments.AddAsync(new Gryzilla_App.Models.ProfileComment
        {
            IdUser = 1,
            IdUserComment = 2,
            CreatedAt = DateTime.Today,
            Description = "Super profil"
        });
        await _context.SaveChangesAsync();
        
        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "Test2"
        });
        await _context.SaveChangesAsync();
        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "Test3"
        });
        await _context.SaveChangesAsync();
        await _context.ReportProfileComments.AddAsync(new Gryzilla_App.Models.ReportProfileComment
        {
            IdUser = 2,
            IdProfileComment= 1,
            IdReason = 1,
            Description = "test",
            ReportedAt = DateTime.Now,
            Viewed = false
        });
        await _context.SaveChangesAsync();
        await _context.ReportProfileComments.AddAsync(new Gryzilla_App.Models.ReportProfileComment
        {
            IdUser = 2,
            IdProfileComment= 1,
            IdReason = 2,
            Description = "test",
            ReportedAt = DateTime.Now.AddHours(2),
            Viewed = false
        });
        await _context.SaveChangesAsync();
        await _context.Reasons.AddAsync(new Gryzilla_App.Models.Reason
        {
            ReasonName = "Test4"
        });
        await _context.ReportProfileComments.AddAsync(new Gryzilla_App.Models.ReportProfileComment
        {
            IdUser = 2,
            IdProfileComment= 1,
            IdReason = 3,
            Description = "test",
            ReportedAt = DateTime.Now.AddHours(1),
            Viewed = false
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddReportProfileCommentToDb_Returns_ReportProfileCommentResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportProfileComment = new NewReportProfileCommentRequestDto
        {
            IdUser = 3,
            IdProfileComment = 1,
            IdReason = 1,
            Content = "test"
        };
        
        //Act
        var res = await _repository.AddReportProfileCommentToDb(newReportProfileComment);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportProfileComments.AnyAsync(e => 
               e.IdUser == newReportProfileComment.IdUser
            && e.IdProfileComment == newReportProfileComment.IdProfileComment
            && e.IdReason == newReportProfileComment.IdReason);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task AddReportProfileCommentToDb_Returns_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportProfileComment = new NewReportProfileCommentRequestDto
        {
            IdUser = 10,
            IdProfileComment = 1,
            IdReason = 1,
            Content = "test"
        };
        
        //Act
        var res = await _repository.AddReportProfileCommentToDb(newReportProfileComment);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddReportProfileCommentToDb_Returns_WithIdUserAsProfileCommentAuthor_Throws_UserCreatorException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportProfileComment = new NewReportProfileCommentRequestDto
        {
            IdUser = 1,
            IdProfileComment = 1,
            IdReason = 1,
            Content = "test"
        };
        
        //Act

        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.AddReportProfileCommentToDb(newReportProfileComment));
    }
    
    [Fact]
    public async Task DeleteReportProfileCommentFromDb_Returns_ReportProfileCommentResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto
        {
            IdUser = 2,
            IdProfileComment = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.DeleteReportProfileCommentFromDb(reportProfileCommentIdsRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportProfileComments.AnyAsync(e => 
            e.IdUser == reportProfileCommentIdsRequestDto.IdUser
            && e.IdProfileComment == reportProfileCommentIdsRequestDto.IdProfileComment
            && e.IdReason == reportProfileCommentIdsRequestDto.IdReason);
        
        Assert.False(report);
    }
    
    [Fact]
    public async Task DeleteReportProfileCommentFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportProfileCommentIdsRequestDto = new ReportProfileCommentIdsRequestDto
        {
            IdUser = 10,
            IdProfileComment = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.DeleteReportProfileCommentFromDb(reportProfileCommentIdsRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task UpdateReportProfileCommentFromDb_Returns_ReportProfileCommentResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportProfileCommentRequestDto = new UpdateReportProfileCommentRequestDto
        {
            IdUser = 2,
            IdProfileComment = 1,
            IdReason = 1,
            Content = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportProfileCommentFromDb(updateReportProfileCommentRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportProfileComments.AnyAsync(e => 
            e.IdUser == updateReportProfileCommentRequestDto.IdUser
            && e.IdProfileComment== updateReportProfileCommentRequestDto.IdProfileComment
            && e.IdReason == updateReportProfileCommentRequestDto.IdReason
            && e.Description == updateReportProfileCommentRequestDto.Content
            && e.Viewed == updateReportProfileCommentRequestDto.Viewed);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task UpdateReportProfileCommentFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportProfileCommentRequestDto = new UpdateReportProfileCommentRequestDto
        {
            IdUser = 10,
            IdProfileComment = 1,
            IdReason = 1,
            Content = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportProfileCommentFromDb(updateReportProfileCommentRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportProfileCommentFromDb_Returns_ReportProfileCommentResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportProfileCommentRequestDto = new ReportProfileCommentIdsRequestDto
        {
            IdUser = 2,
            IdProfileComment = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.GetReportProfileCommentFromDb(updateReportProfileCommentRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportProfileComments.AnyAsync(e => 
            e.IdUser == updateReportProfileCommentRequestDto.IdUser
            && e.IdProfileComment == updateReportProfileCommentRequestDto.IdProfileComment
            && e.IdReason == updateReportProfileCommentRequestDto.IdReason);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task GetReportProfileCommentFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportProfileCommentRequestDto = new ReportProfileCommentIdsRequestDto
        {
            IdUser = 10,
            IdProfileComment= 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.GetReportProfileCommentFromDb(reportProfileCommentRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportProfileCommentsFromDb_Returns_ListOfReportProfileCommentsResponseDtos()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        //Act
        var res = await _repository.GetReportProfileCommentsFromDb();
        
        //Assert
        Assert.NotNull(res);

        var reportsNum = await _context.ReportProfileComments.CountAsync();
        
        Assert.True(res.Count() == reportsNum);
    }
}