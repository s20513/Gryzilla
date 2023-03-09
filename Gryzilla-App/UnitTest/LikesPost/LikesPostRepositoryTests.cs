using Gryzilla_App;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.LikesPost;

public class LikesPostRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly LikesPostDbRepository _repository;

    public  LikesPostRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new LikesPostDbRepository(_context);
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
        
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            Content = "content",
            HighLight = false,
            CreatedAt = DateTime.Now,
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
        var idPost= 1;
        
        //Act
        var res = await _repository.AddLikeToPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Added like");
    }
    
    [Fact]
    public async Task AddNewLikeToDb_Returns_LikeAssigned()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idPost = 1;
        
        var post = await _context.Posts.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        post.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.AddLikeToPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Like has been assigned");
    }
    
    [Fact]
    public async Task AddNewLikeToDb_Returns_PostOrUserDoesntExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        var idPost = 1;
        
        //Act
        var res = await _repository.AddLikeToPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Post or user doesn't exist");
    }
    
    [Fact] 
    public async Task DeleteLikeToDb_Returns_DeletedLike()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idPost = 1;
        
        var post = await _context.Posts.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        post.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.DeleteLikeFromPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Deleted like");
    }
    
    [Fact]
    public async Task DeleteLikeToDb_Returns_LikeHasNotAssigned()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneLike();

        var idUser = 1;
        var idPost = 1;
        
        //Act
        var res = await _repository.DeleteLikeFromPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Like has not been assigned");
    }
    
    [Fact]
    public async Task DeleteLikeToDb_Returns_PostorUserDoesntExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idUser = 1;
        var idPost = 1;
        
        //Act
        var res = await _repository.DeleteLikeFromPost(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res == "Post or user doesn't exist");
    }
    
    
    [Fact]
    public async Task ExistLikeToDb_Returns_LikeExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneLike();
        var idUser = 1;
        var idPost = 1;
        var post = await _context.Posts.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        post.IdUsers.Add(user);
        await _context.SaveChangesAsync();
        
        //Act
        var res = await _repository.ExistLike(idUser, idPost);
        
        //Assert
        Assert.NotNull(res);
        Assert.True(res.liked.Equals(true));
    }
    [Fact]
    public async Task ExistLikeToDb_Returns_PostOrUserDoesNotExist()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        var idUser = 1;
        var idPost = 1;
        
        //Act
        var res = await _repository.ExistLike(idUser, idPost);
        
        //Assert
        Assert.Null(res);
    }
}