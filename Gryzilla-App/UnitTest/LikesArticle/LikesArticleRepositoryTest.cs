using Gryzilla_App;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.LikesArticle;

public class LikesArticleRepositoryTest
{
    private readonly GryzillaContext _context;
    private readonly LikesArticleDbRepository _repository;

    public  LikesArticleRepositoryTest()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new LikesArticleDbRepository(_context);
    }
    
    private async Task AddTestDataWithOneLike()
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
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle);
        
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
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle);
        
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
        var res = await _repository.DeleteLikeFromArticle(idUser, idArticle);
        
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
        Assert.True(res.Equals(true));
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