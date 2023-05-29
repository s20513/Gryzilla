using System.Security.Claims;
using Gryzilla_App;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.LikesArticle;

public class LikesArticleRepositoryTest
{
    private readonly GryzillaContext _context;
    private readonly LikesArticleDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public  LikesArticleRepositoryTest()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new LikesArticleDbRepository(_context);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
    }
    
    private async Task AddTestDataWithOneLike()
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
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        

    }
    
    
    [Fact]
    public async Task AddNewLikeToDb_Returns_AddedLike()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idArticle = 1;
        
        //Act
        var res = await _repository.AddLikeToArticle(idUser, idArticle);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Added like");
    }
    
    [Fact]
    public async Task AddNewLikeToDb_Returns_LikeAssigned()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idArticle = 1;
        
        var article = await _context.Articles.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        article.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        //Act
        var res = await _repository.AddLikeToArticle(idUser, idArticle);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Like has been assigned");
    }
    [Fact]
    public async Task AddNewLikeToDb_Returns_ArticleOrUserDoesntExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        var idArticle = 1;
        
        //Act
        var res = await _repository.AddLikeToArticle(idUser, idArticle);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Article or user doesn't exist");
    }

    [Fact] 
    public async Task DeleteLikeToDb_Returns_DeletedLike()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idArticle = 1;
        
        var article = await _context.Articles.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        article.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Deleted like");
    }
    
    [Fact]
    public async Task DeleteLikeToDb_Returns_LikeHasNotAssigned()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idArticle = 1;
        
        //Act
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Like has not been assigned");
    }
    
    [Fact]
    public async Task DeleteLikeToDb_Returns_ArticleorUserDoesntExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        var idArticle = 1;
        
        //Act
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.ToString() == "Article or user doesn't exist");
    }
    
    [Fact]
    public async Task ExistLikeToDb_Returns_LikeExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneLike();
        var idUser = 1;
        var idArticle = 1;
        
        var article = await _context.Articles.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        article.IdUsers.Add(user);
        
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.ExistLikeArticle(idUser, idArticle);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.liked.Equals(true));
    }
    [Fact]
    public async Task ExistLikeToDb_Returns_ArticleOrUserDoesNotExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        var idUser = 1;
        var idArticle = 1;
        
        
        //Act
        var res = await _repository.ExistLikeArticle(idUser, idArticle);
        
        //Assert
        Assert.Null(res);
    }
}