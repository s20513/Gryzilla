using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ReportPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.ReportPost;

public class RepostPostDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReportPostDbRepository _repository;
    
    public RepostPostDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReportPostDbRepository(_context);
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
        
        await _context.Posts.AddAsync(new Gryzilla_App.Post()
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.Reasons.AddAsync(new Gryzilla_App.Reason
        {
            ReasonName = "Test2"
        });
        await _context.SaveChangesAsync();

        await _context.ReportPosts.AddAsync(new Gryzilla_App.ReportPost
        {
            IdUser = 2,
            IdPost = 1,
            IdReason = 1,
            Description = "test",
            ReportedAt = DateTime.Now,
            Viewed = false
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddReportPostToDb_Returns_ReportPostResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportPost = new NewReportPostRequestDto
        {
            IdUser = 3,
            IdPost = 1,
            IdReason = 1,
            Description = "test"
        };
        
        //Act
        var res = await _repository.AddReportPostToDb(newReportPost);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportPosts.AnyAsync(e => 
               e.IdUser == newReportPost.IdUser
            && e.IdPost == newReportPost.IdPost
            && e.IdReason == newReportPost.IdReason);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task AddReportPostToDb_Returns_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportPost = new NewReportPostRequestDto
        {
            IdUser = 10,
            IdPost = 1,
            IdReason = 1,
            Description = "test"
        };
        
        //Act
        var res = await _repository.AddReportPostToDb(newReportPost);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddReportPostToDb_Returns_WithIdUserAsPostAuthor_Throws_UserCreatorException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReportPost = new NewReportPostRequestDto
        {
            IdUser = 1,
            IdPost = 1,
            IdReason = 1,
            Description = "test"
        };
        
        //Act

        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.AddReportPostToDb(newReportPost));
    }
    
    [Fact]
    public async Task DeleteReportPostFromDb_Returns_ReportPostResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportPostIdsRequestDto = new ReportPostIdsRequestDto
        {
            IdUser = 2,
            IdPost = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.DeleteReportPostFromDb(reportPostIdsRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportPosts.AnyAsync(e => 
            e.IdUser == reportPostIdsRequestDto.IdUser
            && e.IdPost == reportPostIdsRequestDto.IdPost
            && e.IdReason == reportPostIdsRequestDto.IdReason);
        
        Assert.False(report);
    }
    
    [Fact]
    public async Task DeleteReportPostFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var reportPostIdsRequestDto = new ReportPostIdsRequestDto
        {
            IdUser = 10,
            IdPost = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.DeleteReportPostFromDb(reportPostIdsRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task UpdateReportPostFromDb_Returns_ReportPostResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportPostRequestDto = new UpdateReportPostRequestDto
        {
            IdUser = 2,
            IdPost = 1,
            IdReason = 1,
            Description = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportPostFromDb(updateReportPostRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportPosts.AnyAsync(e => 
            e.IdUser == updateReportPostRequestDto.IdUser
            && e.IdPost == updateReportPostRequestDto.IdPost
            && e.IdReason == updateReportPostRequestDto.IdReason
            && e.Description == updateReportPostRequestDto.Description
            && e.Viewed == updateReportPostRequestDto.Viewed);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task UpdateReportPostFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportPostRequestDto = new UpdateReportPostRequestDto
        {
            IdUser = 10,
            IdPost = 1,
            IdReason = 1,
            Description = "NewTest",
            Viewed = true
        };
        
        //Act
        var res = await _repository.UpdateReportPostFromDb(updateReportPostRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportPostFromDb_Returns_ReportPostResponseDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportPostRequestDto = new ReportPostIdsRequestDto
        {
            IdUser = 2,
            IdPost = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.GetReportPostFromDb(updateReportPostRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var report = await _context.ReportPosts.AnyAsync(e => 
            e.IdUser == updateReportPostRequestDto.IdUser
            && e.IdPost == updateReportPostRequestDto.IdPost
            && e.IdReason == updateReportPostRequestDto.IdReason);
        
        Assert.True(report);
    }
    
    [Fact]
    public async Task GetReportPostFromDb_WithNotExistingUser_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var updateReportPostRequestDto = new ReportPostIdsRequestDto
        {
            IdUser = 10,
            IdPost = 1,
            IdReason = 1
        };
        
        //Act
        var res = await _repository.GetReportPostFromDb(updateReportPostRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetReportPostsFromDb_Returns_ListOfReportPostResponseDtos()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        //Act
        var res = await _repository.GetReportPostsFromDb();
        
        //Assert
        Assert.NotNull(res);

        var reportsNum = await _context.ReportPosts.CountAsync();
        
        Assert.True(res.Count() == reportsNum);
    }
}