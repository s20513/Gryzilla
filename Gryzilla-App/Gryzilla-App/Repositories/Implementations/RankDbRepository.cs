using System.Xml;
using Gryzilla_App.DTO.Requests.Rank;
using Gryzilla_App.DTOs.Responses.Rank;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class RankDbRepository : IRankDbRepository
{
    private readonly GryzillaContext _context;

    public RankDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<RankDto?> AddNewRank(AddRankDto addRankDto)
    {
        var rank = await _context.Ranks.Where(x => x.Name == addRankDto.Name).SingleOrDefaultAsync();
        if (rank is not null) return null;
        var newRank = new Rank
        {
            Name = addRankDto.Name,
            RankLevel = addRankDto.RankLevel
        };
        await _context.Ranks.AddAsync(newRank);
        await _context.SaveChangesAsync();
        return new RankDto
        {
            idRank = await _context.Ranks.Select(x => x.IdRank).OrderByDescending(x => x).FirstAsync(),
            Name = newRank.Name,
            RankLevel = newRank.RankLevel
        };
    }

    public async Task<RankDto?> ModifyRank(PutRankDto putRankDto, int idRank)
    {
        var rank = await _context.Ranks.Where(x => x.IdRank == idRank).SingleOrDefaultAsync();
        if (rank is null) return null;
        rank.Name = putRankDto.Name;
        rank.RankLevel = putRankDto.RankLevel;
        await _context.SaveChangesAsync();
        return new RankDto
        {
            idRank = idRank,
            RankLevel = rank.RankLevel,
            Name = rank.Name
        };
    }

    public async Task<RankDto?> DeleteRank(int idRank)
    {
        //brak takiej rangi
        var rank = await _context.Ranks.Where(x => x.IdRank == idRank).SingleOrDefaultAsync();
        if (rank is null) return null;
        //czy uzytkownik ma taka range
        var userRank = await _context.UserData
            .Include(x => x.IdRankNavigation)
            .Where(x => x.IdRank == idRank)
            .ToArrayAsync();
        if (userRank.Length > 0) return null;
        _context.Ranks.Remove(rank);
        await _context.SaveChangesAsync();
        return new RankDto
        {
            idRank = idRank,
            RankLevel = rank.RankLevel,
            Name = rank.Name
        };
    }
}