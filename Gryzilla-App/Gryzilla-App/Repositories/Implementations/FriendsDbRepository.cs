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
    
    public async Task<IEnumerable<FriendDto>?> GetFriendsFromDb(int idUser)
    {
        //sprawdzamy czy użytkownik podany znajduje się w naszej bazie
        var user = await _context.UserData.Where(x => x.IdUser == idUser).SingleOrDefaultAsync();
        //jeśli nie ma go zwracamy null
        if (user is null) return null;
        {
            //szukamy listy asocjacyjnej znajomych naszego użytkownika
            var users = await _context.UserData.Where(x => x.IdUser == idUser)
                .SelectMany(c => c.IdUserFriends)
                .Select(x => new FriendDto
                {
                    IdUser = x.IdUser,
                    Nick = x.Nick
                }).ToArrayAsync();
            //zwracamy
            return users;
        }
    }
    public async Task<string?> DeleteFriendFromDb (int idUser, int idUserFriend)
    {
        //szukamy listy asocjacyjnej friends naszego usera 
        var user = await _context.UserData.Include(x => x.IdUserFriends)
            .Where(a => a.IdUser== idUser)
            .SingleOrDefaultAsync();
        //szukamy listy asocjacyjnej friends naszego przyjaciela 
        var userFriend = await _context.UserData.Include(c => c.IdUserFriends)
            .Where(a => a.IdUser== idUserFriend)
            .SingleOrDefaultAsync();
        //sprawdzamy czy nie są przypadkiem nullem
        if (user is null || userFriend is null) return null;
        //usuwamy przyjaciela po stronie użytkownika
        user.IdUserFriends.Remove(userFriend);
        //usuwamy użytkownika po stronie przyjaciela
        userFriend.IdUserFriends.Remove(user);
        //zapisujemy zmiany
        await _context.SaveChangesAsync();
        return "friend deleted";
    }
}
