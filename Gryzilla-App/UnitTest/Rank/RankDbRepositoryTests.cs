using Gryzilla_App;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace UnitTest.Rank;

public class RankDbRepositoryTests
{
    private readonly GryzillaContext _context;
    private readonly RankDbRepository _repository;

    public RankDbRepositoryTests()
    {
        var options = new DbContextOptions<GryzillaContext>();

        _context = new GryzillaContext(options, true);
        _repository = new RankDbRepository(_context);
    }

    private async Task AddTestDataWithOneRank()
    {
        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Rank1",
            RankLevel = 1
        });

        await _context.SaveChangesAsync();

        await _context.Ranks.AddAsync(new Gryzilla_App.Rank
        {
            Name = "Rank2",
            RankLevel = 1
        });

        await _context.SaveChangesAsync();
        var user = await _context.UserData.AddAsync(new UserDatum
        {
            IdRank = 1,
            Nick = "Nick2",
            Password = "Pass2",
            Email = "email2",
            CreatedAt = DateTime.Today
        });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddNewRankToDb_Returns_RankDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var newRankRequestDto = new AddRankDto()
        {
            Name = "Admin",
            RankLevel = 0
        };


        //Act
        var res = await _repository.AddNewRank(newRankRequestDto);

        //Assert
        Assert.NotNull(res);

        var ranks = _context.Ranks.ToList();
        Assert.True(ranks.Exists(e => e.IdRank == res.IdRank));
    }

    [Fact]
    public async Task AddNewRankToDb_Returns_ThrowSameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var newRankRequestDto = new AddRankDto()
        {
            Name = "Rank2",
            RankLevel = 0
        };

        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.AddNewRank(newRankRequestDto));
    }

    [Fact]
    public async Task ModifyRankFromDb_Returns_RankDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var idRank = 1;

        var modifyRankRequestDto = new PutRankDto()
        {
            Name = "Admin",
            RankLevel = 0
        };

        //Act
        var res = await _repository.ModifyRank(modifyRankRequestDto, idRank);

        //Assert
        Assert.NotNull(res);

        var rank = await _context.Ranks.SingleOrDefaultAsync(e =>
            e.IdRank == idRank
            && e.Name == modifyRankRequestDto.Name
            && e.RankLevel == modifyRankRequestDto.RankLevel);

        Assert.NotNull(rank);
    }

    [Fact]
    public async Task ModifyRankFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        var idRank = 1;

        var modifyRankRequestDto = new PutRankDto()
        {
            Name = "Admin",
            RankLevel = 0
        };

        //Act
        var res = await _repository.ModifyRank(modifyRankRequestDto, idRank);

        //Assert
        Assert.Null(res);
    }

    [Fact]
    public async Task ModifyRankFromDb_Returns_ThrowSameNameException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());
        await AddTestDataWithOneRank();
        var idRank = 1;

        var modifyRankRequestDto = new PutRankDto()
        {
            Name = "Rank2",
            RankLevel = 0
        };

        //Act
        //Assert
        await Assert.ThrowsAsync<SameNameException>(() => _repository.ModifyRank(modifyRankRequestDto, idRank));
    }

    [Fact]
    public async Task DeleteRankFromDb_Returns_RankDto()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var idRank = 2;

        //Act
        var res = await _repository.DeleteRank(idRank);

        //Assert
        Assert.NotNull(res);

        var ranks = _context.Ranks.ToList();
        Assert.False(ranks.Exists(e => e.IdRank == res.IdRank));
    }

    [Fact]
    public async Task DeleteRankFromDb_Returns_Null()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var idRank = 3;

        //Act
        var res = await _repository.DeleteRank(idRank);

        //Assert
        Assert.Null(res);
    }


    [Fact]
    public async Task DeleteRankFromDb_Returns_ThrowReferenceException()
    {
        //Arrange
        await _context.Database.ExecuteSqlRawAsync(DatabaseSql.GetTruncateSql());

        await AddTestDataWithOneRank();

        var idRank = 1;

        //Act
        //Assert
        await Assert.ThrowsAsync<ReferenceException>(() => _repository.DeleteRank(idRank));
    }
}