using Gryzilla_App;
using Gryzilla_App.DTOs.Requests.ReportCommentArticle;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
namespace UnitTest.ReportCommentArticle;

public class ReportCommentArticleRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly ReportCommentArticleDbRepository _repository;

    public ReportCommentArticleRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new ReportCommentArticleDbRepository(_context);
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
            Nick = "Nick2",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick1",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Articles.AddAsync(new Gryzilla_App.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.CommentArticles.AddAsync(new Gryzilla_App.CommentArticle
        {
            IdUser = 1,
            IdArticle = 1,
            DescriptionArticle = "DescPost1"
        });
        await _context.SaveChangesAsync();
        
        await _context.Reasons.AddAsync(new Gryzilla_App.Reason
        {
            ReasonName = "Test2"
        });
        
        await _context.SaveChangesAsync();
        
        await _context.Reasons.AddAsync(new Gryzilla_App.Reason
        {
            ReasonName = "Test3"
        });
        
        await _context.SaveChangesAsync();
        await _context.ReportCommentArticles.AddAsync(new Gryzilla_App.ReportCommentArticle
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 1,
            Description = "Description",
            Viewed = false,
            ReportedAt = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task GetReportCommentArticleFromDb_Returns_ListOfReports()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        //Act
        var res = await _repository.GetReportCommentArticlesFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var reports = await _context.ReportCommentArticles
            .Select(e => e.IdUser).SingleOrDefaultAsync();
        
        Assert.Equal(reports, res.Select(x=>x.IdUser).SingleOrDefault());
    }
    [Fact]
    public async Task GetReportCommentArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetReportCommentArticlesFromDb();
        
        //Assert
        Assert.Empty(res);
    }
    [Fact]
    public async Task GetOneReportCommentArticleFromDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idUser = 1;
        var idComment = 1;
        var idReason = 1;
        
        //Act
        var res = await _repository.GetOneReportCommentArticleFromDb(idUser, idComment, idReason);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentArticles.SingleOrDefaultAsync(e => e.IdReason == idReason);
        Assert.NotNull(report);

        if (res is null) return;
        Assert.Equal(res.IdReason, idReason);
    }

    [Fact]
    public async Task GetOneReportCommentArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idUser = 1;
        var idComment = 2;
        var idReason = 1;
        
        //Act
        var res = await _repository.GetOneReportCommentArticleFromDb(idUser, idComment, idReason);
        
        //Assert
        Assert.Null(res);
        
        var report = await _context.ReportCommentArticles.SingleOrDefaultAsync(e => e.IdReason == idComment);
        Assert.Null(report);
    }
    
    [Fact]
    public async Task AddReportCommentArticleToDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newReasonDto = new NewReportCommentArticleDto
        {
            IdUser = 2,
            IdCommentArticle = 1,
            IdReason = 2,
            Description = "Description"
        };
        
        //Act
        var res = await _repository.AddReportCommentArticleToDb(newReasonDto);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentArticles.SingleOrDefaultAsync(e => e.IdReason == res.IdReason);
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

        var newReasonDto = new NewReportCommentArticleDto
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 1,
            Description = "Description"
        };
        
        //Act
        //Assert
        await Assert.ThrowsAsync<UserCreatorException>(() => _repository.AddReportCommentArticleToDb(newReasonDto));
    }
    
    [Fact]
    public async Task AddReportToDb_Return_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());


        var newReportDto = new NewReportCommentArticleDto
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 1,
            Description = "Description"
        };
        //Act
        var res = await _repository.AddReportCommentArticleToDb(newReportDto);
        
        //Assert
        Assert.Null(res);
        
        var report = await _context.ReportCommentPosts.SingleOrDefaultAsync(e => e.IdReason == newReportDto.IdReason);
        Assert.Null(report);
    }
    
    [Fact]
    public async Task DeleteReportCommentArticleFromDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DeleteReportCommentArticleDto()
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 1,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentArticleFromDb(deleteReportDto);
        
        //Assert
        Assert.NotNull(res);
        
        var reason = await _context.ReportCommentArticles.SingleOrDefaultAsync(e => e.IdReason == deleteReportDto.IdReason);
        Assert.Null(reason);
    }
    
    [Fact]
    public async Task DeleteReportFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DeleteReportCommentArticleDto()
        {
            IdUser = 1,
            IdCommentArticle = 2,
            IdReason = 1,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentArticleFromDb(deleteReportDto);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteReportFromDb_Returns_NullReport()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var deleteReportDto = new DeleteReportCommentArticleDto()
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 2,
        };
        
        //Act
        var res = await _repository.DeleteReportCommentArticleFromDb(deleteReportDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task ModifyReportCommentArticleFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var modifyReportRequestDto = new UpdateReportCommentArticleDto()
        {
            IdUser = 1,
            IdCommentArticle = 2,
            IdReason = 1,
            Description = "Description1",
            Viewed = false
        };

        //Act
        var res = await _repository.UpdateReportCommentArticleFromDb(modifyReportRequestDto);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task ModifyReportCommentArticleFromDb_Returns_NullReport()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataToDb();
        var modifyReportRequestDto = new UpdateReportCommentArticleDto
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 2,
            Description = "Description1",
            Viewed = false
        };

        //Act
        var res = await _repository.UpdateReportCommentArticleFromDb(modifyReportRequestDto);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task ModifyReportCommentArticleFromDb_Returns_ReportCommentArticle()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataToDb();

        var modifyReportRequestDto = new UpdateReportCommentArticleDto()
        {
            IdUser = 1,
            IdCommentArticle = 1,
            IdReason = 1,
            Description = "Description1",
            Viewed = false
        };
        //Act
        var res = await _repository.UpdateReportCommentArticleFromDb(modifyReportRequestDto);
        
        //Assert
        Assert.NotNull(res);
        
        var report = await _context.ReportCommentArticles.SingleOrDefaultAsync(e => e.Description == "Description");
        Assert.Null(report);
    }
}