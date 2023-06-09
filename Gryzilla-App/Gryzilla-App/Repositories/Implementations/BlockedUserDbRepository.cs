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
                End = null,
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
            .Where(e => e.IdUser == blockedUserRequestDto.IdUserBlocked)
            .SingleOrDefaultAsync();

        if (blockedUserData is null || blockedUserData.IdRankNavigation.Name == "Admin")
        {
            return null;
        }

        var userBlock = await _context.BlockedUsers
            .Where(e => e.IdUserBlocked == blockedUserRequestDto.IdUserBlocked)
            .Select(e => new
            {
                e.IdUser,
                Start = EF.Property<DateTime>(e, "StartTime"),
                e.Comment
            })
            .SingleOrDefaultAsync();

        if (userBlock is not null)
        {
            return new BlockedUserDto
            {
                IdUserBlocked = blockedUserRequestDto.IdUserBlocked,
                Nick = blockedUserData.Nick,
                IdRank = blockedUserData.IdRank,
                RankName = blockedUserData.IdRankNavigation.Name,
                IdUserBlocking = userBlock.IdUser,
                Start = userBlock.Start,
                End = null,
                Comment = userBlock.Comment
            };
        }

        var blockedRankId = await _context.Ranks
            .Where(e => e.Name == "Blocked")
            .Select(e => e.IdRank)
            .SingleAsync();

        var blockedUser = new BlockedUser
        {
            IdUser = blockedUserRequestDto.IdUserBlocking,
            IdUserBlocked = blockedUserRequestDto.IdUserBlocked,
            Comment = blockedUserRequestDto.Comment,
            UserBlockedRankId = blockedUserData.IdRank
        };

        blockedUserData.IdRank = blockedRankId;

        await _context.BlockedUsers.AddAsync(blockedUser);
        await _context.SaveChangesAsync();

        return new BlockedUserDto
        {
            IdUserBlocked = blockedUserRequestDto.IdUserBlocked,
            Nick = blockedUserData.Nick,
            IdRank = blockedUserData.IdRank,
            RankName = _context.Ranks
                .Where(x=>x.IdRank ==blockedUserData.IdRank).Select(x=>x.Name).SingleOrDefault(),
            IdUserBlocking = blockedUserRequestDto.IdUserBlocking,
            Start = DateTime.Now,
            End = null,
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

        var userData = await _context.UserData.SingleAsync(e => e.IdUser == idUser);

        userData.IdRank = blockedUser.UserBlockedRankId;

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
            .Join(_context.UserData, blockedUser => blockedUser.IdUser, datum => datum.IdUser, (blockedUser, datum) =>
                new
                {
                    IdUserBlocking = datum.IdUser,
                    UserBlockingNick = datum.Nick,
                    UserBlockingIdRank = datum.IdRank,
                    Start = EF.Property<DateTime>(blockedUser, "StartTime"),
                    End = EF.Property<DateTime>(blockedUser, "EndTime"),
                    blockedUser.Comment 
            }).Join(_context.Ranks, arg => arg.UserBlockingIdRank, rank => rank.IdRank, (arg, rank) =>
                new BlockingUserDto
                {
                    IdUserBlocking = arg.IdUserBlocking,
                    UserBlockingNick = arg.UserBlockingNick,
                    UserBlockingIdRank = arg.UserBlockingIdRank,
                    UserBlockingRankName = rank.Name,
                    Start = arg.Start,
                    End = arg.End.Date.Year == 9999 ? null : arg.End,
                    Comment = arg.Comment
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

    public async Task<bool?> CheckIfUserIsBlocked(int idUser)
    {
        var userData = await _context.UserData
            .AnyAsync(e => e.IdUser == idUser);

        if (!userData)
        {
            return null;
        }
        
        return await _context.BlockedUsers
            .AnyAsync(e => e.IdUserBlocked == idUser);
    }
}