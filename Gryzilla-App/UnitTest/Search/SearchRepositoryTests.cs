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
    private async Task AddTestDataWithManyGroup()
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

        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass1",
            Email = "email1",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
        
        var user = await _context.UserData.FirstAsync();
        
        await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "test",
            CreatedAt = DateTime.Now,
            IdUserCreator = 2,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
        
        var group = await _context.Groups.FirstAsync();

        await _context.GroupUsers.AddAsync(new GroupUser
        {
            IdGroup = group.IdGroup,
            IdUser = user.IdUser
        });
        await _context.SaveChangesAsync();

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "nowa",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "grupa"
        });
        await _context.SaveChangesAsync();

        await _context.Groups.AddAsync(new Group
        {
            GroupName = "3",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
        await _context.Groups.AddAsync(new Group
        {
            GroupName = "4",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
        await _context.Groups.AddAsync(new Group
        {
            GroupName = "5",
            CreatedAt = DateTime.Now,
            IdUserCreator = 1,
            Description = "Nowa grupa"
        });
        await _context.SaveChangesAsync();
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
            Title = "Title",
            CreatedAt = DateTime.Now,
            Content = "1"
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "2"
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "3"
        });
        await _context.SaveChangesAsync();
        
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "4"
        });
        await _context.SaveChangesAsync();
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title1",
            CreatedAt = DateTime.Now,
            Content = "5"
        });
        await _context.SaveChangesAsync();
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "Content"
        });
        await _context.Articles.AddAsync(new Gryzilla_App.Models.Article
        {
            IdUser = 1,
            Title = "Title2",
            CreatedAt = DateTime.Now,
            Content = "6"
        });
        await _context.SaveChangesAsync();
       
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "Content"
        });
        await _context.SaveChangesAsync();

        var tag = await _context.Tags.FirstAsync();
        var article = await _context.Articles.FirstAsync();
        var article1 = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == 2);
        var article2 = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == 3);
        var article3 = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == 4);
        var article4 = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == 5);
        var article5 = await _context.Articles.SingleOrDefaultAsync(x => x.IdArticle == 6);
        article.IdTags.Add(tag);
        article1.IdTags.Add(tag);
        article2.IdTags.Add(tag);
        article3.IdTags.Add(tag);
        article4.IdTags.Add(tag);
        article5.IdTags.Add(tag);
        await _context.SaveChangesAsync();
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
        var res = await _repository.GetArticleByWordFromDb(5,time, "Content");
        
        //Assert
        Assert.NotNull(res);
        
        if (res != null) Assert.Equal(5, res.Articles.Count());
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
    
    [Fact]
    public async Task GetQtyUserByNick_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithOneUser();
        

        //Act
        var res = await _repository.GetUsersByName("Nick1");
        
        //Assert
        Assert.NotNull(res);
        
        var users = await _context
            .UserData
            .Where(x=>x.Nick.ToLower().Contains("Nick1"))
            .Select(e => e.IdUser)
            .ToListAsync();


        if (res != null) Assert.Equal(users, res.Select(e => e.IdUser));
    }
    
    [Fact]
    public async Task GetQtyArticlesByTagFromDb_Returns_WrongNumberException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetArticlesByTagFromDb(4, DateTime.Now, "Samochody"));
    }
    
    [Fact]
    public async Task GetQtyArticlesByTagFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyArticles();
        
        DateTime time = DateTime.Now;

        //Act
        var res = await _repository.GetArticlesByTagFromDb(5,time, "Content");
        
        //Assert
        Assert.NotNull(res);
        

        if (res != null) Assert.Equal(5 ,res.Articles.Count());
    }
    
    [Fact]
    public async Task GetQtyPostsByTagFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetPostsByTagFromDb(4, time, "samochody1"));
    }
    [Fact]
    public async Task GetPostsByTagsFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddPostDataToDb();

        //Act
        var res = await _repository.GetPostsByTagFromDb(5,DateTime.Now, "1");
        
        //Assert
        Assert.NotNull(res);
        
        var posts = await _context
            .Posts
            .Where(x=>x.IdPost==1)
            .OrderByDescending(e => e.IdUsers.Count)
            .Select(e => e.IdPost)
            .SingleOrDefaultAsync();


        if (res != null) Assert.Equal(posts, res.Posts.Select(e => e.idPost).SingleOrDefault());
    }
    
    [Fact]
    public async Task GetQtyGroupsByWordFromDb_Returns_WrongNumberException()
    {
        //Arrange
        DateTime time = DateTime.Now;
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        //Assert
        await Assert.ThrowsAsync<WrongNumberException>(() => _repository.GetGroupsByWordFromDb(4, time, "samochody1"));
    }
    
    [Fact]
    public async Task GetQtyGroupsByWordFromDb_Returns_IEnumerable()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        
        await AddTestDataWithManyGroup();

        //Act
        var res = await _repository.GetGroupsByWordFromDb(5,DateTime.Now, "nowa");
        
        //Assert
        Assert.NotNull(res);
        
        var groups = await _context
            .Groups
            .Where(x=>x.IdGroup==1)
            .Select(e => e.IdGroup)
            .SingleOrDefaultAsync();


        if (res != null) Assert.Equal(groups, res.Groups.Select(e => e.IdGroup).FirstOrDefault());
    }
}