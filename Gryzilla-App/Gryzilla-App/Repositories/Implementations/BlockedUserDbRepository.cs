using Gryzilla_App.DTOs.Requests.BlockedUser;
using Gryzilla_App.DTOs.Responses.BlockedUser;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class BlockedUserDbRepository: IBlockedUserDbRepository
{
    private readonly GryzillaContext _context;

    public BlockedUserDbRepository(GryzillaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlockedUserDto>> GetBlockedUsers()
    {
        return await _context.BlockedUsers
            .Include(e => e.IdUserBlockedNavigation)
            .Include(e => e.IdUserBlockedNavigation.IdRankNavigation)
            .Select(e => new BlockedUserDto
            {
                IdUserBlocked = e.IdUserBlocked,
                Nick = e.IdUserBlockedNavigation.Nick,
                IdRank = e.IdUserBlockedNavigation.IdRank,
                RankName = e.IdUserBlockedNavigation.IdRankNavigation.Name,
                IdUserBlocking = e.IdUser,
                Start = EF.Property<DateTime>(e, "StartTime"),
                Comment = e.Comment
            })
            .ToListAsync();
    }

    public async Task<BlockedUserDto?> BlockUser(BlockedUserRequestDto blockedUserRequestDto)
    {
        var blockingUserData = await _context.UserData
            .AnyAsync(e => e.IdUser == blockedUserRequestDto.IdUserBlocking);

        if (!blockingUserData)
        {
            return null;
        }

        var blockedUserData = await _context.UserData
            .Include(e => e.IdRankNavigation)
            .Select(e => new
            {
                e.Nick,
                e.IdRank,
                RankName = e.IdRankNavigation.Name
            })
            .SingleOrDefaultAsync();
        
        
        if (blockedUserData is null)
        {
            return null;
        }

        var blockedUser = new BlockedUser
        {
            IdUser = blockedUserRequestDto.IdUserBlocking,
            IdUserBlocked = blockedUserRequestDto.IdUserBlocked,
            Comment = blockedUserRequestDto.Comment
        };

        await _context.BlockedUsers.AddAsync(blockedUser);
        await _context.SaveChangesAsync();

        return new BlockedUserDto
        {
            IdUserBlocked = blockedUserRequestDto.IdUserBlocked,
            Nick = blockedUserData.Nick,
            IdRank = blockedUserData.IdRank,
            RankName = blockedUserData.RankName,
            IdUserBlocking = blockedUserRequestDto.IdUserBlocking,
            Start = DateTime.Now,
            Comment = blockedUserRequestDto.Comment
        };
    }

    public async Task<string?> UnlockUser(int idUser)
    {
        var blockedUser = await _context.BlockedUsers
            .Where(e => e.IdUserBlocked == idUser)
            .Include(e => e.IdUserBlockedNavigation)
            .Include(e => e.IdUserBlockedNavigation.IdRankNavigation)
            .SingleOrDefaultAsync();
        
        if (blockedUser is null)
        {
            return null;
        }

        _context.Remove(blockedUser);
        await _context.SaveChangesAsync();

        return "User unlocked";
    }

    public async Task<UserBlockingHistoryDto?> GetUserBlockingHistory(int idUser)
    {
        var user = await _context.UserData
            .Where(e => e.IdUser == idUser)
            .Include(e => e.IdRankNavigation)
            .Select(e => new
            {
                e.Nick,
                e.IdRank,
                e.IdRankNavigation.Name
            })
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var history = await _context.BlockedUsers
            .TemporalAll()
            .Where(e => e.IdUserBlocked == idUser)
            .Include(e => e.IdUserNavigation)
            .Include(e => e.IdUserNavigation.IdRankNavigation)
            .Select(e => new BlockingUserDto
            {
                IdUserBlocking = e.IdUser,
                UserBlockingNick = e.IdUserBlockedNavigation.Nick,
                UserBlockingIdRank = e.IdUserBlockedNavigation.IdRank,
                UserBlockingRankName = e.IdUserBlockedNavigation.IdRankNavigation.Name,
                Start = EF.Property<DateTime>(e, "StartTime"),
                End = EF.Property<DateTime>(e, "EndTime"),
                Comment = e.Comment
            })
            .ToListAsync();

        return new UserBlockingHistoryDto
        {
            IdUser = idUser,
            Nick = user.Nick,
            IdRank = user.IdRank,
            RankName = user.Name,
            History = history
        };
    }
}