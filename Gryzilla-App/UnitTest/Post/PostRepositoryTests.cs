using Gryzilla_App;
using Gryzilla_App.DTO.Responses.Posts;
using Gryzilla_App.DTOs.Requests.Post;
using Gryzilla_App.DTOs.Responses.Posts;
using Gryzilla_App.Exceptions;
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

        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Posts.AddAsync(new Gryzilla_App.Post
        {
            IdUser = 1,
            CreatedAt = DateTime.Today,
            Content = "Content1",
            HighLight = false
        });
        await _context.SaveChangesAsync();
        await _context.Tags.AddAsync(new Gryzilla_App.Tag
        {
            NameTag = "Tag1"
        });
        await _context.SaveChangesAsync();

        var post = await _context.Posts.FirstAsync();
        var user = await _context.UserData.FirstAsync();
        var tag = await  _context.Tags.FirstAsync();
        
        await _context.CommentPosts.AddAsync(new Gryzilla_App.CommentPost
        {
            IdUser = 1,
            DescriptionPost = "Description",
            IdPost = 1
        });

        await _context.Reasons.AddAsync(new Gryzilla_App.Reason
        {
            ReasonName = "ReasonName"
        });
        
        post.IdUsers.Add(user);        
        post.IdTags.Add(tag);
        await _context.SaveChangesAsync();
        
        await _context.ReportPosts.AddAsync(new Gryzilla_App.ReportPost
        {
            Description = "Description",
            ReportedAt = DateTime.Now,
            Viewed = false,
            IdUser = 1,
            IdPost = 1,
            IdReason = 1
        });
        
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
        Assert.False(posts.Exists(e => res != null && e.IdPost == res.IdPost));
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
        Assert.True(tags.Exists(e => res != null && e.IdTag == res.IdTag));
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
    public async Task ModifyPostWithoutTagsFromDb_Returns_PostDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        var putPostDto = new PutPostDto
        {
            IdPost = 1,
            Content = "Content1",
            Tags = new string[]
            {
            }
        };
        const int idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.NotNull(res);

        var post = await _context.Posts.SingleOrDefaultAsync(e =>
            e.IdPost     == idPost
            && e.Content == putPostDto.Content);

        Assert.NotNull(post);
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
            Content = "Content1",
            Tags = new string[]
            {
                "tag",
                "tagi"
            }
        };
        const int idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.NotNull(res);

        var post = await _context.Posts.SingleOrDefaultAsync(e =>
            e.IdPost     == idPost
            && e.Content == putPostDto.Content);

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
            Content = "Content1",
            Tags = new string []
            {
                "testTag", "Tag1"
            }
        };
        var idPost = 1;
        
        //Act
        var res = await _repository.ModifyPostFromDb(putPostDto,idPost);

        //Assert
        Assert.NotNull(res);

        var post = await _context.Posts.SingleOrDefaultAsync(e =>
            e.IdPost     == idPost
            && e.Content == putPostDto.Content);

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
            Content = "Content1",
            Tags = new string[]
            {
               "tag"
            }
        };
        
        const int idPost = 1;

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
        
        var posts = _context.Posts.Where(x=>x.IdPost == idPost).ToList();
        Assert.Single(posts);
        
        var post = posts.SingleOrDefault(e => res != null && e.IdPost == res.idPost);
        Assert.NotNull(post);
    }
    
    [Fact]
    public async Task GetPostFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();
        
        const int idPost = 7;
        
        //Act
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
            Tags = new []
            {
                
                "Tag1",
                "Tag2"
            }
        };
        
        //Act
        var res = await _repository.AddNewPostToDb(newPostRequestDto);
        
        //Assert
        Assert.NotNull(res);

        var posts = _context.Posts.ToList();
        Assert.True(posts.Exists(e => res != null && e.IdPost == res.IdPost));
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
            Tags = new []
            {
                "Tag1"
            }
        };

        //Act
        var res = await _repository.AddNewPostToDb(newPostRequestDto);
        
        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetQtyPostsFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.posts.Select(x=> x.idPost));
    }
    [Fact]
    public async Task GetQtyPostsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyPostsFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyPostsFromDb(4));
    }
    [Fact]
    public async Task GetPostsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataToDb();

        //Act
        var res = await _repository.GetPostsFromDb();

        //Assert
        Assert.NotNull(res);

        var posts = await _context.Posts.Select(e => e.IdPost).ToListAsync();
        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    
    [Fact]
    public async Task GetPostsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostsFromDb();

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetQtyPostsByMostLikesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetQtyPostsByLikesFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.posts.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetTopPost_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetTopPosts();

        //Assert
        Assert.Null(res);
    }
    
    
    [Fact]
    public async Task GetTopPost_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetTopPosts();
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(3)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    
    [Fact]
    public async Task GetQtyPostsByMostLikesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyPostsByLikesFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByMostLikesFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyPostsByLikesFromDb(4));
    }
    [Fact]
    public async Task GetPostsByMostLikesFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetPostsByLikesFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetPostsByMostLikesFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostsByLikesFromDb();

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByCommentsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetQtyPostsByCommentsFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.posts.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetQtyPostsByCommentsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyPostsByCommentsFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByCommentsFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyPostsByCommentsFromDb(4));
    }
    [Fact]
    public async Task GetPostsByCommentsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetPostsByCommentsFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetPostsByCommentsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostsByCommentsFromDb();

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByDateFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetQtyPostsByDateFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(5)
            .OrderBy(e => e.CreatedAt)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.posts.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetQtyPostsByDateFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyPostsByDateFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByDateFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyPostsByDateFromDb(4));
    }
    [Fact]
    public async Task GetPostsByDateFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetPostsByDateFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .OrderBy(e => e.CreatedAt)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetPostsByDateFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostsByDateFromDb();

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetQtyPostsByDateOldestFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetQtyPostsByDateOldestFromDb(5);
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Skip(0)
            .Take(5)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.posts.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetQtyPostsByDateOldestFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetQtyPostsByDateOldestFromDb(5);

        //Assert
        Assert.Null(res);
    }
    [Fact]
    public async Task GetPostsByDateOldestFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetQtyPostsByDateOldestFromDb(4));
    }
    [Fact]
    public async Task GetPostsByDateOldestFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataToDb();

        //Act
        var res = await _repository.GetPostsByDateOldestFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.IdPost)
            .ToListAsync();


        if (res != null) Assert.Equal(posts, res.Select(e => e.idPost));
    }
    [Fact]
    public async Task GetPostsByDateOldestFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostsByDateOldestFromDb();

        //Assert
        Assert.Null(res);
    }
}
