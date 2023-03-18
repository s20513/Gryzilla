using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ReportCommentPost;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.ReportCommentPost;

public class ReportCommentPostRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReportCommentPostDbRepository _repository;

    public ReportCommentPostRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReportCommentPostDbRepository(_context);
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
        await _context.Posts.AddAsync(new Gryzilla_App.Models.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.Models.CommentPost
        {
            IdUser = 1,
            IdPost = 1,
            DescriptionPost = "DescPost1",
            CreatedAt = DateTime.Now
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
        await _context.ReportCommentPosts.AddAsync(new Gryzilla_App.Models.ReportCommentPost
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 1,
            Description = "Description",
            Viewed = false,
            ReportedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task GetReportCommentPostFromDb_Returns_ListOfReports()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        //Act
        var res = await _repository.GetReportCommentPostsFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var reports = await _context.ReportCommentPosts
            .Select(e => e.IdUser).SingleOrDefaultAsync();
        
        Assert.Equal(reports, res.Select(x=>x.IdUser).SingleOrDefault());
    }
    
    [Fact]
    public async Task GetReportCommentArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetReportCommentPostsFromDb();
        
        //Assert
        Assert.Empty(res);
    }
    [Fact]
    public async Task GetOneReportCommentPostsFromDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idUser = 1;
        var idComment = 1;
        var idReason = 1;
        
        //Act
        var res = await _repository.GetOneReportCommentPostFromDb(idUser, idComment, idReason);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == idReason);
        Assert.NotNull(report);

        if (res is null) return;
        Assert.Equal(res.IdReason, idReason);
    }

    [Fact]
    public async Task GetOneReportCommentPostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idUser = 1;
        var idComment = 2;
        var idReason = 1;
        
        //Act
        var res = await _repository.GetOneReportCommentPostFromDb(idUser, idComment, idReason);
        
        //Assert
        Assert.Null(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == idComment);
        Assert.Null(report);
    }
    
    [Fact]
    public async Task AddReportCommentPostToDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReasonDto = new NewReportCommentPostDto
        {
            IdUser = 2,
            IdComment = 1,
            IdReason = 2,
            Content = "Description"
        };
        
        //Act
        var res = await _repository.AddReportCommentPostToDb(newReasonDto);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == res.IdReason);
        Assert.NotNull(report);

        if (report is null) return;
        Assert.Equal(report.IdUser, newReasonDto.IdUser);
    }
    
    [Fact]
    public async Task AddReportToDb_Throws_UserCreatorException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReasonDto = new NewReportCommentPostDto
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 1,
            Content = "Description"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.AddReportCommentPostToDb(newReasonDto));
    }
    
    [Fact]
    public async Task AddReportToDb_Return_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());


        var newReportDto = new NewReportCommentPostDto
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 1,
            Content = "Description"
        };
        //Act
        var res = await _repository.AddReportCommentPostToDb(newReportDto);
        
        //Assert
        Assert.Null(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == newReportDto.IdReason);
        Assert.Null(report);
    }
    
    [Fact]
    public async Task DeleteReportCommentPostFromDb_Returns_ReportCommentPost()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DefaultReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 1,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentPostFromDb(deleteReportDto);
        
        //Assert
        Assert.NotNull(res);
        
        var reason = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == deleteReportDto.IdReason);
        Assert.Null(reason);
    }
    
    [Fact]
    public async Task DeleteReportFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DefaultReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 2,
            IdReason = 1,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentPostFromDb(deleteReportDto);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteReportFromDb_Returns_NullReport()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DefaultReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 2,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentPostFromDb(deleteReportDto);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task ModifyReportCommentPostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var modifyReportRequestDto = new UpdateReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 2,
            IdReason = 1,
            Content = "Description1",
            Viewed = false
        };

        //Act
        var res = await _repository.UpdateReportCommentPostFromDb(modifyReportRequestDto);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task ModifyReportCommentPostFromDb_Returns_NullReport()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        var modifyReportRequestDto = new UpdateReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 2,
            Content = "Description1",
            Viewed = false
        };

        //Act
        var res = await _repository.UpdateReportCommentPostFromDb(modifyReportRequestDto);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task ModifyReportCommentPostFromDb_Returns_ReportCommentPost()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataToDb();

        var modifyReportRequestDto = new UpdateReportCommentPostDto()
        {
            IdUser = 1,
            IdComment = 1,
            IdReason = 1,
            Content = "Description1",
            Viewed = false
        };
        //Act
        var res = await _repository.UpdateReportCommentPostFromDb(modifyReportRequestDto);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.Description == "Description");
        Assert.Null(report);
    }
}