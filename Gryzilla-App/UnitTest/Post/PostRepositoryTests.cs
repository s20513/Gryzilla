using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Post;

public class PostRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly PostDbRepository _repository;

    public PostRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new PostDbRepository(_context);
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
            Title = "Title1",
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();

        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "Tag1"
        });
        await _context.SaveChangesAsync();

        var post = await _context.Posts.FirstAsync();
        var tag = await  _context.Tags.FirstAsync();
        
        post.IdTags.Add(tag);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task DeletePostFromDb_Returns_TagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idPost = 1;

        //Act
        var res = await _repository.DeletePostFromDb(idPost);

        //Assert
        Assert.NotNull(res);

        var posts = _context.Posts.ToList();
        Assert.False(posts.Exists(e => e.IdPost == res.IdPost));
    }

    [Fact]
    public async Task DeletePostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idPost = 3;

        //Act
        var res = await _repository.DeletePostFromDb(idPost);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task DeleteTagFromPost_Returns_TagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var idPost = 1;
        var idTag  = 1;

        //Act
        var res = await _repository.DeleteTagFromPost(idPost, idTag);

        //Assert
        Assert.NotNull(res);

        var tags = _context.Tags.ToList();
        Assert.True(tags.Exists(e => e.IdTag == res.IdTag));
    }

    [Fact]
    public async Task DeleteTagFromPost_Returns_NullTag()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        var idPost = 1;
        var idTag  = 1;
        
        var post = await _context.Posts.FirstAsync();
        var tag = await  _context.Tags.FirstAsync();
        
        post.IdTags.Remove(tag);
        await _context.SaveChangesAsync();
        //Act
        var res = await _repository.DeleteTagFromPost(idPost, idTag);

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task DeleteTagFromPost_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idPost = 1;
        var idTag  = 1;

        //Act
        var res = await _repository.DeleteTagFromPost(idPost, idTag);

        //Assert
        Assert.Null(res);
    }
    
        
    [Fact]
    public async Task ModifyPostFromDb_Returns_PostDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var putPostDto = new PutPostDto
        {
            IdPost = 1,
            Title = "Title1",
            Content = "Content1",
            Tags = new  TagByIdDto []
            {
                new TagByIdDto
                {
                    IdTag = 1
                }
            }
        };
        var idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.NotNull(res);

        var post = await _context.Posts.SingleOrDefaultAsync(e =>
            e.IdPost     == idPost
            && e.Content == putPostDto.Content
            && e.Title   == putPostDto.Title);

        Assert.NotNull(post);
    }
    
    [Fact]
    public async Task ModifyPostFromDb_Returns_WithoutTagPostDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var putPostDto = new PutPostDto
        {
            IdPost = 1,
            Title = "Title1",
            Content = "Content1",
            Tags = new  TagByIdDto []
            {
            }
        };
        var idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.NotNull(res);

        var post = await _context.Posts.SingleOrDefaultAsync(e =>
            e.IdPost     == idPost
            && e.Content == putPostDto.Content
            && e.Title   == putPostDto.Title);

        Assert.NotNull(post);
    }

    [Fact]
    public async Task ModifyPostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var putPostDto = new PutPostDto
        {
            IdPost = 1,
            Title = "Title1",
            Content = "Content1",
            Tags = new  TagByIdDto []
            {
                new TagByIdDto
                {
                    IdTag = 1
                }
            }
        };
        var idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetPostFromDb_Returns_PostDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var idPost = 1;
        
        //Act
        var res = await _repository.GetOnePostFromDb(idPost);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = _context.Posts.ToList();
        Assert.Single(posts);
        
        var post = posts.SingleOrDefault(e => e.IdPost == res.idPost);
        Assert.NotNull(post);
    }
    
    [Fact]
    public async Task GetPostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var idPost= 2;
        
        ///Act
        var res = await _repository.GetOnePostFromDb(idPost);
        
        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task AddNewPostToDb_Returns_PostDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        var newPostRequestDto = new AddPostDto()
        {
            IdUser = 1,
            Content = "Content",
            Title = "Title",
            Tags = new TagDto []
            {
                new TagDto
                {
                    NameTag = "Tag1"
                }
            }
        };
        //Act
        var res = await _repository.AddNewPostToDb(newPostRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var posts = _context.Posts.ToList();
        Assert.True(posts.Exists(e => e.IdPost == res.IdPost));
    }
    
    [Fact]
    public async Task AddNewGroupToDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var newPostRequestDto = new AddPostDto()
        {
            IdUser = 1,
            Content = "Content",
            Title = "Title",
            Tags = new TagDto []
            {
                new TagDto
                {
                    NameTag = "Tag1"
                }
            }
        };

        //Act
        var res = await _repository.AddNewPostToDb(newPostRequestDto);
        
        //Assert
        Assert.Null(res);
    }
}