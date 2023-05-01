using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Search;

public class SearchRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly SearchDbRepository _repository;

    public SearchRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new SearchDbRepository(_context);
    }

    private async Task AddPostDataToDb()
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
            Content = "Content2",
            HighLight = false
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
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "1"
        });
        await _context.SaveChangesAsync();

        var tag = await _context.Tags.FirstAsync();
        var post = await _context.Posts.FirstAsync();
        post.IdTags.Add(tag);
        await _context.SaveChangesAsync();
    }
    
    private async Task AddTestDataWithOneUser()
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
            Password = "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
    }
    private async Task AddTestDataWithManyArticles()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Models.Rank()
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

        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "Content2"
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "Content1"
        });
        await _context.SaveChangesAsync();
        
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "1"
        });
        await _context.SaveChangesAsync();

        var tag = await _context.Tags.FirstAsync();
        var article = await _context.Articles.FirstAsync();
        article.IdTags.Add(tag);
        await _context.SaveChangesAsync();
    }
    

    [Fact]
    public async Task GetQtyPostsByWordFromDb_Returns_Null()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetPostByWordFromDb(5, time, "samochody1");

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetQtyPostsByWordFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetPostByWordFromDb(4, time, "samochody1"));
    }
    
    [Fact]
    public async Task GetPostsByWordFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddPostDataToDb();

        //Act
        var res = await _repository.GetPostByWordFromDb(5,DateTime.Now, "1");
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Where(x=>x.Content.ToLower().Contains("1"))
            .Select(e => e.IdPost)
            .CountAsync();

        var tags =  await _context.Tags
            .Where(x => x.NameTag.ToLower().Contains("1"))
            .SelectMany(x => x.IdPosts)
            .Select(x => x.IdPost).CountAsync();

        if (res != null) Assert.Equal(posts+tags, res.Posts.Count());
    }
    
    [Fact]
    public async Task GetQtyArticlesByWordFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetArticleByWordFromDb(5, DateTime.Now, "samochody");

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetQtyArticlesByWordFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetArticleByWordFromDb(4, DateTime.Now, "Samochody"));
    }
    
    [Fact]
    public async Task GetQtyArticlesByWordFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();
        
        DateTime time = DateTime.Now;

        //Act
        var res = await _repository.GetArticleByWordFromDb(5,time, "1");
        
        //Assert
        Assert.NotNull(res);
        
        var articles = await _context
            .Articles
            .Where(x=>x.Content.ToLower().Contains("1"))
            .Select(e => e.IdArticle)
            .CountAsync();

        var tags =  await _context.Tags
            .Where(x => x.NameTag.ToLower().Contains("1"))
            .SelectMany(x => x.IdArticles)
            .Select(x => x.IdArticle).CountAsync();
        
        if (res != null) Assert.Equal(articles +tags, res.Articles.Count());
    }
    
    [Fact]
    public async Task GetQtyUsersByNameFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetUsersByNameFromDb(5, DateTime.Now, "samochody");

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetQtyUserByNickNameFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetUsersByNameFromDb(4, DateTime.Now, "Nick"));
    }
    
    [Fact]
    public async Task GetQtyUserByNickFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneUser();
        
        DateTime time = DateTime.Now;

        //Act
        var res = await _repository.GetUsersByNameFromDb(5,time, "Nick1");
        
        //Assert
        Assert.NotNull(res);
        
        var users = await _context
            .UserData
            .Where(x=>x.Nick.ToLower().Contains("Nick1"))
            .Skip(0)
            .Take(5)
            .Select(e => e.IdUser)
            .ToListAsync();


        if (res != null) Assert.Equal(users, res.Users.Select(e => e.IdUser));
    }
}