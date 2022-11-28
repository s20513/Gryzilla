using Gryzilla_App.DTOs.Requests.Tag;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Tag;

public class TagRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly TagDbRepository _repository;

    public TagRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();
        
        _context = new GryzillaContext(options, true);
        _repository = new TagDbRepository(_context);
    }

    private async Task AddTestDataWithOneTag()
    {
        await _context.Tags.AddAsync(new Gryzilla_App.Models.Tag
        {
            NameTag = "Name1"
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddNewTagToDb_Returns_TagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneTag();

        var newTagRequestDto = new NewTagDto
        {
            Name = "test"
        };

        //Act
        var res = await _repository.AddTagToDb(newTagRequestDto);

        //Assert
        Assert.NotNull(res);

        var tags = _context.Tags.ToList();
        Assert.True(tags.Exists(e => e.IdTag == res.Id));
    }
    
    [Fact]
    public async Task AddNewTagToDb_Returns_SameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneTag();

        var newTagRequestDto = new NewTagDto
        {
            Name = "Name1"
        };

        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddTagToDb(newTagRequestDto));
    }
    
        
    [Fact]
    public async Task GetTagsFromDb_Returns_TagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneTag();

        //Act
        var res = await _repository.GetTagsFromDb();
        
        //Assert
        Assert.NotNull(res);
        
        var tags = await _context.Tags.Select(e => e.IdTag).ToListAsync();
        Assert.Equal(tags, res.Select(e => e.Id));;
    }
    
    [Fact]
    public async Task GetTagsFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        //Act
        var res = await _repository.GetTagsFromDb();

        //Assert
        Assert.Null(res);
    }
    
    [Fact]
    public async Task GetTagFromDb_Returns_TagDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneTag();

        var idTag = 1;
        //Act
        var res = await _repository.GetTagFromDb(idTag);
        
        //Assert
        Assert.NotNull(res);
        
        var tags = _context.Tags.ToList();
        Assert.Single(tags);
        
        var tag = tags.SingleOrDefault(e => e.IdTag == res.Id);
        Assert.NotNull(tag);
    }
    
    
    [Fact]
    public async Task GetTagFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idTag = 1;
        //Act
        var res = await _repository.GetTagFromDb(idTag);

        //Assert
        Assert.Null(res);

    }
}