using System.Xml;
using Gryzilla_App.DTO.Requests.Rank;
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

    public async Task<string?> AddNewRank(AddRankDto addRankDto)
    {
        var rank = await _context.Ranks.Where(x => x.Name == addRankDto.Name).SingleOrDefaultAsync();
        if (rank is not null) return null;
        
       await _context.Ranks.AddAsync(new Rank
        {
            Name = addRankDto.Name,
            RankLevel = addRankDto.RankLevel
        });
        await _context.SaveChangesAsync();
        return "Added new rank";
    }

    public async Task<string?> ModifyRank(PutRankDto putRankDto, int idRank)
    {
        var rank = await _context.Ranks.Where(x => x.IdRank == idRank).SingleOrDefaultAsync();
        if (rank is null) return null;
        rank.Name = putRankDto.Name;
        rank.RankLevel = putRankDto.RankLevel;
        await _context.SaveChangesAsync();
        return "Modified rank";
    }

    public async Task<string?> DeleteRank(int idRank)
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
        return "Deleted rank";
    }
}