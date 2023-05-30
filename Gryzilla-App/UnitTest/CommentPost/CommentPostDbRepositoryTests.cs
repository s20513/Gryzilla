using System.Security.Claims;
using Gryzilla_App;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest.CommentPost;

public class CommentPostDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly CommentPostDbRepository _repository;
    private readonly Mock<ClaimsPrincipal> _mockClaimsPrincipal;

    public CommentPostDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new CommentPostDbRepository(_context);
        
        _mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "User"),
        };
        _mockClaimsPrincipal.Setup(x => x.Claims).Returns(claims);
        _mockClaimsPrincipal
            .Setup(x => x.FindFirst(It.IsAny<string>()))
            .Returns<string>(claimType => claims.FirstOrDefault(c => c.Type == claimType));
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
    }

    [Fact]
    public async void AddCommentToPost_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 1,
            IdPost = 1,
            Content = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.True(postComments.Count == 2);
        
        var postComment = postComments.SingleOrDefault(e => 
                                                    e.IdComment       == res.IdComment &&
                                                    e.IdPost          == res.IdPost &&
                                                    e.IdUser          == res.IdUser &&
                                                    e.DescriptionPost == res.Content);
        Assert.NotNull(postComment);
    }
    
    [Fact]
    public async void AddCommentToPost_With_No_Existing_Post_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 1,
            IdPost = 1,
            Content = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void AddCommentToPost_With_No_Existing_User_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        var newPostCommentDto = new NewPostCommentDto
        {
            IdUser = 2,
            IdPost = 1,
            Content = "DescPost1"
        };
        
        //Act
        var res = await _repository.AddCommentToPost(newPostCommentDto);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void ModifyPostCommentFromDb_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var idComment = 1;
        
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = idComment,
            IdPost = 1,
            Content = "NewDescPost1"
        };
        
        //Act
        var res = await _repository.ModifyPostCommentFromDb(putPostCommentDto, idComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Single(postComments);
        
        var postComment = postComments.SingleOrDefault(e => 
            e.IdComment       == res.IdComment &&
            e.IdPost          == res.IdPost &&
            e.IdUser          == res.IdUser &&
            e.DescriptionPost == res.Content);
        Assert.NotNull(postComment);
    }
    
    [Fact]
    public async void ModifyPostCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var idComment = 2;
        
        var putPostCommentDto = new PutPostCommentDto
        {
            IdComment = idComment,
            IdPost = 1,
            Content = "NewDescPost1"
        };
        
        //Act
        var res = await _repository.ModifyPostCommentFromDb(putPostCommentDto, idComment, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void DeleteCommentFromDb_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 1;
        
        //Act
        var res = await _repository.DeleteCommentFromDb(id, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.ToList();
        Assert.Empty(postComments);
    }
    
    [Fact]
    public async void DeleteCommentFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 2;
        
        //Act
        var res = await _repository.DeleteCommentFromDb(id, _mockClaimsPrincipal.Object);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async void GetCommentsPost_Returns_PostCommentDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var id = 1;
        
        //Act
        var res = await _repository.GetPostCommentsFromDb(id);
        
        //Assert
        Assert.NotNull(res);
        
        var postComments = _context.CommentPosts.Where(x=>x.IdPost==id).ToList();
        Assert.True(postComments.Count == res.Comments.Count());
       
        Assert.NotNull(res.Comments);
    }
    
    [Fact]
    public async void GetCommentsPost_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var id = 1;
        //Act
        var res = await _repository.GetPostCommentsFromDb(id);
        
        //Assert
        Assert.Null(res);
    }
}