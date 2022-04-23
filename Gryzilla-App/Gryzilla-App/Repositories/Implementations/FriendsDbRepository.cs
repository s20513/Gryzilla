using Gryzilla_App.DTO.Responses;
using Gryzilla_App.DTO.Responses.Friends;
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
    
    public async Task<IEnumerable<FriendDto>> GetFriendsFromDb(int idUser)
    {

        var users = await _context.UserData.Where(x => x.IdUser == idUser)
            .SelectMany(c => c.IdUserFriends)
            .Select(x => new FriendDto
            {
                IdUser = x.IdUser,
                Nick = x.Nick
            }).ToArrayAsync();
        
        return users;
    }
}