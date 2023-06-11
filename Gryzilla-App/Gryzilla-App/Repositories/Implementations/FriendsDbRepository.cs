using System.Security.Claims;
using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Friends;
using Gryzilla_App.Exceptions;
using Gryzilla_App.Helpers;
using Gryzilla_App.Models;
using Gryzilla_App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gryzilla_App.Repositories.Implementations;

public class FriendsDbRepository : IFriendsDbRepository
{
    private readonly GryzillaContext _context;
    public FriendsDbRepository(GryzillaContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<FriendDto>?> GetFriendsFromDb(int idUser)
    {
        var user = await _context
            .UserData
            .Where(x => x.IdUser == idUser)
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var users = await _context
            .UserData.Where(x => x.IdUser == idUser)
            .SelectMany(y => y.IdUsers)
            .Select(y => new FriendDto
            {
                IdUser = y.IdUser,
                Nick = y.Nick
            }).ToArrayAsync();
        
        return users;
    }
    public async Task<FriendDto?> DeleteFriendFromDb (int idUser, int idUserFriend, ClaimsPrincipal userClaims)
    {
        UserDatum? user;
        UserDatum? userFriend;

        user = await _context
            .UserData
            .Include(x => x.IdUserFriends)
            .Where(a => a.IdUser== idUser)
            .SingleOrDefaultAsync();
        
        if (user is null || !ActionAuthorizer.IsAuthorOrAdmin(userClaims, user.IdUser))
        {
            return null;
        }
        
        userFriend = await _context
            .UserData
            .Include(c => c.IdUserFriends)
            .Where(a => a.IdUser== idUserFriend)
            .SingleOrDefaultAsync();

        if (userFriend is null)
        {
            return null;
        }
        
        userFriend.IdUserFriends.Remove(user);
        
        await _context.SaveChangesAsync();

        return new FriendDto
        {
            IdUser = idUserFriend,
            Nick = userFriend.Nick
        };
    }

    public async Task<FriendDto?> AddNewFriendToDb(int idUser, int idUserFriend)
    {
        UserDatum? user;
        UserDatum? userFriend;
        
        user = await _context
            .UserData
            .Include(x => x.IdUserFriends)
            .Where(a => a.IdUser== idUser)
            .SingleOrDefaultAsync();
        
        if (user is null)
        {
            return null;
        }
        
        userFriend = await _context
            .UserData
            .Include(x => x.IdUserFriends)
            .Where(a => a.IdUser== idUserFriend )
            .SingleOrDefaultAsync();

        if (userFriend is null)
        {
            return null;
        }

        if (user.IdUserFriends.Contains(userFriend))
        {
            throw new ReferenceException($"{userFriend.Nick} is already {user.Nick} friend!");
        }
        
        userFriend.IdUserFriends.Add(user);
        
        await _context.SaveChangesAsync();
        
        return new FriendDto
        {
            IdUser = idUserFriend,
            Nick = userFriend.Nick
        };
    }
}
